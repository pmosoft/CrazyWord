using UnityEngine;
using AndroidMediaBrowser;

public class Query : MonoBehaviour
{
    public Audio myAudio;
    public AudioSource audioSource;

	private string titleQuery = "";
	private string result = "";

    private static Query instance;
    public static Query Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Query>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
		GUI.skin.button.fixedHeight = (Screen.height - 20) / 10;
		GUI.skin.button.fixedWidth = (Screen.width - 20) / 3 - 3;
		GUI.skin.textField.fixedWidth = (Screen.width - 20);
		GUI.skin.textField.fixedHeight = (Screen.width - 20) / 12;
		GUI.skin.textArea.fixedHeight = (Screen.height - 20) / 3 - 20 - GUI.skin.button.fixedHeight / 3;
		
		GUILayout.Label("Title:");
		titleQuery = GUILayout.TextField(titleQuery);		
		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Query Audio"))
			{
				var audios = AudioBrowser.QueryLibrary("", "", titleQuery);
				if (audios == null)
				{
					result = "Audio query failed";
				}
				else
				{
					result = "Audio query results count: " + audios.Length;
					foreach(var audio in audios)
					{
						result += "\n   " + audio.Artist + " - " + audio.Title;

                        StartCoroutine(audio.LoadAudioClip(false, false, AudioType.WAV, (clip) => audioSource.clip = clip));
					}
				}
			}
			
			if (GUILayout.Button("Query Video"))
			{
				var videos = VideoBrowser.QueryLibrary(titleQuery);
				if (videos == null)
				{
					result = "Video query failed";
				}
				else
				{
					result = "Video query results count: " + videos.Length;
					foreach(var video in videos)
					{
						result += "\n   " + video.Title;
					}
				}
			}
			
			if (GUILayout.Button("Query Image"))
			{
				var images = ImageBrowser.QueryLibrary(titleQuery);
				if (images == null)
				{
					result = "Image query failed";
				}
				else
				{
					result = "Image query results count: " + images.Length;
					foreach(var image in images)
					{
						result += "\n   " + image.Title;
					}
				}
			}
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Space(20);
		GUILayout.Label("Result:");
		GUILayout.TextArea(result);
		
		GUILayout.Space(20);
		GUI.skin.button.fixedWidth = Screen.width - 20;
		if (GUILayout.Button("Exit"))
		{
			Application.Quit();
		}
	
		GUILayout.EndArea();
	}
}
