using UnityEngine;
using System.Collections;

public class Navi : MonoBehaviour {

	public UIPanel fadeInOutUIPanel; 
	
	float fadeInOutAlphaOffset;
	float fadeInOutAlphaOffsetDecrease;
	float fadeInOutAlphaOffsetDelayTime = 0.01f;

    //public TweenAlpha fadeInOutTweenAlpha;    
    public UISprite fadeInOutUISprite;    

	void Awake(){
		fadeInOutUIPanel =  GameObject.Find("10_FadeInOutPanel").GetComponent<UIPanel>();
	}

	public IEnumerator FadeIn() {

        //fadeInOutTweenAlpha.from = 1f;
        //fadeInOutTweenAlpha.to = 0f;
        //fadeInOutTweenAlpha.duration = 1f;
        //fadeInOutTweenAlpha.enabled = true; 
        //yield return new WaitForSeconds( 1f );
        ////fadeInOutTweenAlpha.ResetToBeginning();
        ////fadeInOutTweenAlpha.enabled = false; 
        fadeInOutUISprite.enabled = false;


        fadeInOutAlphaOffset = 50f;
        fadeInOutAlphaOffsetDecrease = 1f / fadeInOutAlphaOffset;

        fadeInOutUIPanel.alpha = 1.0f;
        for(int i = 1; i < (int)fadeInOutAlphaOffset;i++){
            fadeInOutUIPanel.alpha -= fadeInOutAlphaOffsetDecrease;
            yield return new WaitForSeconds( fadeInOutAlphaOffsetDelayTime );
        }
        fadeInOutUIPanel.alpha = 0.0f; 
	}

	public IEnumerator FadeOut (float fadeInOutAlphaOffset) {

        //fadeInOutTweenAlpha.ResetToBeginning();
        //fadeInOutTweenAlpha.from = 0f;
        //fadeInOutTweenAlpha.to = 1f;
        //fadeInOutTweenAlpha.duration = 0.2f;
        //fadeInOutTweenAlpha.enabled = true; 
        //yield return new WaitForSeconds( 0.2f );

        //FadeInOutPanel.SetActive(true);

        fadeInOutAlphaOffsetDecrease = 1f / fadeInOutAlphaOffset;
		
        fadeInOutUIPanel.alpha = 1f / fadeInOutAlphaOffset;
        for(int i = 1; i < (int)fadeInOutAlphaOffset;i++) {
            fadeInOutUIPanel.alpha += fadeInOutAlphaOffsetDecrease;
            yield return new WaitForSeconds( fadeInOutAlphaOffsetDelayTime );
        }
    }

	public IEnumerator GoHomeScene(){
        fadeInOutUISprite.enabled = false;
		yield return StartCoroutine( FadeOut(50f) );
		Application.LoadLevel("Home");
	}

	public IEnumerator GoMapScene(){
        fadeInOutUISprite.enabled = true;
		yield return StartCoroutine( FadeOut(50f) );
		Application.LoadLevel("Map");
	}

	public IEnumerator GoGameScene(){
        fadeInOutUISprite.enabled = false;

        float fadeInOutAlphaOffset = 100f;

        if(g.curStage < 10) fadeInOutAlphaOffset = 60f;
        else if(g.curStage < 100) fadeInOutAlphaOffset = 80f;
        else if(g.curStage >= 100) fadeInOutAlphaOffset = 100f;

		yield return StartCoroutine( FadeOut(fadeInOutAlphaOffset) );
		Application.LoadLevel("Games");
		
	}

}
