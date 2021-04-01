using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class AndroidSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
        #endif

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
