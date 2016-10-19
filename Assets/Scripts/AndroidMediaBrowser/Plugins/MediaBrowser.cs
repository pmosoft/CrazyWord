using UnityEngine;
using System;
using System.Collections;

namespace AndroidMediaBrowser
{
#if UNITY_ANDROID
	internal static class MediaBrowser
	{
		private static AndroidJavaObject _androidAdapterJavaObject;
		private static GameObject _androidAdapterGameObject;
		private static AmbAdapter _androidAdapterComponent;
		
		internal static void InitAndroidAdapterIfNeeded()
		{
			if (_androidAdapterJavaObject != null ||
				_androidAdapterGameObject != null ||
				_androidAdapterComponent != null)
			{
				return;
			}
			
			_androidAdapterGameObject = new GameObject("AmbAdapter");
			GameObject.DontDestroyOnLoad(_androidAdapterGameObject);
			_androidAdapterComponent = _androidAdapterGameObject.AddComponent<AmbAdapter>();
			
			_androidAdapterJavaObject = new AndroidJavaObject("com.diversido.amb.Adapter", _androidAdapterGameObject.name);
		}
		
		internal static AndroidJavaObject AndroidAdapter
		{
			get
			{
				InitAndroidAdapterIfNeeded();
				return _androidAdapterJavaObject;
			}
		}
		
		internal static AmbAdapter Callbacks
		{
			get 
			{
				InitAndroidAdapterIfNeeded();
				return _androidAdapterComponent;
			}
		}
	}
	
	public class MediaBrowser<T> where T : Media, new()
	{
		public static event Action<T> OnPicked;
		protected static void InvokeOnPicked(T t)
		{
			var c = OnPicked;
			if (c != null)
				c(t);
		}
		
		public static event Action OnPickCanceled;
		protected static void InvokeOnPickCanceled()
		{
			var c = OnPickCanceled;
			if (c != null)
				c();
		}
		
		private static bool _callbackInited = false;
		protected static void InitCallbackIfNeeded(Action action)
		{
			if (!_callbackInited)
			{
				action();
				_callbackInited = true;
			}
		}
		
		internal static bool IsStatusOk(string status)
		{
			return status == "0";
		}
		
		private static string _getMethod = string.Empty;
		private static T CreateMedia(AndroidJavaObject obj)
		{
			if (obj == null)
				return null;
			
			var t = new T();
			t.Init(obj);
			return t;
		}
		
		internal static void Pick(Action initCallback, string method, string getMethod)
		{
			InitCallbackIfNeeded(initCallback);
			_getMethod = getMethod;
			
			MediaBrowser.AndroidAdapter.Call(method);
		}
		
		protected static void _OnPicked(string status)
		{
			if (!IsStatusOk(status))
			{
				InvokeOnPickCanceled();
				return;
			}
			
			var result = MediaBrowser.AndroidAdapter.Call<AndroidJavaObject>(_getMethod);
			var media = CreateMedia(result);
			if (media == null)
			{
				InvokeOnPickCanceled();
				return;
			}
			
			InvokeOnPicked(media);
		}
		
		protected static T [] GetArray(Action initCallback, string method, params object [] args)
		{
			InitCallbackIfNeeded(initCallback);
			
			T [] array = null;
			using (var results = MediaBrowser.AndroidAdapter.Call<AndroidJavaObject>(method, args))
			{
				var resultsCount = results.Call<int>("getCount");
				array = new T[resultsCount];
				for (var i = 0; i < array.Length; ++i)
				{
					AndroidJNI.AttachCurrentThread();
			        AndroidJNI.PushLocalFrame(0);
			        try
					{
						using (AndroidJavaObject result = results.Call<AndroidJavaObject>("get", i))
						{
							array[i] = CreateMedia(result);
						}
					}
					catch (Exception ex)
	        		{
						Debug.Log("Received exception getting item " + i + " trunkating result\n" + ex.ToString());
						var a = new T[i];
						for (var j = 0; j < i; ++j)
						{
							a[j] = array[j];
						}
						array = a;
	        		}
	        		finally
	        		{
	            		AndroidJNI.PopLocalFrame(IntPtr.Zero);
	        		}
				}
			}
			return array;
		}
		
		internal static T Get(Action initCallback, string method, params object [] args)
		{
			InitCallbackIfNeeded(initCallback);
			
			var result = MediaBrowser.AndroidAdapter.Call<AndroidJavaObject>(method, args);
			return CreateMedia(result);
		}
	}

	public class AudioBrowser : MediaBrowser<Audio>
	{
		private static void Init()
		{
			MediaBrowser.Callbacks.OnAudioPicked += _OnPicked;
		}
		
		public static void Pick()
		{
			Pick(Init, "pickAudio", "getPickedAudio");
		}
		
		public static Audio [] QueryLibrary(string artist, string album, string title)
		{
			return GetArray(Init, "queryAudioLibrary", artist, album, title);
		}
		
		public static Audio GetStartObject()
		{
			return Get(Init, "getStartIntentAudio");
		}
	}
	
	public class VideoBrowser : MediaBrowser<Video>
	{
		private static void Init()
		{
			MediaBrowser.Callbacks.OnVideoPicked += _OnPicked;
		}
		
		public static void Pick()
		{
			Pick(Init, "pickVideo", "getPickedVideo");
		}
		
		public static Video [] QueryLibrary(string title)
		{
			return GetArray(Init, "queryVideoLibrary", title);
		}
		
		public static Video GetStartObject()
		{
			return Get(Init, "getStartIntentVideo");
		}
	}
	
	public class ImageBrowser : MediaBrowser<Image>
	{
		private static void Init()
		{
			MediaBrowser.Callbacks.OnImagePicked += _OnPicked;
		}
		
		public static void Pick()
		{
			Pick(Init, "pickImage", "getPickedImage");
		}
		
		public static Image [] QueryLibrary(string title)
		{
			return GetArray(Init, "queryImageLibrary", title);
		}
		
		public static Image GetStartObject()
		{
			return Get(Init, "getStartIntentImage");
		}
	}
#else
	public class AudioBrowser
	{
		public static event Action<Audio> OnPicked;
		public static event Action OnPickCanceled;
		
		public static void Pick()
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
		}
		
		public static Audio [] QueryLibrary(string album, string artist, string title)
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
			return null;
		}
		
		public static Audio GetStartObject()
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
			return null;
		}
	}
	
	public class VideoBrowser
	{
		public static event Action<Video> OnPicked;
		public static event Action OnPickCanceled;
		
		public static void Pick()
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
		}
		
		public static Video [] QueryLibrary(string title)
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
			return null;
		}
		
		public static Video GetStartObject()
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
			return null;
		}
	}
	
	public class ImageBrowser
	{
		public static event Action<Image> OnPicked;
		public static event Action OnPickCanceled;
		
		public static void Pick()
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
		}
		
		public static Image [] QueryLibrary(string title)
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
			return null;
		}
		
		public static Image GetStartObject()
		{
			Debug.LogWarning("Android Media Browser is supported only for Android platform");
			return null;
		}
	}
#endif
}
