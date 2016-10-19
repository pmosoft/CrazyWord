using UnityEngine;
using System.Collections;

public class PurchaseManager : MonoBehaviour 
{

	
	// PurchaseManager 싱글톤 패턴.
	#region
	private static PurchaseManager _instance;
	public static PurchaseManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<PurchaseManager>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	
	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if (this != _instance)
				Destroy(this.gameObject);
		}
	}
	#endregion
	
	
	
	// 시작 시 InApp 모듈 초기화.
	// 콜백 함수는 GoogleIABEventListener -> billingSupportedEvent or billingNotSupportedEvent.
	void Start()
	{
        #if UNITY_ANDROID && !UNITY_EDITOR
    		// Google Play Developer Consol -> 서비스 및 API -> 라이선스 및 인앱 결제 -> 어플리케이션 용 라이센스 키 등록.
    		var key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArEIczNJSAWR9ELIiRDcy3K50RdLhNte3x6O9IW7+Y0Z3HcNhPQBvN6eKNlkj7zA9eypLUXDmJx+0GYQf92EXEW3YNtEWqr3S7Umot2+ZhYw4ZZWaqHfizZaCAOfGT9waCZLaE1W+E7Z7tKJCpolEE3kZXf8TFyJUHh3RGT4oteUBl/wll84fipzVj4DFVMhp7MdEMtPRbWH9+Mp6tldXiWZ7iobioO9N+u31NfdNpLCsWlr3ZC//LSfff2IpPJn03VB64s9g1PL93KVemxoCXWjklRnfEWci5tg09lRsKBNAYZVmIGqCbD2M+REO9jDT0MvxuoBLXd5Leu1wS10YrQIDAQAB";
    		GoogleIAB.init(key);

        # endif
	}
	
	
	/* 인앱 결제 프로세스 시작.
     * 사용법 : GoogleIAB.purchaseProduct(product ID);
     */
	public void StartPurchase(string purchaseItem)
	{
        #if UNITY_ANDROID && !UNITY_EDITOR
		    if     (purchaseItem == "coin100") GoogleIAB.purchaseProduct("coin100");
		    else if(purchaseItem == "coin200") GoogleIAB.purchaseProduct("coin200");
		    else if(purchaseItem == "coin300") GoogleIAB.purchaseProduct("coin300");
            else if(purchaseItem == "allpack") GoogleIAB.purchaseProduct("allpack");
        # endif
	}
}
