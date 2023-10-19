using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Playground.Scenes.Scripts.Items.ItemCategory
{
    public class Shop_UI_Category : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] TextMeshProUGUI Name;
        [SerializeField] Image           Image;
        [SerializeField] Color           Default;
        [SerializeField] Color           Selected;

        ItemCategory              _category;
        UnityAction<ItemCategory> OnSelectedEvent;

        public void Bind(ItemCategory category, UnityAction<ItemCategory> onSelectedEvent)
        {
            Name.text = (_category = category).name;
            OnSelectedEvent = onSelectedEvent;

            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            Image.color = selected ? Selected : Default;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelectedEvent.Invoke(_category);
        }
    }
}