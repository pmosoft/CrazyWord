var hSliderValue : float = 0.0;

function OnGUI () {    
	hSliderValue = GUI.HorizontalSlider (Rect (25, 25, 100, 30), hSliderValue, 0.0, 10.0);
}