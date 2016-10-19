using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour {
	string tagname;

	void Update () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);	
		
		RaycastHit hits;
		
		if (Physics.Raycast (ray,out hits, 100)) {//생성된 객체를 이동시킴.
			
			tagname =     hits.collider.tag;
			print (tagname);
		}	
		//Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

		//Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);
		//if (Physics.Raycast(ray,out hit, 1000f)) {
			//GameObject cb = hit.collider.GetComponent<gameObject>();
	}


}
