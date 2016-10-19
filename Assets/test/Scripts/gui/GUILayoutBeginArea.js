function OnGUI () {
    // Starts an area to draw elements
    GUILayout.BeginArea (Rect (100,100,100,100));
    GUILayout.Button ("Click me");
    GUILayout.Button ("Or me");
    GUILayout.Button ("Or me2");
    GUILayout.EndArea ();
}