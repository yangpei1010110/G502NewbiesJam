using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using Playground.Scenes.Scripts.Items;
using Playground.Scenes.Scripts.Items.ItemCategory;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Playground.Scenes.Scripts.Shop
{
    public class Shop_UI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI Title;
        [SerializeField] TextMeshProUGUI Cost;
        [SerializeField] Transform       ItemCategoryRoot;
        [SerializeField] Transform       ItemRoot;

        [SerializeField] GameObject Shop_UI_Category_Prefab;
        [SerializeField] GameObject Shop_UI_Item_Prefab;

        [SerializeField] List<Item> Items;

        private Dictionary<ItemCategory, Shop_UI_Category> _itemCategoryMap = new();
        private Dictionary<Item, Shop_UI_Item>             _itemMap         = new();

        private ItemCategory _selectedItemCategory;
        private Item         _selectedItem;

        private void Start()
        {
            RebuildUI();
        }

        void OnItemSelected(Item item)
        {
            _selectedItem = item;
            foreach (KeyValuePair<Item,Shop_UI_Item> kv in _itemMap)
            {
                kv.Value.SetSelected(kv.Key.Equals(_selectedItem));
            }
        }

        void OnItemCategorySelected(ItemCategory category)
        {
            _selectedItemCategory = category;
            ItemRoot.MMDestroyAllChildren();
            var itemUIArray = Items.Where(i => i.Category == _selectedItemCategory).ToArrayPooled();
            _itemMap.Clear();
            foreach (Item item in itemUIArray)
            {
                var go = Instantiate(Shop_UI_Item_Prefab, ItemRoot);
                var ui = go.GetComponent<Shop_UI_Item>();
                ui.Bind(item, OnItemSelected);
                _itemMap.TryAdd(item, ui);
            }

            foreach (KeyValuePair<ItemCategory, Shop_UI_Category> kv in _itemCategoryMap)
            {
                kv.Value.SetSelected(kv.Key.Equals(_selectedItemCategory));
            }
        }

        void RebuildUI()
        {
            ItemCategoryRoot.MMDestroyAllChildren();
            ItemRoot.MMDestroyAllChildren();
            _itemCategoryMap.Clear();
            _itemMap.Clear();

            var categoryUIArray = Items.Select(item => item.Category).Distinct().OrderBy(category => category.name).ToArrayPooled();
            foreach (ItemCategory category in categoryUIArray)
            {
                var go = Instantiate(Shop_UI_Category_Prefab, ItemCategoryRoot);
                var ui = go.GetComponent<Shop_UI_Category>();
                ui.Bind(category, OnItemCategorySelected);
                _itemCategoryMap.TryAdd(category, ui);
            }
        }
    }
}