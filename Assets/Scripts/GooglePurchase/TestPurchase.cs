using UnityEngine;
using System.Collections;

public class TestPurchase : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PurchaseManager.Instance.StartPurchase("coin100");
        }
	
	}
}
