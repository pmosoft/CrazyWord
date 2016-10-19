using UnityEngine;
using System.Collections;

public class GameShootingTrigger : MonoBehaviour {

	public GameControl gameControl;
	public GameBasic gameBasic;

	public GameShooting gameShooting;
	public GameComponent gameComponent;


	string hitLetter;

	// 충돌 시작
	void OnTriggerEnter(Collider hit)
	{
		//Debug.Log("hit.gameObject.tag="+hit.gameObject.tag);
		if(	g.arrowShootCnt > 0 && hit.gameObject.tag == "Letter") {

			//Debug.Log("gameObject.tag="+gameObject.tag);
			Destroy(gameObject);
			
			//Debug.Log("trigger g.arrowDir="+g.arrowDir);
			
			gameComponent.SetArrowPrefab(hit.gameObject.transform);
			//Debug.Log("hit.gameObject.name="+hit.gameObject.name);
			
			//Debug.Log("trigger gameComponent.letterUILabel.text="+hit.gameObject.transform.FindChild("LetterLabel").GetComponent<UILabel>().text);
			//Debug.Log("trigger gameComponent.letterUILabel.text="+gameComponent.letterUILabel.text);
			//Debug.Log("trigger g.texName[g.correctLetterIdx].ToString()="+g.texName[g.correctLetterIdx].ToString());

			hitLetter = hit.gameObject.transform.FindChild("LetterLabel").GetComponent<UILabel>().text;
			if( hitLetter == g.texName[g.correctLetterIdx].ToString() ) {
				// TTS
				gameBasic.PlayTTS(g.texShowName[g.correctLetterIdx].ToString());

				gameBasic.CorrectWordColor(g.correctLetterIdx);
				g.correctLetterIdx++;

			} else if(g.correctLetterIdx > 0 && hitLetter != g.texName[g.correctLetterIdx].ToString() ) {
				g.correctLetterIdx--;
				gameBasic.CorrectWordColor(g.correctLetterIdx-1);

				// Vibrate
				if(g.isVibrate) Handheld.Vibrate();
			} else {
				// Vibrate
				if(g.isVibrate) Handheld.Vibrate();
			}
			 
			//Debug.Log("parent="+hit.gameObject.transform.parent.gameObject.name);
			hit.gameObject.transform.parent.gameObject.GetComponent<TweenPosition>().enabled = false;

			g.animator = hit.transform.GetComponentInChildren<Animator>();
				//.gameObject.transform.GetComponent<Animator>();
			g.animator.SetBool("isLetterWiggle",false);

			g.animator.SetBool("isLetterFlicker",true);

			Destroy(hit.gameObject,0.4f);
			gameShooting.SetArrowGunPreFab(g.arrowIdx,g.tileHeightCnt);

			g.bowSprite[ g.arrowIdx ].spriteName = "arrow4";
			
			for(int i=0;i<g.bowCnt;i++) g.bowBox[i].enabled = true;
			
			//------------------------
			// Checking TurnClear
			//------------------------
			//Debug.Log("g.correctLetterIdx1="+g.correctLetterIdx);
			//Debug.Log("g.texName="+g.texName);
			
			if (g.correctLetterIdx == g.texName.Length) {


				gameControl.LevelPassStart("clear");

				//StartCoroutine ( gameControl.TurnClear ());
				//gameControl.TurnClear2();
			}


		}
	}

    void voidFunc(){}


}
