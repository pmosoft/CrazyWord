using UnityEngine;
using System.Collections;

public class aniTest01 : MonoBehaviour {

	public Animator anim;
	public Animator anim2;

	public GameObject Card2;

	void test01 () {
		print("test01="+System.DateTime.Now); 
	}
	
	void test02 () {
		print("test02="+System.DateTime.Now); 
	}

	void Start() {

		StartCoroutine( repla () );
	}

	IEnumerator repla(){

		//anim.SetTrigger("playtf");

		//anim.SetBool("playtf",true);
		anim.SetBool("isAny",true);
		//anim2.SetBool("playtf2",true);
		anim.SetBool("playtf3",true);

		yield return new WaitForSeconds(0.1f);

		//Destroy(Card2);
		//anim.SetBool("isAny",false);
//		anim.SetBool("playtf",false);
//		anim.SetBool("playtf2",true);
//
//		anim2.SetBool("playtf2",false);
//		anim2.SetBool("playtf",true);

//		anim.SetBool("playtf2",true);
//
//		anim.SetBool("playtf",false);
//		yield return new WaitForSeconds(2.0f);
//
//		//anim.SetBool("isAny",false);
//		anim.SetBool("playtf2",true);
//		yield return new WaitForSeconds(1.0f);
//		anim.SetBool("playtf2",false);
//
//		anim2.SetBool("isAny",true);

		//yield return new WaitForSeconds(1.5f);
		//anim2.SetBool("isAny",false);

		//anim.SetTrigger("playtf");
		//anim.SetBool("playtf",false);
		//anim.SetTrigger("inttf");
		//anim.SetInteger("inttf",2);
		//yield return new WaitForSeconds(1.0f);

		//anim.SetTrigger("playtf");

		//anim.SetBool("playtf",true);

		//anim.SetInteger("inttf",9);

		//anim.StartPlayback();
		yield return new WaitForSeconds(1.0f);


		//StartCoroutine( repla () );

	}


		//Animator animator1 = LetterTile.AddComponent<Animator>();
		//animator1.enabled = false;
	    //animator1.runtimeAnimatorController = Resources.Load("Animators/RainLetter") as RuntimeAnimatorController;
		//animator1.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load ("Animators/MoleLetter"));
		//animator.SetBool("isShoot",true);
		
//			Vector3 v3 = LetterTile.transform.localPosition;
//			tileTweenPos.from = new Vector3(FlowerPosX(x),flowerPosY,0f);
//			tileTweenPos.to = new Vector3(FlowerPosX(x)+250,flowerPosY-100f,0f);
//			tileTweenPos.duration = 1f;
//			//
//			AnimationCurve animationCurve = new AnimationCurve(
//				new Keyframe(0.0f,0f,0f,0f),
//				new Keyframe(0.1f,-0.1f,-0.1f,0f),
//				new Keyframe(0.2f,-0.2f,-0.2f,0f),
//				new Keyframe(0.3f,-0.3f,-0.3f,0f),
//				new Keyframe(0.4f,-0.4f,-0.4f,0f),
//				new Keyframe(0.5f,1f,1f,1f),
//				new Keyframe(1.0f,0f,0f,0f)
//				);
//			tileTweenPos.animationCurve = animationCurve;
////			//
////			//
//			tileTweenPos.enabled = true;
//			tileTweenPos.ResetToBeginning();
		

	
	
}
