using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using SimpleJSON;

public class EventCommon : MonoBehaviour {

	public Navi navi;
	public bool isCurPrevDist = false;

	bool touchPlatform = false;	


	void Awake () { 
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			touchPlatform = true;
		}
	}

	//-------------------------------
	// Common
	//-------------------------------

	public void EventOnPress(){
		//print ("EV EventOnPress");
		
		if (touchPlatform) {
			g.PrevPoint = Input.GetTouch(0).position;
		} else {
			g.PrevPoint = Input.mousePosition;
		}

	}
	
	public void EventOnRelease(){
		//print ("EV EventOnRelease");
		
		if (touchPlatform) {
			g.CurPoint = Input.GetTouch(0).position;
		} else {
			g.CurPoint = Input.mousePosition;
		}
		
		float curPrevDist = Mathf.Abs(g.CurPoint.x - g.PrevPoint.x) + Mathf.Abs(g.CurPoint.y - g.PrevPoint.y);
		
		if (curPrevDist < 20) isCurPrevDist = true; else isCurPrevDist = false; 
		
	}

	//-------------------------------
	// Navigation
	//-------------------------------

    //public void EventHomeSceneOnRelease(){	
    //    //print ("EV EventHomeOnRelease");
    //    EventOnRelease();
    //    if (isCurPrevDist) StartCoroutine(navi.GoHomeScene());
    //}

    //public void EventMapSceneOnRelease(){	
    //    //print ("EV EventMapOnRelease");
    //    EventOnRelease();
    //    if (isCurPrevDist) StartCoroutine(navi.GoMapScene());
    //}

    //public void EventGameSceneOnRelease(){	
    //    //print ("EV EventGameOnRelease");
    //    EventOnRelease();
    //    if (isCurPrevDist) StartCoroutine(navi.GoGameScene());
    //}



    //-------------------------------------------
    //           Quit Warning Popup
    //-------------------------------------------
	//public AudioClip[] audioClip = new AudioClip[1];
    //public AudioClip startClip;
    //public AudioSource buttonAudioSource;

    //public GameObject PopupBg8;
    //public GameObject QuitWarningPopup;
    //public Animator quitWarningPopupAnimator;
    //public void EventQuitWarning() {

    //    g.isQuit = false;
    //    print("EventQuitWarning");
    //    EventOnPress();

    //    //buttonAudioSource.clip = startClip; buttonAudioSource.Play();
    //    QuitWarningPopup.SetActive(true);
    //    PopupBg8.SetActive(true);
    //    quitWarningPopupAnimator.speed = 2;
    //    quitWarningPopupAnimator.SetBool("isSmallLargeScale2", true);
    //}
    //public void EventQuitWarningOK() {
    //    print("EventQuitWarningOK");
    //    EventOnRelease();
    //    if (isCurPrevDist) Application.Quit();
    //}
    //public void EventQuitWarningClose() {
    //    print("EventQuitWarningClose");
    //    EventOnRelease();
    //    if (isCurPrevDist) {
    //        //buttonAudioSource.clip = startClip; buttonAudioSource.Play();
    
    //        QuitWarningPopup.SetActive(false);
    //        PopupBg8.SetActive(false);
    //        quitWarningPopupAnimator.SetBool("isSmallLargeScale2", false);
    //    }
    //}


}
