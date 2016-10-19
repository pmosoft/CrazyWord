using UnityEngine;
using System.Collections;

public class audioTest01 : MonoBehaviour {

	public AudioClip audioMatchClip;
	//public AudioSource audioMatchSource;

	// Use this for initialization
	void Start () {

		AudioSource.PlayClipAtPoint(audioMatchClip, Vector3.zero);
		//AudioSource.PlayClipAtPoint(audioMatchClip, transform.position);

		//audio.PlayOneShot(audioMatchClip);

		//audioMatchSource = gameObject.AddComponent<AudioSource>();
		//audioMatchSource.clip = audioMatchClip;
		//audioMatchSource.Play();


	}
	
}
