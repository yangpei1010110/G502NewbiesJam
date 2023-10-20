using System;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Playground.Scenes
{
    public class OffgridMapGeneration : MonoBehaviour
    {
        [SerializeField] [LabelText("宽和高")]
        Vector2Int Size;

        [SerializeField] [LabelText("地图块预制体")]
        GameObject MapBlockPrefab;
        
        [SerializeField] [LabelText("地图块根节点")]
        Transform MapBlockRoot;

        private void Start()
        {
            MapBlockRoot.MMDestroyAllChildren();
        }
    }
}