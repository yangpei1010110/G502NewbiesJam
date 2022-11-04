using System;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace UI_Scripts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TestUiEvent : MonoBehaviour
    {
        [SerializeField, LabelText("当前对象"),] public TextMeshProUGUI display;

        [SerializeField, Range(0.1f, 2f), LabelText("FPS采样时间"),]
        public float sampleDuration = 0.25f;

        [LabelText("失去焦点降帧率")] public bool isFocusChangeFrameRate = true;

        private long  frames;
        private float duration, best = float.MaxValue, worst = float.MinValue;

        private void OnApplicationFocus(bool hasFocus)
        {
            if (isFocusChangeFrameRate)
            {
                if (hasFocus)
                {
                    Application.targetFrameRate = -1;
                }
                else
                {
                    Application.targetFrameRate = 1;
                }
            }
        }

        private void Start()
        {
            display = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            float frameDuration = Time.unscaledDeltaTime;
            frames   += 1;
            duration += frameDuration;
            best     =  math.min(best, frameDuration);
            worst    =  math.max(worst, frameDuration);

            if (duration >= sampleDuration)
            {
                display.SetText(
                    $"FPS\n"                     +
                    $"{1f     / best:0.0}\n"     +
                    $"{frames / duration:0.0}\n" +
                    $"{1f     / worst:0.0}");
                frames   = 0;
                duration = 0;
                best     = float.MaxValue;
                worst    = float.MinValue;
            }
        }


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