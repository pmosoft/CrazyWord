using UnityEngine;
using System.Collections;

public class LevelPass : MonoBehaviour {

	public GameComponent gameComponent;


	int levelPassDisableMaxCnt = 9;

    public void LevelPassButtonStart(){ StartCoroutine("LevelPassButton"); }
    public void LevelPassButtonStop(){ StopCoroutine("LevelPassButton"); }

	public IEnumerator LevelPassButton(){

		//--------------------
		// BoxCollider
		//--------------------
		gameComponent.levelSkipBoxCollider.enabled = true;

		int levelPassDisableCnt = 9;


		for(int i=1;i<=levelPassDisableMaxCnt;i++) {
			levelPassDisableCnt--;
			gameComponent.levelSkipUILabel.text = levelPassDisableCnt.ToString();
			yield return new WaitForSeconds(1f);
		}
		
		gameComponent.levelSkipUILabel.text = "";
		gameComponent.levelSkipUISprite.spriteName = "levelskipx";
		gameComponent.levelSkipUISprite.MakePixelPerfect();
		
		gameComponent.levelSkipAnimator.SetBool("isButtonWiggle",false);

		//--------------------
		// BoxCollider
		//--------------------
		gameComponent.levelSkipBoxCollider.enabled = false;

	}

}
