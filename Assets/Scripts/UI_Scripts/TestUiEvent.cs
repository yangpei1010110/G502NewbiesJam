using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace UI_Scripts
{
    public class TestUiEvent : MonoBehaviour
    {
        [LabelText("是否打印资源路径")] public bool isPrintPath = true;
        private void Awake()
        {
            if (isPrintPath)
            {
                // 脚本资源路径测试 Application.dataPath
                Debug.Log($"Application.dataPath:{Application.dataPath}");

                // 脚本资源路径测试 Application.streamingAssetsPath
                Debug.Log($"Application.streamingAssetsPath:{Application.streamingAssetsPath}");

                // 脚本资源路径测试 Application.persistentDataPath
                Debug.Log($"Application.persistentDataPath:{Application.persistentDataPath}");

                // 脚本资源路径测试 Application.temporaryCachePath
                Debug.Log($"Application.temporaryCachePath:{Application.temporaryCachePath}");

                // 脚本资源路径测试 Application.absoluteURL
                Debug.Log($"Application.absoluteURL:{Application.absoluteURL}");
            }
        }
    }
}