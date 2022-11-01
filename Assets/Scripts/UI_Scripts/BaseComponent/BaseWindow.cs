using UnityEngine;

namespace UI_Scripts.BaseComponent
{
    public abstract class BaseWindow<T> : BaseWindow where T : BaseWindow<T>
    {
        public static T Instance { get; private set; }

        public override void OnBackPressed()
        {
            Close();
        }

        protected static void Open()
        {
            if (Instance != null)
            {
                return;
            }

            BaseWindowManager.Instance.OpenWindow<T>();
        }

        protected static void Close()
        {
            if (Instance == null)
            {
                Debug.LogError("Instance is null");
                return;
            }

            BaseWindowManager.Instance.CloseWindow();
        }

        protected virtual void Awake()
        {
            Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }

    public abstract class BaseWindow : MonoBehaviour
    {
        public abstract void OnBackPressed();
    }
}