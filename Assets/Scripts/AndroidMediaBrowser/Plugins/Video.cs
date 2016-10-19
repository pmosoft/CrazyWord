using UnityEngine;
using System;
using System.Collections;

namespace AndroidMediaBrowser
{
	public class Video : Media
	{
		public string Album;
		public string Artist;
		public string BucketDisplayName;
		public string Description;
		public string Category;
		public string Tags;
		public string Language;
		public string Resolution;
	
		public long Duration;
		public long Bookmark;
	
		public bool IsPrivate;
		public long DateTaken;
		public double Latitude;
		public double Longitude;
		
		public override string ToString()
		{
			return string.Format
			(
				"[AndroidMediaBrowser.Video [{0}] title: {1}, path: {2}, uri: {3}",
				Id, Title, Path, Uri
			);
		}
		
#if UNITY_ANDROID
		internal override void Init(AndroidJavaObject obj)
		{
			base.Init(obj);
			
			Album = obj.Get<string>("album");
			Artist = obj.Get<string>("artist");
			BucketDisplayName = obj.Get<string>("bucketDisplayName");
			Description = obj.Get<string>("description");
			Category = obj.Get<string>("category");
			Tags = obj.Get<string>("tags");
			Language = obj.Get<string>("language");
			Resolution = obj.Get<string>("resolution");
			
			Duration = obj.Get<long>("duration");
			Bookmark = obj.Get<long>("bookmark");
			
			IsPrivate = obj.Get<bool>("isPrivate");
			DateTaken = obj.Get<long>("dateTaken");
			Latitude = obj.Get<double>("latitude");
			Longitude = obj.Get<double>("longitude");
		}
#endif
	}
}
