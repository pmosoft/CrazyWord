using UnityEngine;
using System.Collections;

public class delayTime01 : MonoBehaviour {
	int MoonSecond = 0;
	float MoonTimeControl = 0.0f;

	// Use this for initialization
	void Start () {

		timeTest01();
		//int i01 = MoonTimer();
		//print (i01);

	}

	void timeTest01()
	{
		float ff1 = 0.0f;
		float ff2 = 0.0f;
		for(int i = 0; i < 1000; i++) {
			ff1 += Time.deltaTime;
			ff2 += Time.time;
			print(ff1+","+ff2);
		}
	}

	int MoonTimer()
	{
		
		MoonTimeControl += Time.deltaTime;
		if (MoonTimeControl >= 1)
		{
			MoonTimeControl=0;
			MoonSecond++;
		}
		return MoonSecond;
	}


}


//-----------------------------------------------------------------------------------------------
//	// Time.deltaTime 을 이용한 방법. 가장 사용하기 쉽고 많이 쓰인다.
//	// 각 함수마다 특성이 조금씩 달라서 상황에 맞게 쓰면 된다.
//	-----------------------------------------------------------------------------------------------
//		//초시계
//		int MoonSecond=0;
//
//int MoonTimer()
//{
//	
//	MoonTimeControl += Time.deltaTime;
//	if (MoonTimeControl >= 1)
//	{
//		MoonTimeControl=0;
//		MoonSecond++;
//	}
//	return MoonSecond;
//}
//-----------------------------------------------------------------------------------------------
//	//InvokeRepeating 을 이용한 방법.
//	-----------------------------------------------------------------------------------------------
//		
//		int MyTimer =0;
//
//void Start()
//{ 
//	//매초마다 함수를 불러온다.
//	//인자값("불러올 함수 이름",처음 시작시 딜레이,몇초마다 반복할건지 결정);
//	InvokeRepeating("GetMeOut",1,1);
//}
//
//
//void GetMeOut() // 매초마다 이 함수를 부를것이다.
//{
//	if(MyTimer < 5) { MyTimer++; Debug.Log(MyTimer); }
//	else { CancelInvoke("TimerInvoke"); }//실행을 중지하고 빠져나간다. 
//}
//
//-----------------------------------------------------------------------------------------------
//	//yield 를 이용한 방법
//	-----------------------------------------------------------------------------------------------
//		void Start()
//{ 
//	StartCoroutine(MyTimer());
//}
//
//IEnumerator MyTimer()
//{
//	for (int aa = 0; aa <10; aa++) //10 번 반복한다.
//	{
//		yield return new WaitForSeconds (4); //4초 간격으로
//		
//		Debug.Log ("Life after people");
//	}
//}