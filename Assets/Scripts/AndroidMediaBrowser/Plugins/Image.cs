using UnityEngine;
using System;
using System.Collections;

namespace AndroidMediaBrowser
{
	public class Image : Media
	{
		public string BucketDisplayName;
		public string Description;
		public string Orientation;
		public string PicasaId;
		
		public bool IsPrivate;
		public long DateTaken;
		public double Latitude;
		public double Longitude;
		
		public Texture2D Texture;
	    
		public override string ToString()
		{
			return string.Format
			(
				"[AndroidMediaBrowser.Image [{0}] title: {1}, path: {2}, uri: {3}",
				Id, Title, Path, Uri
			);
		}
		
		public IEnumerator LoadTexture(Action<Texture> callback = null)
		{
			var url = "file://" + Path;
			var www = new WWW(url);
		    Debug.Log ("[Amb] Loading texture from url: " + url);
			yield return www;
			
		    if (www.error == null)
			{
				Texture = www.texture;
			}
			else
			{
				Debug.Log ("[Amb] Texture load error: " + www.error);
			}
			
			if (callback != null)
				callback(Texture);
		}
		
#if UNITY_ANDROID
		internal override void Init(AndroidJavaObject obj)
		{
			base.Init(obj);
			
			BucketDisplayName = obj.Get<string>("bucketDisplayName");
			Description = obj.Get<string>("description");
			Orientation = obj.Get<string>("orientation");
			PicasaId = obj.Get<string>("picasaId");
			
			IsPrivate = obj.Get<bool>("isPrivate");
			DateTaken = obj.Get<long>("dateTaken");
			Latitude = obj.Get<double>("latitude");
			Longitude = obj.Get<double>("longitude");
		}
#endif
	}
}
