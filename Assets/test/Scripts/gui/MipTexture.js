// This script will tint texture's mip levels in different colors
// (1st level red, 2nd green, 3rd blue). You can use it to see
// which mip levels are actually used and how.

function Start () {
    // duplicate the original texture and assign to the material
    var texture : Texture2D = Instantiate(renderer.material.mainTexture);
    renderer.material.mainTexture = texture;

    // colors used to tint the first 3 mip levels
    var colors = new Color[3];
    colors[0] = Color.red;
    colors[1] = Color.green;
    colors[2] = Color.blue;
    var mipCount = Mathf.Min( 3, texture.mipmapCount );

    // tint each mip level
    for( var mip = 0; mip < mipCount; ++mip ) {
        var cols = texture.GetPixels( mip );
        for( var i = 0; i < cols.Length; ++i ) {
            cols[i] = Color.Lerp( cols[i], colors[mip], 0.33 );
        }
        texture.SetPixels( cols, mip );
    }

    // actually apply all SetPixels, don't recalculate mip levels
    texture.Apply( false );
}