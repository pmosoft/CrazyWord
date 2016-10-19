using UnityEngine;
using AndroidMediaBrowser;

public class TextureLoader : MonoBehaviour
{
    public GameObject go;

	void Start()
	{
		ImageBrowser.OnPicked += (image) =>
		{
			StartCoroutine(LoadTexture(image));
		};
		ImageBrowser.Pick();
	}
	
	private Texture2D _texture;


	private System.Collections.IEnumerator LoadTexture(Image image)
	{
		yield return StartCoroutine(image.LoadTexture());
		_texture = image.Texture;

        go.renderer.material.mainTexture = (Texture)_texture;

	}
}
