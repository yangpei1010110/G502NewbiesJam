using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Playground.Scenes.Scripts.Items
{
    public class Shop_UI_Item : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] Image           image;
        [SerializeField] TextMeshProUGUI Name;
        [SerializeField] TextMeshProUGUI Description;
        [SerializeField] TextMeshProUGUI Cost;
        [SerializeField] Image           BackgroundImage;
        [SerializeField] Color           Default;
        [SerializeField] Color           Selected;

        private Item      _item;
        UnityAction<Item> OnSelectedEvent;

        public void SetSelected(bool selected)
        {
            BackgroundImage.color = selected ? Selected : Default;
        }

        public void Bind(Item item, UnityAction<Item> onSelectedEvent)
        {
            _item = item;
            Name.text = _item.name;
            Description.text = _item.description;
            Cost.text = $"价格: {_item.cost}";
            image.sprite = _item.image;
            OnSelectedEvent = onSelectedEvent;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelectedEvent.Invoke(_item);
        }
    }
}