using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        sealed public class ConsoleCommandAttribute : Attribute
        {
            public string commandName;

            public ConsoleCommandAttribute(string commandName)
            {
                this.commandName = commandName;
            }
        }

        private GameObject childContent;
        private Image      background;

        private GameObject contentGameObject;
        private GameObject inputGameObject;
        public  GameObject textPrefab;

        [SerializeField]
        public InputAction _consoleAction;
        [SerializeField]
        public InputAction _consoleAutoCompleteAction;

        private bool _consoleActive;

        private Dictionary<string, Func<string[], string[]>> _consoleCommands = new();

        private void Awake()
        {
            // 控制台的子 UI
            childContent = transform.GetChild(0).gameObject;
            // 控制台的背景
            background = GetComponent<Image>();
            // 控制台的显示内容
            contentGameObject = childContent.transform.Find("console-display")
                                            .Find("viewport")
                                            .Find("content")
                                            .gameObject;
            // 控制台的输入框
            inputGameObject = childContent.transform.Find("console-input").gameObject;
            // 控制台的自动补全快捷键
            _consoleAutoCompleteAction.performed += _ =>
            {
                if (_consoleActive)
                {
                    TMP_InputField inputField = inputGameObject.GetComponent<TMP_InputField>();
                    if (!string.IsNullOrWhiteSpace(inputField.text))
                    {
                        string[] result = AutoCompleteCheck(inputField.text);
                        if (result != null && result.Length == 1)
                        {
                            inputField.text = result[0];
                            inputField.MoveToEndOfLine(false, false);
                        }
                        else
                        {
                            AddMessage(@" <color=#00ff00ff>你输入的可能是</color>");
                            foreach (string tempString in result ?? Array.Empty<string>())
                            {
                                AddMessage($"  <color=#00ff00ff>{tempString}</color>");
                            }
                        }
                        ResizeContent();
                    }
                }
            };
            // 控制台的快捷键
            _consoleAction.performed += _ =>
            {
                if (_consoleActive)
                {
                    _consoleAutoCompleteAction.Disable();
                    ExitConsole();
                }
                else
                {
                    _consoleAutoCompleteAction.Enable();
                    EnterConsole();
                }
            };
            _consoleAction.Enable();

            InitConsoleCommands();
        }

        /// <summary>
        /// 初始化控制台命令
        /// </summary>
        private void InitConsoleCommands()
        {
            _consoleCommands.Clear();
            // 反射 attribute 并添加命令到字典中
            MethodInfo[] methods = GetType().GetMethods();
            foreach (MethodInfo method in methods)
            {
                object[] attributes = method.GetCustomAttributes(typeof(ConsoleCommandAttribute), false);
                if (attributes.Length > 0)
                {
                    _consoleCommands.Add(
                        ((ConsoleCommandAttribute)attributes[0]).commandName,
                        str => (string[])method.Invoke(this, new object[] { str, }));
                }
            }
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

        /// <summary>
        /// 等待输入框的回车事件
        /// </summary>
        public void ConsoleInputEnd()
        {
            TMP_InputField textField = inputGameObject.GetComponent<TMP_InputField>();

            if (!string.IsNullOrWhiteSpace(textField.text))
            {
                string[] parameters = textField.text.Split(' ').ToArray();
                string commandName = parameters[0].ToLowerInvariant();
                if (_consoleCommands.TryGetValue(commandName, out Func<string[], string[]> action))
                {
                    // echo command
                    AddMessage($"<i><b><color=yellow>{string.Join(' ', parameters)}</color></b></i>");

                    string[] result = action.Invoke(parameters);
                    foreach (string tempString in result)
                    {
                        AddMessage(tempString);
                    }
                }
                else
                {
                    AddMessage(textField.text);
                }

                textField.text = string.Empty;
                textField.ActivateInputField();
                ResizeContent();
            }
        }

        /// <summary>
        /// 重新计算控制台内容的高度
        /// </summary>
        public void ResizeContent()
        {
            Rect contentRect = contentGameObject.GetComponent<RectTransform>().rect;
            float allChildrenHeight = contentGameObject.transform.GetAllChildren()
                                                       .Select(t => t.GetComponent<RectTransform>().rect.height)
                                                       .Sum();
            contentRect.height = allChildrenHeight;
            contentGameObject.GetComponent<RectTransform>().sizeDelta = contentRect.size;
        }

        public void AddMessage(string content)
        {
            TextMeshProUGUI newMessage = Instantiate(textPrefab, contentGameObject.transform).GetComponent<TextMeshProUGUI>();
            newMessage.name = $"{newMessage.name}-{contentGameObject.transform.childCount}-{content}";
            newMessage.text = content;
        }

        /// <summary>
        /// 清除控制台内容
        /// </summary>
        [ConsoleCommand("clear")]
        public string[] ClearConsole(string[] param = null)
        {
            foreach (Transform child in contentGameObject.transform.GetAllChildren())
            {
                DestroyImmediate(child.gameObject);
            }

            return new[] { @"<color=yellow>清除控制台</color>", };
        }

        /// <summary>
        /// 测试一次垃圾回收
        /// </summary>
        [ConsoleCommand("gc")]
        public string[] GcTest(string[] param = null)
        {
            GC.Collect();
            return new[] { @"<color=yellow>垃圾回收测试</color>", };
        }

        /// <summary>
        /// 打印游戏对象
        /// </summary>
        [ConsoleCommand("print")]
        public string[] PrintGameObject(string[] param = null)
        {
            if (param == null || param.Length <= 1)
            {
                return new[] { @"<color=red>参数错误</color>", };
            }
            else
            {
                GameObject go = GameObject.Find(param[1]);
                if (go)
                {
                    return new[]
                    {
                        go.name,
                        go.transform.position.ToString(),
                    };
                }
                else
                {
                    return new[] { @"<color=yellow>未找到游戏对象</color>", };
                }
            }
        }

        /// <summary>
        /// 命令模糊查询
        /// </summary>
        public string[] AutoCompleteCheck(string content)
        {
            content = content.ToLowerInvariant();
            string[] result = _consoleCommands
                             .Keys
                             .Where(command => command.Contains(content, StringComparison.InvariantCultureIgnoreCase))
                             .ToArray();

            return result;
        }
    }
}