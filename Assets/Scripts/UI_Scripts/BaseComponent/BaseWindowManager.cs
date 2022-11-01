using System;
using System.Collections.Generic;
using UI_Scripts.Windows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI_Scripts.BaseComponent
{
    public class BaseWindowManager : MonoBehaviour
    {
        public MainWindow    MainWindowPrefab;
        public OptionsWindow OptionsWindowPrefab;

        private readonly Stack<BaseWindow> _windowStack = new();
        public           BaseWindow        baseWindowPrefab;

        public void OpenWindow<T>() where T : BaseWindow
        {
            if (_windowStack.TryPeek(out BaseWindow topActiveWindow))
            {
                topActiveWindow.gameObject.SetActive(false);
            }

            _windowStack.Push(Instantiate(GetPrefab<T>(), transform));
        }

        public T GetPrefab<T>() where T : BaseWindow
        {
            if (typeof(T) == typeof(MainWindow))
            {
                return MainWindowPrefab as T;
            }
            else if (typeof(T) == typeof(OptionsWindow))
            {
                return OptionsWindowPrefab as T;
            }
            else
            {
                throw new MissingReferenceException("Prefab not found");
            }
        }

        private void Update()
        {
            // if input system press escape key
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (_windowStack.TryPop(out BaseWindow topBackPressedWindow))
                {
                    topBackPressedWindow.OnBackPressed();
                }
            }
        }

        public void CloseWindow(BaseWindow baseWindow)
        {
            throw new NotImplementedException();
        }

        public void CloseWindow()
        {
            if (_windowStack.TryPop(out BaseWindow topDestroyWindow))
            {
                Destroy(topDestroyWindow.gameObject);
            }


            if (_windowStack.TryPeek(out BaseWindow topActiveWindow))
            {
                topActiveWindow.gameObject.SetActive(false);
            }
        }


        public static BaseWindowManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}