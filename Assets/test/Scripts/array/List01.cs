using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class List01 : MonoBehaviour {

	List<int> iList01;
	List<int> iList02;

	// Use this for initialization
	void Start () {
		test4 ();
	}

	void test4(){
		List<string>[] stageTex01 = new List<string>[4];

		stageTex01[0] =  new List<string>();
		stageTex01[1] =  new List<string>();
		stageTex01[2] =  new List<string>();
		stageTex01[3] =  new List<string>();

		stageTex01[0].Add("v01_001_001_hobby_1");
		stageTex01[0].Add("v01_001_001_hobby_2");
		stageTex01[0].Add("v01_001_001_hobby_3");
		stageTex01[0].Add("v01_001_001_hobby_4");

		stageTex01[1].Add("v01_001_002_idea_1");
		stageTex01[1].Add("v01_001_002_idea_2");
		stageTex01[1].Add("v01_001_002_idea_3");
		
		stageTex01[2].Add("v01_001_003_dancing_1");
		stageTex01[2].Add("v01_001_003_dancing_2");

		stageTex01[3].Add("v01_001_004_hang_1");


		print (stageTex01[0][0]);

		print (stageTex01[0].Count);

		for (int i = 0; i < 4; i++) {
			int randomIndex = Random.Range(i, 4);
			List<string> temp = stageTex01[i];
			stageTex01[i] = stageTex01[randomIndex];
			stageTex01[randomIndex] = temp;
		}

		for (int i = 0; i < 4; i++) {
			print (stageTex01[i][0]);
			print (stageTex01[i].Count);
		}




	}

	void  test3(){
		
		List<int>[] stageTex02 = new List<int>[2];

		stageTex02[0] =  new List<int>();

		stageTex02[0].Add(1);

		print (stageTex02[0][0]);

	}

	void  test2(){

		List<int> iList01;
		List<int> iList02;

		iList01 = new List<int>();
		
		iList01.Add(1);
		iList01.Add(2);
		iList01.Add(3);
		
		print (iList01[0]);			

		iList02 = new List<int>();

		//iList02.CopyTo((int[])iList01);

		iList01.RemoveAt(1);
		iList01.RemoveAt(2);
		iList01.RemoveAt(3);		

		foreach (int i in iList02) {
			print (i);			
		}


	}


	void test1(){

		iList01 = new List<int>();
		
		iList01.Add(1);
		iList01.Add(2);
		iList01.Add(3);
		
		print (iList01[0]);			
		
		iList01.RemoveAt(iList01.Count-1);
		
		foreach (int i in iList01) {
			print (i);			
		}

	}

}
