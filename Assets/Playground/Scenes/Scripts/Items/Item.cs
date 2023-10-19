using Sirenix.OdinInspector;
using UnityEngine;

namespace Playground.Scenes.Scripts.Items
{
    [CreateAssetMenu(menuName = "G502Game/物品", fileName = "Item_")]
    public class Item : ScriptableObject
    {
        [LabelText("名称")]
        public new string name;
        [LabelText("价值")]
        public int cost;
        [LabelText("图片")]
        public Sprite image;
        [LabelText("分类")]
        public ItemCategory.ItemCategory Category;

        [TextArea(3, 5)]
        [LabelText("描述")]
        public string description;
    }
}