using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
    public Button menubutton;
    public Button backbutton;
    public VisualElement playUI;
    public VisualElement menuUI;
    // Start is called before the first frame update

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        menubutton = root.Q<Button>("MenuButton");
        backbutton = root.Q<Button>("BackButton");
        playUI = root.Q<VisualElement>("Play");
        menuUI = root.Q<VisualElement>("Menu");

        menubutton.clicked += MenuButtonPressed;
        backbutton.clicked += BackButtonPressed;
    }
    void MenuButtonPressed()//设置
    {
        Debug.Log(1);
        menuUI.style.display = DisplayStyle.None;
        playUI.style.display = DisplayStyle.Flex;
    }
    void BackButtonPressed()//返回
    {
        Debug.Log(4);
        menuUI.style.display = DisplayStyle.None;
        playUI.style.display = DisplayStyle.Flex;
    }
}
    

