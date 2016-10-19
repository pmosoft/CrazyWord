using UnityEngine;
using System.Collections;

public class MapScale : MonoBehaviour {
	Transform mTrans,mParent;
	Vector3 scale;
	float cellHeight = 320f;
	float downScale;

	BoxCollider topBoxCollider;
	BoxCollider centerBoxCollider;
	BoxCollider bottomBoxCollider;

	UILabel lessonNum;
	UISprite rankUISprite;
	UISprite iconUISprite;

	void Start () {
		mTrans  = transform;
		scale   = mTrans.localScale;
		mParent = mTrans.parent.parent;

		topBoxCollider    = mTrans.transform.FindChild("1_TopBoard").GetComponent<BoxCollider>();
		centerBoxCollider = mTrans.transform.FindChild("2_CenterBoard").GetComponent<BoxCollider>();
		bottomBoxCollider = mTrans.transform.FindChild("1_BottomBoard").GetComponent<BoxCollider>();

		lessonNum = mTrans.transform.FindChild("2_CenterBoard").FindChild("3_LessonNum").GetComponent<UILabel>();
		rankUISprite = mTrans.transform.FindChild("2_CenterBoard").FindChild("3_Rank").GetComponent<UISprite>();
		iconUISprite = mTrans.transform.FindChild("2_CenterBoard").FindChild("3_Icon").GetComponent<UISprite>();

	}

    int lessonNumber = 0;
	void Update () {
		//Debug.Log("mTrans.localPosition="+mTrans.localPosition);
		//Debug.Log("mParent.localPosition="+mParent.localPosition);

		Vector3 pos = mTrans.localPosition + mParent.localPosition;

//		Debug.Log("mTrans.localPosition="+mTrans.localPosition);
//		Debug.Log("mParent.localPosition="+mParent.localPosition);

		//Debug.Log("pos="+pos);
		float dist = Mathf.Clamp(Mathf.Abs(pos.y), 0f, cellHeight);
		//Debug.Log("dist="+dist);

        if (lessonNum.text == "A"){ lessonNumber = -3; } else if (lessonNum.text == "B"){ lessonNumber = -2; } else if (lessonNum.text == "C"){ lessonNumber = -1; } else if (lessonNum.text == "D"){ lessonNumber =  0; }
        else lessonNumber = int.Parse(lessonNum.text);

		if(dist < 10) {
			topBoxCollider.enabled = g.isMap;
			centerBoxCollider.enabled = g.isMap;
			bottomBoxCollider.enabled = g.isMap;

			g.curStage = lessonNumber;
			g.curRankName = rankUISprite.spriteName;
			g.curIconName = iconUISprite.spriteName;


		} else if(dist == 280) {
			topBoxCollider.enabled = false;
			centerBoxCollider.enabled = false;
			bottomBoxCollider.enabled = false;
		} else {
			topBoxCollider.enabled = false;
			centerBoxCollider.enabled = false;
			bottomBoxCollider.enabled = false;
		}

		mTrans.localScale = ((cellHeight - dist*0.4f) / cellHeight) * scale;


	}
}
