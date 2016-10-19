using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GoogleIABEventListener : MonoBehaviour
{
    public PopupShopControl popupShopControl;

    private static GoogleIABEventListener _instance;
	public static GoogleIABEventListener instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GoogleIABEventListener>();
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
	
	
	#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
	}
	
	
	void OnDisable()
	{
		// Remove all event handlers
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
	}
	
	
	//////////////////// A. InApp 초기화 성공 / 실패 //////////////////////////////////////////////////////////// 
	
	/* InApp 초기화 성공 시 호출되는 콜백 함수. 
     * GoogleIAB.init( key ); 실행 후 호출되는 콜백 함수.
     * 초기화 성공 시, 소비 요청 실패 아이템이 있는 지 확인. GoogleIAB.queryInventory(skus);
     */
	#region
	void billingSupportedEvent()
	{
		Debug.Log("billingSupportedEvent");
		
		
		// 초기화 성공 시, 소비 요청 실패 아이템이 있는 지 확인. GoogleIAB.queryInventory(skus);
		// skus 값에는 Bundle Identifier, InApp Product id 1, InApp Product id 2, InApp Product id 3...
		var skus = new string[] { "com.nextpiasoft.crazyword", "crazycoin1000", };
		GoogleIAB.queryInventory(skus);
	}
	
	
	
	/* InApp 초기화 실패 시 호출되는 함수.
     * GoogleIAB.init( key ); 실행 후 호출되는 콜백 함수.
     */
	void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
	}
	
	#endregion
	
	
	
	
	//////////////////// B. 결제 성공 / 실패 //////////////////////////////////////////////////////////// 
	
	/* 결제 성공 시 호출되는 콜백 함수.
    * GoogleIAB.purchaseProduct(purchaseKey); 실행 후 호출되는 콜백 함수.
    * 반드시 소비 요청  GoogleIAB.consumeProduct(productId); 을 해야 재구매가 가능.
    */
	#region
	void purchaseSucceededEvent(GooglePurchase purchase)
	{
		
		Debug.Log("purchaseSucceededEvent: " + purchase);
		
		
		// 해당 Product ID 의 소비 요청 반드시 시작 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!.
		GoogleIAB.consumeProduct(purchase.productId);
	}
	
	
	/* 결제 실패 시 호출되는 콜백 함수.
     * GoogleIAB.purchaseProduct(purchaseKey); 실행 후 호출되는 콜백 함수.
     */
	void purchaseFailedEvent(string error)
	{
		Debug.Log("purchaseFailedEvent: " + error);
        popupShopControl.EventPurchaseWarning();
	}
	#endregion
	
	
	
	//////////////////// C. 소비 요청 성공 / 실패 //////////////////////////////////////////////////////////// 
	/*
     * 소비 요청 성공 시 호출되는 콜백 함수.
     */
	#region
	void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
   		Debug.Log("purchase.orderId=" + purchase.orderId);
        Debug.Log("purchase.productId=" + purchase.productId);

        g.isPurchaseSucceed = true;
        g.purchaseOrderId = purchase.orderId;
        g.purchaseProductId = purchase.productId;

	
    }
	
	
	
	/*
     * 소비 요청 실패 시 호출되는 콜백 함수.
     */
	void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("consumePurchaseFailedEvent: " + error);
	}
	#endregion
	
	
	//////////////////// D. 소비 요청 확인 성공 / 실패 //////////////////////////////////////////////////////////// 
	
	/* 구글 결제 정상 처리 후, 소비 요청이 실패 된 것이 있는 지 확인 성공 시 콜백 함수.
     * GoogleIAB.queryInventory( skus ); 실행 후 호출되는 콜백 함수.
     * purchases.Count 값이 0 이면 문제가 없음. (해당 product id 재구매 가능).
     * purchases.Count 값이 0보다 크면 갯수만큼 소비가 안되어 재구매가 안됨.
     * GoogleIAB.consumeProduct(productId); 함수를 사용해서 소비시켜야 함.
     */
	#region
	void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		Debug.Log(string.Format("DDDDDDDDD sss queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
		Prime31.Utils.logObject(purchases);
		Prime31.Utils.logObject(skus);
		
		// 소비 요청이 안된 아이템이 있는지 확인해서, 소비 요청이 안된 아이템 들에 대한 재요청.
		if (purchases.Count > 0)
		{
			for (int i = 0; i < purchases.Count; i++)
			{
				Debug.Log("DDDDDDD consumeProduct Start,  Product ID  : " + purchases[i].productId);
				GoogleIAB.consumeProduct(purchases[i].productId);
			}
		}
	}
	
	
	
	
	
	/* 구글 결제 정상 처리 후, 소비 요청이 실패 된 것이 있는 지 확인 실패 시 콜백 함수.
     * GoogleIAB.queryInventory( skus ); 실행 후 호출되는 콜백 함수.
     */
	void queryInventoryFailedEvent(string error)
	{
		Debug.Log("DDDDDDDDD eee queryInventoryFailedEvent: " + error);
	}
	#endregion
	
	
	
	
	//////////////////// E. 결제/소비 요청 성공 시 구글 영수증 코드 //////////////////////////////////////////////////////////// 
	/*  영수증 코드(JSON).
    */
	void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}
	
	
	
	
	#endif
}

