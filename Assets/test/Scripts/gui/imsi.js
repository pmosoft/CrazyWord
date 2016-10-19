    var hSliderValue : float = 0.0;    
    var style : GUIStyle;    
    function OnGUI () {        
    GUI.skin.horizontalSliderThumb = style;        
    hSliderValue = GUILayout.HorizontalSlider (hSliderValue, 0.0, 10.0);    
    }