using Sirenix.OdinInspector;
using UnityEngine;

namespace Playground.Scenes.Scripts.Items.ItemCategory
{
    [CreateAssetMenu(menuName = "G502Game/物品分类", fileName = "ItemCategory_")]
    public class ItemCategory : ScriptableObject
    {
        [LabelText("名称")]
        public new string name;
    }
}