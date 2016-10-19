using UnityEngine;
using System.Collections;

public class renderer01 : MonoBehaviour {
	
	public GameObject NNN;
	
	// Use this for initialization
	void Start () {
		
		
		//renderer.material.color = Color.red;
		Texture dice_texture = Resources.LoadAssetAtPath("Assets/saiPang/Resources/Textures/Dice/Dice_Bomul_d.png",typeof(Texture)) as Texture;

		Renderer[] renderers = NNN.GetComponentsInChildren<Renderer>();
		
		foreach (Renderer r in renderers)
		{
			//r.enabled = false;
			r.material.mainTexture = dice_texture;
		}
	

	}

}
