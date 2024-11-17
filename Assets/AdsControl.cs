using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsControl : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener,IUnityAdsShowListener
{
    string androidGameId = "5731839";
    string iosGameId = "5731838";
    string gameId;

    private void Awake()
    {
#if UNITY_IOS
        gameId = iosGameId;
#elif UNITY_ANDROID
        gameId = androidGameId;
#endif


        if (Advertisement.isInitialized == false && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, false, this);
        }
    }
    public void OnInitializationComplete()
    {
#if UNITY_IOS
        Advertisement.Load("Rewarded_iOS", this);
#elif UNITY_ANDROID
        Advertisement.Load("Rewarded_Android", this);
#endif

    }
    public void ShowAd()
    {
#if UNITY_IOS
         Advertisement.Show("Rewarded_iOS", this);
#elif UNITY_ANDROID
        Advertisement.Show("Rewarded_Android", this);
#endif

    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("init failure " + message);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Load failure " + message);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
       
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //kasih hadiah setelah nonton iklan
        if (placementId == "Rewarded_Android" || placementId == "Rewarded_iOS")
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("ads complete"); // give reward
            }
            else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
            {
                Debug.Log("Ads Skipped"); //show warning
            }
            else
            {
                Debug.Log("ads show unknown");
            }
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("init failure " + message);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
