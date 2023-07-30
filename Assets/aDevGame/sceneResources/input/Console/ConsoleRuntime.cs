using System;
using System.Linq;
using GameDemo.Scripts.Extensions;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace aDevGame.sceneResources.input.Console
{
    public class ConsoleRuntime : MonoBehaviour
    {
        private GameObject childContent;
        private Image      background;

        private GameObject contentGameObject;
        private GameObject inputGameObject;
        public  GameObject textPrefab;

        [SerializeField]
        public InputAction _consoleAction;

        private bool _consoleActive;

        private void Awake()
        {
            childContent = transform.GetChild(0).gameObject;
            background = GetComponent<Image>();

            contentGameObject = childContent.transform.Find("console-display")
                                            .Find("viewport")
                                            .Find("content")
                                            .gameObject;
            inputGameObject = childContent.transform.Find("console-input").gameObject;
            _consoleAction.performed += _ =>
            {
                if (_consoleActive)
                {
                    ExitConsole();
                }
                else
                {
                    EnterConsole();
                }
            };

            _consoleAction.Enable();
        }

        private void Start()
        {
            ClearConsole();
        }

        public void EnterConsole()
        {
            GamePause();
            InputManager.Instance.InputDetectionActive = false;
            background.enabled = true;
            childContent.SetActive(true);
            _consoleActive = true;

            inputGameObject.GetComponent<TMP_InputField>().ActivateInputField();
        }

        public void ExitConsole()
        {
            GameUnPause();
            childContent.SetActive(false);
            background.enabled = false;
            InputManager.Instance.InputDetectionActive = true;
            _consoleActive = false;
        }

        public void GamePause()
        {
            GameManager.Instance.Pause();
            GUIManager.Instance.SetPauseScreen(false);
        }

        public void GameUnPause()
        {
            GameManager.Instance.UnPause();
        }

        public void ConsoleInputEnd()
        {
            var textField = inputGameObject.GetComponent<TMP_InputField>();
            string tempString;

            if (!string.IsNullOrWhiteSpace(textField.text))
            {
                switch (textField.text.ToLowerInvariant())
                {
                    case "clear":
                    {
                        ClearConsole();

                        tempString = @"<color=yellow>清除控制台</color>";
                        break;
                    }
                    case "gc":
                    {
                        GC.Collect();
                        tempString = @"<color=yellow>垃圾回收测试</color>";
                        break;
                    }
                    default:
                    {
                        tempString = textField.text;
                        break;
                    }
                }

                var newMessage = Instantiate(textPrefab, contentGameObject.transform).GetComponent<TextMeshProUGUI>();
                newMessage.name = $"{newMessage.name}-{contentGameObject.transform.childCount}-{tempString}";
                newMessage.text = tempString;

                textField.text = string.Empty;
                textField.ActivateInputField();
                ResizeContent();
            }
        }

        public void ResizeContent()
        {
            var contentRect = contentGameObject.GetComponent<RectTransform>().rect;
            var allChildrenHeight = contentGameObject.transform.GetAllChildren()
                                                     .Select(t => t.GetComponent<RectTransform>().rect.height)
                                                     .Sum();
            Debug.Log($" contentRect.height = {contentRect.height} allChildrenHeight = {allChildrenHeight}");
            contentRect.height = allChildrenHeight;
            contentGameObject.GetComponent<RectTransform>().sizeDelta = contentRect.size;
        }

        public void ClearConsole()
        {
            foreach (Transform child in contentGameObject.transform.GetAllChildren())
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}