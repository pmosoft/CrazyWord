using UnityEngine;
using System;
using System.Collections;

namespace AndroidMediaBrowser
{
	public class Audio : Media
	{
		public string Artist;
		public string Album;
		public string Composer;
		public int Year;
		public int Track;
	
		public long Duration;
		public long Bookmark;
	
		public bool IsAlarm;
		public bool IsMusic;
		public bool IsNotification;
		public bool IsPodcast;
		public bool IsRingtone;
		
		public AudioClip AudioClip;
	
		public override string ToString()
		{
			return string.Format
			(
				"[AndroidMediaBrowser.Audio [{0}] artist: {1}, title: {2}, path: {3}, uri: {4}",
				Id, Artist, Title, Path, Uri
			);
		}
		
		public IEnumerator LoadAudioClip(bool threeD, bool stream, AudioType audioType, Action<AudioClip> callback = null)
		{
			var url = "file://" + Path;
			var www = new WWW(url);
		    yield return www;





            if (www.error == null)
            {
                AudioClip = www.GetAudioClip(threeD, stream, audioType);

                // hack.
                Query.Instance.myAudio = this;

            }
			
			if (callback != null)
				callback(AudioClip);
		}
		
#if UNITY_ANDROID
		internal override void Init(AndroidJavaObject obj)
		{
			base.Init(obj);
			
			Artist = obj.Get<string>("artist");
			Album = obj.Get<string>("album");
			Composer = obj.Get<string>("composer");
			Year = obj.Get<int>("year");
			Track = obj.Get<int>("track");
			
			Duration = obj.Get<long>("duration");
			Bookmark = obj.Get<long>("bookmark");
			
			IsAlarm = obj.Get<bool>("isAlarm");
			IsMusic = obj.Get<bool>("isMusic");
			IsNotification = obj.Get<bool>("isNotification");
			IsPodcast = obj.Get<bool>("isPodcast");
			IsRingtone = obj.Get<bool>("isRingtone");
		}
#endif
	}
}
