// Draws a texture in the left corner of the screen.
// The texture is drawn in a window 60x60 pixels.
// The source texture is given an aspect ratio of 10x1
// and scaled to fit in the 60x60 rectangle. Because
// the aspect ratio is preserved, the texture will fit
// inside a 60x10 pixel area of the screen rectangle.
var aTexture : Texture;

function OnGUI() {

    if(!aTexture){
        Debug.LogError("Assign a Texture in the inspector.");
        return;
    }
    
    GUI.DrawTexture(Rect(100,100,160,160), aTexture, ScaleMode.ScaleToFit, true, 10.0f);
}