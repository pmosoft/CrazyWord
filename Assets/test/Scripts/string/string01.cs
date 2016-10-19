using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class string01 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//test01();
		test08();

	}

	void test08(){
		
		string str1 = "abcd.png";

        print( str1.Contains(".1png") );
	}	


//	void test03(){
//		
//		string letterExcluded = "abcd";
//
//		for(int i=65;i<128;i++) {
//			char c1 = System.Convert.ToChar(i);		
//			print (i+"="+c1);
//		}
//	}	

	void test07(){
		
		int i1 = 10%3;

		print(((1-1)/3)+1);
		print(((2-1)/3)+1);
		print(((3-1)/3)+1);
		print(((4-1)/3)+1);
		print(((5-1)/3)+1);
		print(((6-1)/3)+1);
		print(((7-1)/3)+1);
		print(((8-1)/3)+1);
		print(((9-1)/3)+1);
		print(((10-1)/3)+1);
		print(((11-1)/3)+1);
		print(((12-1)/3)+1);
		print(((13-1)/3)+1);
		print(((14-1)/3)+1);

//		if     (g.texShowName.Length <= 3){ blankCnt=1;  }
//		else if(g.texShowName.Length <= 6){ blankCnt=2;  }
//		else if(g.texShowName.Length <= 9){ blankCnt=3;  }
//		else if(g.texShowName.Length <=12){ blankCnt=4;  }
//		else if(g.texShowName.Length <=15){ blankCnt=5;  }
//		else if(g.texShowName.Length <=18){ blankCnt=6;  }

	}

	void test06(){

		string context = "abcd";
		//string context1 = context.Substring(0,context.IndexOf(" "));
		//string context2 = context.Substring(context.IndexOf(" "),context.Length-context.IndexOf(" ") );

		print(context.IndexOf(" "));
		//print("context1="+context1);
		//print("context2="+context2);
	}


	void test05(){
		bool b1 = true;
		bool b2 = false;
		if(b1&&b2) print ("1 true"); else print ("1 false");
		if(b1||b2) print ("2 true"); else print ("2 false");
	}

	void test04(){

		int bonusTextCnt = Random.Range(1,3);
		string bonusWordText;
		
		int alphabat1 = Random.Range(65,89);
		int alphabat2 = Random.Range(65,89);
		int alphabat3 = Random.Range(65,89);

		print ("bonusTextCnt="+bonusTextCnt);
		print ("alphabat1="+alphabat1);
		print ("alphabat2="+alphabat2);
		print ("alphabat3="+alphabat3);

		bonusWordText = System.Convert.ToChar(alphabat1).ToString();	
		print ("bonusWordText="+bonusWordText);
		if(bonusTextCnt==2) {
			bonusWordText += System.Convert.ToChar(alphabat2).ToString();	
		} else if (bonusTextCnt==3) {
			bonusWordText += System.Convert.ToChar(alphabat2).ToString();	
			bonusWordText += System.Convert.ToChar(alphabat3).ToString();	
		}
		print ("bonusWordText="+bonusWordText);
	}	

	void test01(){
		string s1 = "abcd";
		
		print( s1.Substring(0,2) );
		print( s1.Substring(0,2) );
		print( s1.Substring(1,1) );
		print( s1.Substring(1,2) );
		print( s1.Substring(0,0) );
		print( s1.LastIndexOf("c") );
	}	

	void test02(){

		string letterExcluded = "abcd";
		int letterIndex = letterExcluded.IndexOf('c');
		print(letterIndex);
		string letterExcluded2 = letterExcluded.Remove(letterIndex,1);
		letterExcluded = letterExcluded2;
		print ("letterExcluded="+letterExcluded);
		print ("letterExcluded2="+letterExcluded2);

	}	

}
