using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace aDevGame.sceneResources.scripts
{
    /// <summary>
    /// 测试地图生成管理窗口
    /// </summary>
    public class MinesTilemapLevelGeneratorWindow : OdinEditorWindow
    {
        private static MinesTilemapLevelGenerator _generator;

        public static void ShowWindow(MinesTilemapLevelGenerator generator)
        {
            _generator = generator;
            var window = GetWindow<MinesTilemapLevelGeneratorWindow>();
            window.Load();
            window.Show();
        }

        public void Load()
        {
            width = _generator.GridWidth.x;
            height = _generator.GridHeight.x;
            globalSeed = _generator.GlobalSeed;
            randomGlobalSeed = _generator.RandomizeGlobalSeed;
            horizonHeight = _generator.horizonHeight;
            maxHorizonIteration = _generator.maxHorizonIteration;
            horizonTileBase = _generator.horizonTileBase;
            geosphere = _generator.geosphere;
        }

        [LabelText("保存设置")]
        [Button(ButtonSizes.Large)]
        [PropertyOrder(-1)]
        public void Save()
        {
            _generator.GridWidth = new Vector2Int(width, width);
            _generator.GridHeight = new Vector2Int(height, height);
            _generator.GlobalSeed = globalSeed;
            _generator.RandomizeGlobalSeed = randomGlobalSeed;
            _generator.horizonHeight = horizonHeight;
            _generator.maxHorizonIteration = maxHorizonIteration;
            _generator.horizonTileBase = horizonTileBase;
            _generator.geosphere = geosphere;
        }

        [LabelText("生成地图")]
        [Button(ButtonSizes.Large)]
        [PropertyOrder(-1)]
        public void Generate()
        {
            Save();
            _generator.Generate();
            Load();
        }

        [FoldoutGroup("地图生成设置"), LabelText("宽度"),]
        [PropertyRange(1, 200)]
        public int width = 100;

        [FoldoutGroup("地图生成设置"), LabelText("高度"),]
        [PropertyRange(1, 200)]
        public int height = 50;

        [FoldoutGroup("地图生成设置"), LabelText("全局地图种子"),]
        public int globalSeed = 50;

        [FoldoutGroup("地图生成设置"), HorizontalGroup("地图生成设置/随机生成全局地图种子"), LabelText("随机生成全局地图种子"),]
        public bool randomGlobalSeed = true;

        [FoldoutGroup("地图生成设置"), HorizontalGroup("地图生成设置/随机生成全局地图种子"), LabelText("手动生成种子"),]
        [Button]
        public void GenerateGlobalSeed() => globalSeed = Random.Range(0, int.MaxValue);

        [FoldoutGroup("地图生成设置"), LabelText("地平线距离天花板高度"),]
        [PropertyRange(1, nameof(height))]
        public int horizonHeight = 30;

        [FoldoutGroup("地图生成设置"), LabelText("地平线随机迭代次数"),]
        [PropertyRange(1, 30)]
        public int maxHorizonIteration = 5;

        [FoldoutGroup("地图生成设置"), LabelText("地平线TileBase"),]
        public TileBase horizonTileBase;

        [Serializable]
        public struct MineDataSingle
        {
            [LabelText("矿物TileBase")]
            public TileBase tileBase;
            [LabelText("矿物数量")]
            public int resourceCount;
            [LabelText("矿物大小")]
            public int resourceSize;
        }

        [Serializable]
        public struct MineDataShell
        {
            [LabelText("层级排序")]
            public int layerOrder;
            [LabelText("层级名称")]
            public string shellName;
            [LabelText("层级占比权重")]
            public int weight;
            [LabelText("默认地块")]
            public TileBase defaultTileBase;
            [LabelText("地块数据")]
            public List<MineDataSingle> mineDataSingles;
        }

        [FoldoutGroup("地图生成设置"), LabelText("地下分层地块"),]
        public List<MineDataShell> geosphere;
    }
}