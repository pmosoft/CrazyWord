using UnityEngine;
using System.Collections;

public class HeroItem : MonoBehaviour {
	Transform mTrans, mParent;
	Vector3 scale;
	float cellHeight;
	float downScale;
	HeroTable hTable;

	void Start () {
		mTrans = transform;
		scale = mTrans.localScale;
		mParent = mTrans.parent;
		hTable = mParent.GetComponent<HeroTable>();
		cellHeight = hTable.cellHeight;
		downScale = hTable.downScale;
	}
	
	void Update () {
		//print ("mTrans.localPosition="+mTrans.localPosition);
		//print ("mParent.localPosition="+mParent.localPosition);
		Vector3 pos = mTrans.localPosition + mParent.localPosition;
		//print ("pos="+pos);
		float dist = Mathf.Clamp(Mathf.Abs(pos.y), 0f, cellHeight);
		//print ("dist="+dist);
		
		mTrans.localScale = ((cellHeight - dist*downScale) / cellHeight) * scale;
		
		//print ("dist*downScale="+dist*downScale);
		//print ("((cellWidth - dist*downScale) / cellWidth)+"<==((cellWidth - dist*downScale) / cellWidth)");
		
	}
}
