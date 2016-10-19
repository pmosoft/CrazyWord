using UnityEngine;
using System.Collections;

public class IEnumeratorEX01 : MonoBehaviour {

	// Use this for initialization
	void Start () {

		
		string[] strList = { "jeong", "kang", "won", "lee", "hee" };
		
		IEnumerator e = strList.GetEnumerator();
		
		while (e.MoveNext())
		{
			print(e.Current);
		}

		e.Reset();
		e.MoveNext();
		print(e.Current);

	}
}
