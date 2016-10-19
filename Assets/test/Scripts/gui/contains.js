function Update () {
    var rect = Rect (0, 0, 150, 150);

    Debug.Log("Input.mousePosition.x="+Input.mousePosition.x);
    Debug.Log("Input.mousePosition.y="+Input.mousePosition.y);
    if (rect.Contains(Input.mousePosition))
        print("Inside");
       
}