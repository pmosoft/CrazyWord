using UnityEngine;
using System.Collections;

public class ngui_tween : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//TestTweenscale();
		TestTweenPosition();

	}

	void TestTweenscale(){
		float duration = 2f; // 애니메이션의 길이입니다.(시간)
		Vector3 scaleTo = Vector3.one * 3; // 오브젝트의 최종 Scale 입니다.
		
		TweenScale tweenscale = TweenScale.Begin(gameObject, duration, scaleTo);
		//tweenscale.method = UITweener.Method.BounceIn;
		
		AnimationCurve animationCurve = new AnimationCurve(
			new Keyframe(0f, 0f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.7f, 1.2f, 1f, 1f), // 애니메이션 시작후 70% 지점에서 1.2의 사이즈까지 커졌다가
			new Keyframe(1f, 1f, 1f, 0f)); // 100%로 애니메이션이 끝날때는 1.0의 사이즈가 됩니다.
		
		tweenscale.animationCurve = animationCurve;
	}

	void TestTweenPosition(){
		float duration = 2f; // 애니메이션의 길이입니다.(시간)
		Vector3 scaleTo = Vector3.one * 3; // 오브젝트의 최종 Scale 입니다.

		TweenPosition tweenposition = TweenPosition.Begin(gameObject, duration, scaleTo);
		//tweenscale.method = UITweener.Method.BounceIn;
		
		AnimationCurve animationCurve = new AnimationCurve(
			new Keyframe(0.0f, 0.0f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.1f, 0.1f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.2f, 0.2f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.3f, 0.3f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.4f, 0.4f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.5f, 0.5f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.6f, 0.6f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.7f, 0.7f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.8f, 0.8f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(0.9f, 0.9f, 0f, 1f), // 0%일때 0의 값에서 시작해서
			new Keyframe(1.0f, 1.0f, 0f, 1f), // 0%일때 0의 값에서 시작해서

			new Keyframe(0.7f, 1.2f, 1f, 1f), // 애니메이션 시작후 70% 지점에서 1.2의 사이즈까지 커졌다가
			new Keyframe(1f, 1f, 1f, 0f)); // 100%로 애니메이션이 끝날때는 1.0의 사이즈가 됩니다.
		
		tweenposition.animationCurve = animationCurve;


	}


}
