using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class shooting : MonoBehaviour {

	public GameObject arrowSprite;

	void Start() {

//		HOTween.To(transform, 2f, new TweenParms()
//		           .Prop("localPosition", new Vector3(200,0,0) )
//		           .Ease(EaseType.Linear)
//		           );

	}

	// 충돌 시작
	void OnTriggerEnter(Collider hit)
	{
		// GO 삭제 - GO 하위의 컴포넌트를 삭제할 때도 Destroy() 메서드를 사용할 수 있습니다.
		
		print("hit.gameObject.tag="+hit.gameObject.tag);
		//Destroy(this.gameObject);
	}

	
	
}