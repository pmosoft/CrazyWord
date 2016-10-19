using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class HeroTable : MonoBehaviour {
	UISprite[] heros;

	Transform mTrans;
	bool mIsDragging = false;
	Vector3 mPosition, mLocalPosition;
	Vector3 mDragStartPosition;
	Vector3 mDragPosition;
	Vector3 mStartPosition;
	
	public float cellHeight = 320f;
	public float downScale = 0.4f;
	public int cellTotal = 50;
	public int seq = 3;
	
	public UILabel titleLabel;
	
	// Use this for initialization
	void Start () {
		StartCoroutine( DelayStart(1f) );
	}
	void Awake(){
		mTrans = transform;
		mPosition = mTrans.position;
		mLocalPosition = mTrans.localPosition;
	}
	
	IEnumerator DelayStart(float delayTime) {
		yield return new WaitForSeconds(delayTime);
		heros = gameObject.GetComponentsInChildren<UISprite>();
		SetPosition(false);
	}
	
	void SetSequence(bool isRight){
		//print ("SetSequence");
		//print ("mLocalPosition="+mLocalPosition);
		//print ("mTrans.localPosition="+mTrans.localPosition);

		Vector3 dist = mTrans.localPosition - mLocalPosition; 
		//print ("dist="+dist);

		float distY = Mathf.Round(dist.y/cellHeight);
		//print ("distY="+distY);

		seq = (int)distY;
		if (seq >= cellTotal) seq = cellTotal - 1;
		if (seq <= 0) seq = 0;
	}
	
	void SetPosition(bool isMotion){
		//print ("SetPosition");
		//print ("isMotion="+isMotion);

		Vector3 pos = mLocalPosition;
		//print ("pos1="+pos);

		//print ("seq="+seq);


		pos += new Vector3(0f, seq * cellHeight, 0f);
		//print ("pos2="+pos);

		//pos -= new Vector3(0f, 300f, 0f);
		if (isMotion) {
			//print ("if isMotion");

			TweenParms parms = new TweenParms();
			parms.Prop("localPosition", pos);
			parms.Ease(EaseType.EaseOutCirc);
			HOTween.To(mTrans, 0.1f, parms);
			HOTween.Play();
		} else {
			//print ("else isMotion");

			mTrans.localPosition = pos;
		}
		//titleLabel.text = heros[seq].spriteName;
	}

	void Drop () {
		//print ("Drop");
		Vector3 dist = mDragPosition - mDragStartPosition;
		//print ("dist="+dist);

		if (dist.y>0f) SetSequence(true);
		else SetSequence(false);
		SetPosition(true);
	}

	void OnDrag (Vector2 delta) {
		Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastTouchPosition);
		float dist = 0f;
		Vector3 currentPos = ray.GetPoint(dist);

		if (UICamera.currentTouchID == -1 || UICamera.currentTouchID == 0) {
			if (!mIsDragging) {
				mIsDragging = true;
				mDragPosition = currentPos;
			} else {
				Vector3 pos = mStartPosition - (mDragStartPosition - currentPos);
				Vector3 cpos = new Vector3(mTrans.position.x, pos.y, mTrans.position.z);
				mTrans.position = cpos;
			}
		}
	}

	void OnPress (bool isPressed) {
		//print ("OnPress");
		//print ("isPressed="+isPressed);

		mIsDragging = false;
		Collider col = collider;
		if (col != null) {
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastTouchPosition);
			float dist = 0f;
			mDragStartPosition = ray.GetPoint(dist);
			mStartPosition = mTrans.position;

			//print ("mDragStartPosition="+mDragStartPosition);
			//print ("mStartPosition="+mStartPosition);

			col.enabled = !isPressed;
		}
		if (!isPressed) Drop();
	}
}
