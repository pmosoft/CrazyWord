#pragma strict

function Start () {

	test03();

}

function test01 () {

    var arr = new Array ();

    // Add one element
    arr.Push ("Hello");
    
    // print the first element ("Hello")
    print(arr[0]);

    // Resize the array
    arr.length = 2;
    // Assign "World" to the second element
    arr[1] = "World";
    
    // iterate through the array
    for (var value : String in arr) {
        print(value);
    }	
}

function test02 () {

    var array = new Array (Vector3(0, 0, 0), Vector3(0, 0, 1));
    array.Push(Vector3(0, 0, 2));
    array.Push(Vector3(0, 0, 3));

}        

function test03 () {
    var v01 : Vector2 = Vector2(3, 3);
    var v02 : Vector2 = Vector2(6, 6);
    var v03 : Vector2;

    var f01 : float = Vector2.Distance(v01,v02);

    print (f01);

    var array = new Array (Vector2(1, 1), Vector2(1, 2));
    array.Push(Vector2(1, 3));
    array.Push(Vector2(1, 4));

    print (array);
    
    
    
                                               
}        

function test04 () {
    var array = new Array (Vector3(0, 0, 0), Vector3(0, 0, 1));
    array.Push(Vector3(0, 0, 2));
    array.Push(Vector3(0, 0, 3));

    print (array);

    // Copy the js array into a builtin array
    var builtinArray : Vector3[] = array.ToBuiltin(Vector3) as Vector3[];
    
    // Assign the builtin array to a js Array
    var newarr = new Array (builtinArray);
    
    // newarr contains the same elements as array
    print (newarr);
}