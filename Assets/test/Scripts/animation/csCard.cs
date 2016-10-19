using UnityEngine;
using System.Collections;

public class csCard : MonoBehaviour 
{
	void Start() 
	{
		AnimationCurve scale1 = AnimationCurve.Linear(0, 2.9f, 0.5f, 4);
		AnimationCurve scale2 = AnimationCurve.Linear(0, 0.2f, 0.5f, 0.2f);
		AnimationClip clip = new AnimationClip();
		
		clip.SetCurve("", typeof(Transform), "localScale.x", scale1);
		clip.SetCurve("", typeof(Transform), "localScale.z", scale1);
		clip.SetCurve("", typeof(Transform), "localScale.y", scale2);
		
		AnimationEvent evt = new AnimationEvent ();
		evt.time = 0.5f;
		evt.functionName = "OpenImage";
		clip.AddEvent (evt);
		
		animation.AddClip(clip, "test");
	}
	
	void OpenImage () 
	{
		gameObject.renderer.material.mainTexture = (Texture)Resources.Load ("imgFront");
	}

	void CloseImage () 
	{
		gameObject.renderer.material.mainTexture = (Texture)Resources.Load ("imgBack");
	}

	void OnMouseUp ()
	{
		animation.Play ("test");
	}
}