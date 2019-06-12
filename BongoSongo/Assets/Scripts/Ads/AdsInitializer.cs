using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsInitializer : MonoBehaviour {
    public void Start() {
#if UNITY_ANDROID
        string appId = "ca-app-pub-8099181658913762~8623879374";
#else
        string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
    }
}
