using UI_Scripts.BaseComponent;
using UnityEngine;

namespace UI_Scripts.Windows
{
    public class MainWindow : BaseWindow<MainWindow>
    {
        public static void Show()
        {
            Open();
        }
        public static void Hide()
        {
            Close();
        }
        public override void OnBackPressed()
        {
            Application.Quit();
        }
    }
}