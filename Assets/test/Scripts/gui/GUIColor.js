// Tints all GUI drawed elements with yellow.
var lineTex : Texture2D;
var Tex2 : Texture2D;

function OnGUI() {

    
    //GUI.color = Color.yellow;
    //GUI.color = color1;
    //GUI.Label (Rect (10, 10, 100, 20), "Hello World!");
    //GUI.Box(Rect(10, 50, 50, 50), "A BOX");
    //GUI.Button(Rect(10,110,70,30), "A button");
    GUI.DrawTexture(Rect(0,0,512,512), lineTex);

	    //var e : Event = Event.current;
    //Debug.Log(color2.ToString);


    //var letter : Texture;    
    //renderer.material.SetTexture("letter02", letter);
    //GUI.DrawTexture(Rect(0,0,128,128), letter);

    //var mouse : Vector2 = Input.mousePosition;
    //var color2 : Color;
	//color2 = lineTex.GetPixel(mouse.x,mouse.y);


    //GUI.DrawTexture(Rect(200,200,512,512), Tex2);



}


function Update() {
    //var mouse : Vector2 = Input.mousePosition;
    //Debug.Log(mouse.x+","+(mouse.y-560)*-1);

}


function Start() {
    var b : float; 
    var c : float; 

    var color1 : Color = Color(1, 1, 1, 1); // white;
    var color2 : Color = Color(0, 0, 0, 1); // black;
    var color3 : Color;
    var color4 : Color = Color(1, 0, 0, 1); // red


    var i1 : int;
    var i2 : int;
    var i3 : int;

    var array = new Array ();
    
    //var mouse : Vector2 = Input.mousePosition;
    
    
            //var e : Event = Event.current;
    /*
	for (x=0;x<128;x++) {
 		for (y=0;y<128;y++) {
			color3 = lineTex.GetPixel(x,y);
            //Debug.Log(color3.ToString);
            if (color3 == color1) i1++;
            else if (color3 == color2) {
            	i2++;
				array.Add(Vector2(x,y));            	
 			    Tex2.SetPixel (x, y, color4);

            }            	
            else i3++;
    	}
        //Debug.Log(color3.r+" "+color3.g+" "+color3.b+" "+color3.a);
    }	
    Debug.Log("i1="+i1);
    Debug.Log("i2="+i2);
    Debug.Log("i3="+i3);
    Debug.Log("array="+array.length);
  
    Debug.Log("array[1]="+array[1]);
    Debug.Log("array[2]="+array[2]);
    Debug.Log("array[3]="+array[3]);
    Debug.Log("array[4]="+array[4]);
    
    Tex2.Apply();
    */
    //var color1 : Color = Color(1, 0.3, 0.4, 0.5);
    //var color2 : Color;
    
    //b = color1.b;
    //c = color1.b;
    
//	color2 = lineTex.GetPixel(mouse.x,mouse.y);
//    Debug.Log(color2.r+" "+color2.g+" "+color2.b);
    //GUI.DrawTexture(Rect(0,0,128,128), lineTex);
		
}