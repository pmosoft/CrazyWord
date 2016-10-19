using UnityEngine;
using System;
using System.Collections;

namespace AndroidMediaBrowser
{
	public abstract class Media
	{
		public long Id;
		public string Uri;
	    public string Path;
		public long Size;
		public string DisplayName;
	    public string Title;
	    
		
#if UNITY_ANDROID
		internal virtual void Init(AndroidJavaObject obj)
		{
			Id = obj.Get<long>("id");
			Uri = obj.Get<string>("uri");
			Path = obj.Get<string>("path");
			Size = obj.Get<long>("size");
			DisplayName = obj.Get<string>("displayName");
			Title = obj.Get<string>("title");
		}
#endif
	}
}
