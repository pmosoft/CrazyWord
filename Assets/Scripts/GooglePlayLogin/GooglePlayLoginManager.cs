using UnityEngine;
using System.Collections;

public class GooglePlayLoginManager : MonoBehaviour {

    public HomeControl homeControl;

	// Use this for initialization
	void Start () {

        GooglePlayLogin();
	
	}

    void GooglePlayLogin()
    {
        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.Activate();

        // Google Play Login.
        Social.localUser.Authenticate((bool success) =>
        {
            // Google play Login Success.
            if (success)
            {
                g.googleId = Social.localUser.id;

                Debug.Log("SSSSSSS. Google Play Login Success .. ID : " + g.googleId);
                homeControl.HomeStart();
            }

            // Google play Login Failed.
            else if (!success)
            {
                Debug.Log("FFFFFFF. Error Google Play Login ");
            }

        });
    }
}
