using System;
using UnityEngine;

public class PrintStack : MonoBehaviour
{
    private void OnEnable()
    {
        // print stack info
        Debug.Log(Application.backgroundLoadingPriority);
        Debug.Log(Environment.StackTrace);
    }
}
