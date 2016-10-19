// Shows the button rect properties in a label when the mouse is over it
var buttonText : GUIContent = new GUIContent("some button"); 
var buttonStyle : GUIStyle = GUIStyle.none; 

function OnGUI() { 
    var rt : Rect = GUILayoutUtility.GetRect(buttonText, buttonStyle); 
    if (rt.Contains(Event.current.mousePosition)) { 
        GUI.Label(Rect(0,20,200,70), "PosX: " + rt.x + "\nPosY: " + rt.y + 
              "\nWidth: " + rt.width + "\nHeight: " + rt.height);
    } 
    GUI.Button(rt, buttonText, buttonStyle); 
}