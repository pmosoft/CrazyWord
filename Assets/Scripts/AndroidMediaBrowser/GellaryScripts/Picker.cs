using UnityEngine;
using AndroidMediaBrowser;

public class Picker : MonoBehaviour
{
	private string _video = "";
	private string _audio = "";
	private string _image = "";
	
	void Start()
	{
		VideoBrowser.OnPicked += (video) =>
		{
			_video = string.Format(
				"Id: {0}\nUri: {1}\nPath: {2}\nTitle: {3}",
				video.Id, video.Uri, video.Path, video.Title);
		};
		VideoBrowser.OnPickCanceled += () =>
		{
			_video = "Video pick canceled";
		};
		
		AudioBrowser.OnPicked += (audio) =>
		{
			_audio = string.Format(
				"Id: {0}\nUri: {1}\nPath: {2}\nTitle: {3}\nArtist: {4}",
				audio.Id, audio.Uri, audio.Path, audio.Title, audio.Artist);
		};
		AudioBrowser.OnPickCanceled += () =>
		{
			_audio = "Audio pick canceled";
		};
		
		ImageBrowser.OnPicked += (image) =>
		{
			_image = string.Format(
				"Id: {0}\nUri: {1}\nPath: {2}\nTitle: {3}",
				image.Id, image.Uri, image.Path, image.Title);
		};
		ImageBrowser.OnPickCanceled += () =>
		{
			_image = "Image pick canceled";
		};
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUI.skin.button.fixedHeight = (Screen.height - 20) / 12;
		GUI.skin.button.fixedWidth = (Screen.width - 20) / 4;
		GUI.skin.textArea.fixedHeight = (Screen.height - 20) / 3 - 20 - GUI.skin.button.fixedHeight / 3;
		
		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Pick Video"))
			{
				VideoBrowser.Pick();
			}
			GUILayout.TextArea(_video);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
		
		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Pick Audio"))
			{
				AudioBrowser.Pick();
			}
			GUILayout.TextArea(_audio);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
		
		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Pick Image"))
			{
				ImageBrowser.Pick();
			}
			GUILayout.TextArea(_image);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
		
		GUI.skin.button.fixedWidth = Screen.width - 20;
		if (GUILayout.Button("Exit"))
		{
			Application.Quit();
		}
	
		GUILayout.EndArea();
	}
}
