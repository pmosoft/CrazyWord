private var stringToEdit : String = "가";
var gskin : GUISkin;

function OnGUI () {
    GUI.skin = gskin;
    gskin.textField.fixedHeight = 500;
    gskin.textField.fixedWidth = 500;
    gskin.textField.fontSize = 500;
    
    // Make a text field that modifies stringToEdit.
    stringToEdit = GUI.TextField (Rect (0, 0, 500, 500), stringToEdit, 1);
} 