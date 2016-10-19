using UnityEngine;
using System.Collections;

public class dot : MonoBehaviour {

	public float ContAngle(Vector3 fwd, Vector3 targetDir)
	{
		float angle = Vector3.Angle(fwd, targetDir);
		
		if (AngleDir(fwd, targetDir, Vector3.up) == -1)
		{
			angle = 360.0f - angle;
			if( angle > 359.9999f )
				angle -= 360.0f;
			return angle;
		}
		else
			return angle;
	}
	
	public int AngleDir( Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);
		
		if (dir > 0.0)
			return 1;
		else if (dir < 0.0)
			return -1;
		else
			return 0;
	}


	// Use this for initialization
	void Start () {

		//Vector2 v1 = new Vector2(0,0);
		//Vector2 v2 = new Vector2(1,1);
		//print (Vector2.Angle(v1,v2));

//		float dot1 = Vector2.Dot(v1, v2);
//		print ("dot1="+dot1);

//		Vector3 v1 = new Vector3(0,0,0);
//		Vector3 v2 = new Vector3(2,1,0);
//
//		print(ContAngle(v1,v2));
		//		float dot1 = Vector2.Dot(v1, v2);
		//		print ("dot1="+dot1);


		Vector2 vec1= new Vector2(0,0); 
		Vector2 vec2= new Vector2(1,1);
		//Get the dot product
		float dot = Vector3.Dot(vec1,vec2);
		// Divide the dot by the product of the magnitudes of the vectors
		dot = dot/(vec1.magnitude*vec2.magnitude);
		//Get the arc cosin of the angle, you now have your angle in radians 
		var acos = Mathf.Acos(dot);
		//Multiply by 180/Mathf.PI to convert to degrees
		var angle = acos*180/Mathf.PI;
		//Congrats, you made it really hard on yourself.
		print(angle);


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
