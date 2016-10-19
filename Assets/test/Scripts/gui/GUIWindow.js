var windowRect : Rect = Rect (20, 20, 120, 50);

function OnGUI () {    
// Register the window.    
	windowRect = GUI.Window (0, windowRect, DoMyWindow, "My Window");
}
// Make the contents of the window

function DoMyWindow (windowID : int) {    
// Make a very long rect that is 20 pixels tall.     
// This will make the window be resizable by the top    
// title bar - no matter how wide it gets.    
	GUI.DragWindow (Rect (0,0, 10000, 20));
}