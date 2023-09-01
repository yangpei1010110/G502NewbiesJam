using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public Button backButton;
    public VisualElement mainMenu;
    public VisualElement optionsMenu;
    
    private void Awake()
    {
        // 获取 rootVisualElement，这个根视觉元素是 UI 层级的起始点。
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 给变量赋值。
        playButton = root.Q<Button>("PlayButton");
        optionsButton = root.Q<Button>("OptionsButton");
        quitButton = root.Q<Button>("QuitButton");
        backButton = root.Q<Button>("BackButton");
        mainMenu = root.Q<VisualElement>("MainMenu");
        optionsMenu = root.Q<VisualElement>("OptionsMenu");
        // 将以下方法指定给这些按钮：
        playButton.clicked += PlayButtonPressed;
        optionsButton.clicked += OptionsButtonPressed;
        quitButton.clicked += QuitButtonPressed;
        backButton.clicked += BackButtonPressed;
    }
    void PlayButtonPressed()//开始
    {
        SceneManager.LoadScene("Main");
    }
    void OptionsButtonPressed()//设置
    {
        mainMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.Flex;
    }
    void QuitButtonPressed()//退出
    {
        Debug.Log("Quit!");
        //Application.Quit();
    }
    void BackButtonPressed()//返回
    {
        optionsMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.Flex;
    }
}
