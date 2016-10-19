using UnityEngine;
using System;

public class AmbAdapter : MonoBehaviour
{
	public event Action<string> OnAudioPicked;
	private void onAudioPicked(string status)
	{
		OnAudioPicked(status);
	}
	
	public event Action<string> OnVideoPicked;
	private void onVideoPicked(string status)
	{
		OnVideoPicked(status);
	}

	public event Action<string> OnImagePicked;
	private void onImagePicked(string status)
	{
		OnImagePicked(status);
	}
}
