using UnityEngine;
using System.Collections;

public class anisound01 : MonoBehaviour {

	public AudioClip blip;

	// Use this for initialization
	void blipSounder () {
		AudioSource.PlayClipAtPoint(blip, transform.position);	
	}

}
