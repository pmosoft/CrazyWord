function Start () {
    // Create a new texture and assign it to the renderer's material
    var texture = new Texture2D(128, 128);
    renderer.material.mainTexture = texture;
    // Fill the texture with Sierpinski's fractal pattern!
    for (var y : int = 0; y < texture.height; ++y) {
        for (var x : int = 0; x < texture.width; ++x) {
            var color = (x&y) ? Color.white : Color.gray;
            texture.SetPixel (x, y, color);
        }
    }
    // Apply all SetPixel calls
    texture.Apply();
}