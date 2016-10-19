using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

public class MediaBrowserSetupWindow : EditorWindow
{
	private const string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
	private const float CheckInterval = 1.0f;
	private const string DefaultAndroidManifest = "AndroidMediaBrowser/Editor/DefaultAndroidManifest.xml";
	
	private const string AndroidManifest = "Plugins/Android/AndroidManifest.xml";
	
	[MenuItem("Diversido/Android Media Browser Setup")]
    static void ShowWindow()
	{
		((MediaBrowserSetupWindow)EditorWindow.GetWindow(typeof(MediaBrowserSetupWindow))).CheckSetup(true);
	}
	
	public MediaBrowserSetupWindow()
	{
		title = "Android Media Browser";
	}

	private static string FullPathTo(string asset)
	{
		return Path.GetFullPath(Path.Combine(Application.dataPath, asset));
	}
	
	private float _lastSetupCheckTime = 0.0f;
	private bool _androidManifestPresent = false;
	
	private bool _manifestAudioPicker = false;
	private bool _manifestVideoPicker = false;
	private bool _manifestImagePicker = false;
	
	private bool _manifestAudioHandler = false;
	private bool _manifestVideoHandler = false;
	private bool _manifestImageHandler = false;
	
	private bool _userAudioPicker = false;
	private bool _userVideoPicker = false;
	private bool _userImagePicker = false;
	
	private bool _userAudioHandler = false;
	private bool _userVideoHandler = false;
	private bool _userImageHandler = false;
	
	private XmlNode FindActivity(XmlDocument doc, string activityName)
	{
		var activities = doc.SelectNodes("/manifest/application/activity");
		for (var i = 0; i < activities.Count; ++i)
		{
			var activity = activities.Item(i);
			var name = activity.Attributes.GetNamedItem("android:name");
			var label = activity.Attributes.GetNamedItem("android:label");
			if (name == null || label == null)
				continue;
			
			if (name.Value == activityName)
				return activity;
		}
		return null;
	}
	
	private XmlNode AddActivity(XmlDocument doc, string activityName, string activityLabel)
	{
		var app = doc.SelectSingleNode("/manifest/application");
		var activity = doc.CreateNode(XmlNodeType.Element, "activity", "");
		
		var name = (XmlAttribute)doc.CreateNode(XmlNodeType.Attribute, "android", "name", AndroidXmlNamespace);
		name.Value = activityName;
		activity.Attributes.Append(name);
		
		var label = (XmlAttribute)doc.CreateNode(XmlNodeType.Attribute, "android", "label", AndroidXmlNamespace);
		label.Value = activityLabel;
		activity.Attributes.Append(label);
		
		var configChanges = (XmlAttribute)doc.CreateNode(XmlNodeType.Attribute, "android", "configChanges", AndroidXmlNamespace);
		configChanges.Value = "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen";
		activity.Attributes.Append(configChanges);
		
		app.AppendChild(activity);
		return activity;
	}
	
	private bool RemoveActivity(XmlDocument doc, string activityName)
	{
		var app = doc.SelectSingleNode("/manifest/application");
		var fr = FindActivity(doc, activityName);
		if (fr == null)
			return false;
		
		app.RemoveChild(fr);
		return true;
	}
	
	
	
	private XmlNode FindIntent(XmlNode player, string intentAction, string intentCategory, string intentMimeType)
	{
		var intents = player.SelectNodes("intent-filter");
		for (var i = 0; i < intents.Count; ++i)
		{
			var intent = intents.Item(i);
			
			var action = intent.SelectSingleNode("action");
			if (action == null)
				continue;
			var actionName = action.Attributes.GetNamedItem("android:name");
			if (actionName == null || actionName.Value != intentAction)
				continue;
			
			var category = intent.SelectSingleNode("category");
			if (category == null)
				continue;
			var categoryName = category.Attributes.GetNamedItem("android:name");
			if (categoryName == null || categoryName.Value != intentCategory)
				continue;
			
			var datas = intent.SelectNodes("data");
			if (datas == null)
				continue;
			for (var j = 0; j < datas.Count; ++j)
			{
				var data = datas.Item(j);
				var mimeType = data.Attributes.GetNamedItem("android:mimeType");
				if (mimeType != null && mimeType.Value == intentMimeType)
					return intent;
			}
		}
		return null;
	}
	
	private XmlNode AddIntent(XmlDocument doc, XmlNode player, string intentAction, string intentCategory, string intentMimeType, string [] intentSchemes)
	{
		var intent = doc.CreateNode(XmlNodeType.Element, "intent-filter", "");
		
		var action = doc.CreateNode(XmlNodeType.Element, "action", "");
		var actionName = (XmlAttribute)doc.CreateNode(XmlNodeType.Attribute, "android", "name", AndroidXmlNamespace);
		actionName.Value = intentAction;
		action.Attributes.Append(actionName);	
		intent.AppendChild(action);
		
		var category = doc.CreateNode(XmlNodeType.Element, "category", "");
		var categoryName = (XmlAttribute)doc.CreateNode(XmlNodeType.Attribute, "android", "name", AndroidXmlNamespace);
		categoryName.Value = intentCategory;
		category.Attributes.Append(categoryName);	
		intent.AppendChild(category);
		
		var mimeType = doc.CreateNode(XmlNodeType.Element, "data", "");
		var mimeTypeName = (XmlAttribute)doc.CreateNode(XmlNodeType.Attribute, "android", "mimeType", AndroidXmlNamespace);
		mimeTypeName.Value = intentMimeType;
		mimeType.Attributes.Append(mimeTypeName);	
		intent.AppendChild(mimeType);
		
		foreach(var intentScheme in intentSchemes)
		{
			var scheme = doc.CreateNode(XmlNodeType.Element, "data", "");
			
			var schemeName = (XmlAttribute)doc.CreateNode(XmlNodeType.Attribute, "android", "scheme", AndroidXmlNamespace);
			schemeName.Value = intentScheme;
			scheme.Attributes.Append(schemeName);
			
			intent.AppendChild(scheme);
		}
		
		player.AppendChild(intent);
		return intent;
	}
	
	private bool RemoveIntent(XmlNode player, string intentAction, string intentCategory, string intentMimeType)
	{
		var fr = FindIntent(player, intentAction, intentCategory, intentMimeType);
		if (fr == null)
			return false;
		
		player.RemoveChild(fr);
		return true;
	}
	
	
	private void TraverseAndroidManifest(bool writeState)
	{
		_manifestAudioPicker = false;
		_manifestVideoPicker = false;
		_manifestImagePicker = false;
		_manifestAudioHandler = false;
		_manifestVideoHandler = false;
		_manifestImageHandler = false;
		
		var doc = new XmlDocument();
		doc.Load(FullPathTo(AndroidManifest));
		
		_manifestAudioPicker = FindActivity(doc, "com.diversido.amb.AudioPicker") != null;
		_manifestVideoPicker = FindActivity(doc, "com.diversido.amb.VideoPicker") != null;
		_manifestImagePicker = FindActivity(doc, "com.diversido.amb.ImagePicker") != null;
		
		var player = FindActivity(doc, "com.unity3d.player.UnityPlayerNativeActivity");
		if (player != null)
		{
			_manifestAudioHandler = FindIntent(player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "audio/*") != null;
			_manifestVideoHandler = FindIntent(player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "video/*") != null;
			_manifestImageHandler = FindIntent(player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "image/*") != null;
			/*
			<intent-filter>
                <action android:name="android.intent.action.VIEW" />
                <category android:name="android.intent.category.DEFAULT" />
                <data android:scheme="content"/>
                <data android:scheme="file"/>
                <data android:mimeType="audio/*"/>
            </intent-filter>
			<intent-filter>
                <action android:name="android.intent.action.VIEW" />
                <category android:name="android.intent.category.DEFAULT" />
                <data android:scheme="content"/>
                <data android:scheme="file"/>
                <data android:mimeType="video/*"/>
            </intent-filter>
			<intent-filter>
                <action android:name="android.intent.action.VIEW" />
                <category android:name="android.intent.category.DEFAULT" />
                <data android:scheme="content"/>
                <data android:scheme="file"/>
                <data android:mimeType="image/*"/>
            </intent-filter>
            */
		}
		
		if (writeState)
		{
			if (_manifestAudioPicker && !_userAudioPicker)
				RemoveActivity(doc, "com.diversido.amb.AudioPicker");
			if (!_manifestAudioPicker && _userAudioPicker)
				AddActivity(doc, "com.diversido.amb.AudioPicker", "AudioPicker");
			_manifestAudioPicker = _userAudioPicker;
			
			if (_manifestVideoPicker && !_userVideoPicker)
				RemoveActivity(doc, "com.diversido.amb.VideoPicker");
			if (!_manifestVideoPicker && _userVideoPicker)
				AddActivity(doc, "com.diversido.amb.VideoPicker", "VideoPicker");
			_manifestVideoPicker = _userVideoPicker;
			
			if (_manifestImagePicker && !_userImagePicker)
				RemoveActivity(doc, "com.diversido.amb.ImagePicker");
			if (!_manifestImagePicker && _userImagePicker)
				AddActivity(doc, "com.diversido.amb.ImagePicker", "ImagePicker");
			_manifestImagePicker = _userImagePicker;
			
			if (player != null)
			{
				if (_manifestAudioHandler && !_userAudioHandler)
					RemoveIntent(player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "audio/*");
				if (!_manifestAudioHandler && _userAudioHandler)
					AddIntent(doc, player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "audio/*", new [] { "content", "file" });
				_manifestAudioHandler = _userAudioHandler;
				
				if (_manifestVideoHandler && !_userVideoHandler)
					RemoveIntent(player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "video/*");
				if (!_manifestVideoHandler && _userVideoHandler)
					AddIntent(doc, player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "video/*", new [] { "content", "file" });
				_manifestVideoHandler = _userVideoHandler;
				
				if (_manifestImageHandler && !_userImageHandler)
					RemoveIntent(player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "image/*");
				if (!_manifestImageHandler && _userImageHandler)
					AddIntent(doc, player, "android.intent.action.VIEW", "android.intent.category.DEFAULT", "image/*", new [] { "content", "file" });
				_manifestImageHandler = _userImageHandler;
			}
			
			doc.Save(FullPathTo(AndroidManifest));
		}
	}
	
	private void CheckSetup(bool force)
	{
		if (EditorApplication.timeSinceStartup < _lastSetupCheckTime + CheckInterval && !force)
			return;
		_lastSetupCheckTime = (float)EditorApplication.timeSinceStartup;
		
		_androidManifestPresent = File.Exists(FullPathTo(AndroidManifest));
		if (!_androidManifestPresent)
		{
			_manifestAudioPicker = false;
			_manifestVideoPicker = false;
			_manifestImagePicker = false;
			_manifestAudioHandler = false;
			_manifestVideoHandler = false;
			_manifestImageHandler = false;
		}
		else
		{
			TraverseAndroidManifest(false);
		}
	}
		
	private void FixAndroidManifestPresense()
	{
		File.Copy(FullPathTo(DefaultAndroidManifest), FullPathTo(AndroidManifest), true);
	}
	
	private void ApplyAndroidManifestModification()
	{
		TraverseAndroidManifest(true);
	}
	
	void OnGUI()
	{
		CheckSetup(false);
		
		if (!_androidManifestPresent)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.HelpBox(AndroidManifest + " file is missing. Default manifest file can be generated.", MessageType.Error, true);
			GUI.skin.button.fixedWidth = 39;
			GUI.skin.button.fixedHeight = 39;
			if (GUILayout.Button("Fix"))
			{
				FixAndroidManifestPresense();
				CheckSetup(true);
			}
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			_userAudioPicker = EditorGUILayout.Toggle("Audio Picker Enabled", _manifestAudioPicker);
			_userVideoPicker = EditorGUILayout.Toggle("Video Picker Enabled", _manifestVideoPicker);
			_userImagePicker = EditorGUILayout.Toggle("Image Picker Enabled", _manifestImagePicker);

			_userAudioHandler = EditorGUILayout.Toggle("Register as Audio Viewer", _manifestAudioHandler);
			_userVideoHandler = EditorGUILayout.Toggle("Register as Video Viewer", _manifestVideoHandler);
			_userImageHandler = EditorGUILayout.Toggle("Register as Image Viewer", _manifestImageHandler);
			
			if (_userAudioPicker != _manifestAudioPicker ||
				_userVideoPicker != _manifestVideoPicker ||
				_userImagePicker != _manifestImagePicker ||
				_userAudioHandler != _manifestAudioHandler ||
				_userVideoHandler != _manifestVideoHandler ||
				_userImageHandler != _manifestImageHandler)
				ApplyAndroidManifestModification();
		}
		
		GUI.skin.button.fixedWidth = 0;
		GUI.skin.button.fixedHeight = 0;
	}
}
