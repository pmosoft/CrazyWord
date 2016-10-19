using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class diceNumber : MonoBehaviour {

	// Use this for initialization
	void Start () {

    // int[0,,] : z
    // int[1,,] : x
    // int[,0,] : dice1
    // int[,1,] : dice2
    // int[,2,] : dice3
    // int[,3,] : dice4
    // int[,4,] : dice5
    // int[,5,] : dice6
    // int[,,0] : dice numbers
    // int[,,1] : dice numbers
    // int[,,2] : dice numbers
    // int[,,3] : dice numbers

	int[,,] zdice1 = new int[2,6,4]
	{                             
		{ // z axis (0)
		 {124536,132456,153246,145326}, //(0,1,)
		 {213645,241365,264135,236415}, //(0,2,)
		 {315624,321564,362154,356214}, //(0,3,)     
		 {412653,451263,465123,426513}, //(0,4,)     
		 {514632,531462,563142,546312}, //(0,5,)     
		 {623541,642351,654231,636521}  //(0,6,)    
		},
		{ // x axis (1)
		 {123546,135426,154236,142356}, //(1,1,)
		 {214635,246315,263145,231465}, //(1,2,)
		 {312654,326514,365124,351264}, //(1,3,)     
		 {412353,423513,435123,451233}, //(1,4,)     
		 {513642,536412,564132,541362}, //(1,5,)     
		 {624531,645321,653241,632451}  //(1,6,)    
		}
	};                            

	print (zdice1[0,0,0]);

	}

}








