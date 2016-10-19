using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class int01 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		test03 ();

	}

	void test01() {

		//private Tile[,] diceTiles = new Tile[Data.tileWidth, Data.tileHeight];
		
		//diceTiles[x,y].diceMain = diceMain;
		
		//print( ia(0,0) );
		
		int[,,] array =  
		{
			{{1,2},{3,4},{5,6}},
			{{1,4},{2,5},{3,6}},
			{{6,5},{4,3},{2,1}},
			{{6,3},{5,2},{4,1}},
			{{6,3},{5,2},{4,1}},
			{{6,3},{5,2},{4,1}},
			{{6,3},{5,2},{4,1}},
			{{6,3},{5,2},{4,1}},
		};
		
		print ( array[0,1,1] );
		print ( array[0,2,1] );
		

	}

	
	void test02() {
		
		//private Tile[,] diceTiles = new Tile[Data.tileWidth, Data.tileHeight];
		
		//diceTiles[x,y].diceMain = diceMain;
		
	}


	void test03() {
		int[,] ia = new int[2,2];
		
		ia[0,0] = 1;
		
		int[] ib = new int[2];
		
		ib[0] = 1;
		
		print( ib[0] );	

		
	}


}
