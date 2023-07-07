using MoreMountains.Tools;
using UnityEditor;
using UnityEngine;

namespace aDevGame.sceneResources.scripts.editor
{
    [CustomEditor(typeof(MinesTilemapLevelGenerator), true)]
    [CanEditMultipleObjects]
    public class MinesMMTilemapGeneratorEditor : MMTilemapGeneratorEditor
    {
        public MinesTilemapLevelGenerator Generator;

        public override void OnInspectorGUI()
        {
            Generator = (MinesTilemapLevelGenerator)target;
            if (GUILayout.Button("地图生成设置"))
            {
                MinesTilemapLevelGeneratorWindow.ShowWindow(Generator);
            }

            base.OnInspectorGUI();
        }
    }
}