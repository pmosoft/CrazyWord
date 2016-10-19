using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using SimpleJSON;

public class FacebookManager : MonoBehaviour {

    public DBOW dbow;
    public PopupFriendControl popupFriendControl;
    public PopupShopControl popupShopControl;

    //-------------------------------------------------------------
    //                       FacebookScores
    //-------------------------------------------------------------
    // save my score
    public void FacebookPostMyScore(string myScore) 
    {
        var query = new Dictionary<string, string>();
        query["score"] = "100";
        FB.API(
               "/me/scores", 
               Facebook.HttpMethod.POST, 
               delegate(FBResult r) { Debug.Log("Result: " + r.Text); }, 
               query
               );
    }



    //-------------------------------------------------------------
    //                      FacebookFeed
    //-------------------------------------------------------------
    public void FacebookFeed()
    {
        Debug.Log("FacebookFeed");
        FB.Feed(
                linkName: "CrazyWord",
                linkCaption: "이미지연상 & 게임으로 중독된다! 단어암기게임",
                link: "http://crazyword.org/",
                callback: onFeedComplete
                ); 
    }
    public void onFeedComplete(FBResult result)
    {
        var profile = JSON.Parse(result.Text);
        Debug.Log("FeedComplete result.Text="+result.Text);

        string errorCheck = profile["error"];
        string feedResult = profile["cancelled"];

        if (errorCheck != "not logged in") {
            // Facebook Feed Cancel.
            if (feedResult == "true") {
                Debug.Log("Facebook Feed Cancel here ..");
            }
            // Facebook Feed Success.
            else if (feedResult == null) {
                Debug.Log("Facebook Feed Success Here ..");
                StartCoroutine(dbow.PurchaseTran(g.wwwDate, "fbShare"));
            }
        }
        // Facebook Feed Cancel.
        else {
            Debug.Log("Facebook Feed Cancel");
        }

        popupShopControl.EventShopFbShareClose();

    }




    //-------------------------------------------------------------
    //                       FacebookAppRequest
    //-------------------------------------------------------------
    public void FacebookAppRequest()
    {
        FB.AppRequest(
            message: "이미지연상 & 게임으로 중독된다! 단어암기게임",
            callback: AppRequestCallback
        );
    }

    void AppRequestCallback(FBResult result) {
        Debug.Log("AppRequestCallback="+result.Text);

        g.facebookEvent = "";

        StartCoroutine ( popupFriendControl.MakeFriendGrid() );
        popupFriendControl.fbLoginInfo.SetActive(false);

    }
     
    //-------------------------------------------------------------
    //                       FacebookDirectRequest
    //-------------------------------------------------------------
    public void FacebookDirectRequest()
    {
        string message = "";

        if(g.facebookEvent=="AppRequstRequestGift") {
            message = "Please, Send me Hp";
        } else if(g.facebookEvent=="AppRequstSendGift") {
            message = "I present Hp to you";
        }

        FB.AppRequest(
        message: message,
        to: g.fbFriendId.Split(','),
        filters : null,
        excludeIds : null,
        maxRecipients : null,
        data : "Hp",
        title : "CrazyWord Japanese",
        callback : AppRequestDirectCallback
        );
    }
    void AppRequestDirectCallback(FBResult result) {
        Debug.Log("AppRequestGiftCallback="+result.Text);
        var RetJson = JSON.Parse(result.Text);
        string cancelled = RetJson["cancelled"]; string request = RetJson["request"]; string to = RetJson["to"][0];
        //Debug.Log("cancelled="+cancelled); Debug.Log("request="+request); Debug.Log("to="+to);

        if(request.Length > 5 && to.Length > 5) {
            if(g.facebookEvent=="AppRequstRequestGift") {
                StartCoroutine( dbow.InsFbGiftHpHis(g.fbFriendId,g.fbId,"1") );
                g.requestGiftUISprites[g.friendListIdx].spriteName = "hprequst_o";
                g.requestGiftBoxColliders[g.friendListIdx].enabled = false;
            } else if(g.facebookEvent=="AppRequstSendGift") {
                StartCoroutine( dbow.InsFbGiftHpHis(g.fbId,g.fbFriendId,"2") );
                g.sendGiftUISprites[g.friendListIdx].spriteName = "presentsend_o";
                g.sendGiftBoxColliders[g.friendListIdx].enabled = false;
            }
            //AppRequestGiftCallback={"request":"1384364611870851","to":["744649378961239"]}
        }
        g.facebookEvent = "";
    }


}
