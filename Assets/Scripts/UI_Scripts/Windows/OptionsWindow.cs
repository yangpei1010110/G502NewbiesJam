using UI_Scripts.BaseComponent;

namespace UI_Scripts.Windows
{
    public class OptionsWindow : BaseWindow<OptionsWindow>
    {
        public static void Show()
        {
            Open();
        }

        public static void Hide()
        {
            Close();
        }
    }
}