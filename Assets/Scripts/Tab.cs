using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public CanvasScaler canvasScaler;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            CheckFoldableStatus();
        }
    }
    void CheckFoldableStatus()
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject resources = currentActivity.Call<AndroidJavaObject>("getResources");
        AndroidJavaObject configuration = resources.Call<AndroidJavaObject>("getConfiguration");

        int screenLayout = configuration.Get<int>("screenLayout");

        bool isFoldable = (screenLayout & 0x08) != 0; // This checks for large screen layout (not specifically foldable)

        if (isFoldable || CheckForFlipDevice()) // Combine with additional flip detection logic
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
#else
        // Fallback logic for other platforms
        canvasScaler.matchWidthOrHeight = 1;
#endif
    }

    private bool CheckForFlipDevice()
    {
        // Optionally, add logic to detect specific flip devices by checking screen dimensions, aspect ratio, or model names.
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        float aspectRatio = screenSize.x / screenSize.y;

        // Example condition: Checking for specific aspect ratios or resolution changes
        if (aspectRatio < 1.0f && screenSize.y > 2000) // Hypothetical condition for flip detection
        {
            return true;
        }

        return false;
    }

}


