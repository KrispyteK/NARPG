using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class DisplayInterstitialAd : MonoBehaviour
{
    private InterstitialAd interstitial;

    void Start () {
        //AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
        //AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
        //AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
        //string android_id = secure.CallStatic<string>("getString", contentResolver, "android_id");

        //print("Android id: " + android_id);

        print(SystemInfo.deviceUniqueIdentifier);

        RequestInterstitial();
    }

    public void ShowAd () {
        this.interstitial.Show();
    }

    public void RequestInterstitial() {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-8099181658913762/3701234494";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice("fa5a74be9c07b043dd89da89f58b2bb6").Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }
}
