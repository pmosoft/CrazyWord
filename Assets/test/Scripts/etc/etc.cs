using UnityEngine;
using System.Collections;

public class etc : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//infixOperator();
		//vectorTest01();
		//distance01();
		//bool_if();
		boolTest01();

	}

	
	void boolTest01(){
		
		bool b1 = true;
		bool b2 = false;

		print ("bool="+(b1|b2));
		print ("bool="+(b1&b2));

		
	}

	void bool_if(){

		bool b1 = true;
		print ("bool true ="+b1);

		if(b1) print ("if true ="+b1);

		// error
		//if(0) print ("if 0 ="+b1);


	}
	


	void destroy01(){

		print ("11");
		GameObject.Destroy(gameObject);
		print ("22");	
		GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube2.transform.position = new Vector3(1,1,0);

	}


	void distance01(){
		Vector2 PrevPoint = new Vector2(1,1);
		Vector2 CurPoint = new Vector2(3,2);
		Vector2 distance = CurPoint - PrevPoint;

		print (distance);
		
	}



	void etc01(){
		int i1 = 163452;
		int i2 = i1/100000;
		print (i2);

	}

	void vectorTest01(){
		Vector3 v1 = new Vector3(2,1,1);
		print (v1);
		print (v1*-1);

	}

	void infixOperator(){
		int x = 0;
		int y = 4;
		
		x = ( y < 4 ? --x : y < 3 ? ++x : x);
		
		print (x);
	}
}
