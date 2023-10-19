using Playground.Scenes.Scripts.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Playground.Scenes.Scripts.Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _textDialog;
        [SerializeField]
        private Image _itemImage;

        public void OnItemSelected(Item item)
        {
            _textDialog.text = item.description;
            _itemImage.sprite = item.image;
        }
    }
}