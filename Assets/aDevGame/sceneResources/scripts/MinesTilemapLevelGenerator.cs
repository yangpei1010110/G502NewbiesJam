#if UNITY_EDITOR
#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace aDevGame.sceneResources.scripts
{
    /// <summary>
    /// 覆盖原有的生成器
    /// </summary>
    public class MinesTilemapLevelGenerator : TilemapLevelGenerator
    {
        /// the tilemap containing the walls
        [Tooltip("the tilemap containing the walls")]
        public Tilemap ObstaclesMineTilemap; 
        public override void Generate()
        {
            TargetGrid.GetComponent<MMTilemapCleaner>().CleanAllChildren();
            //原有的生成器方法
            base.Generate();
            GeosphereGenerate();
            // 地平线生成
            HorizonGenerate();
            // debug init spawn
            InitialSpawn.position = new Vector3(0, 0, 0);
        }

        // 地下数据明细
        [HideInInspector]
        public List<MinesTilemapLevelGeneratorWindow.MineDataShell> geosphere;

        /// <summary>
        /// 地平线下生成
        /// </summary>
        public void GeosphereGenerate()
        {
            // 设置随机数种子
            var random = new System.Random(GlobalSeed);
            Random.InitState(GlobalSeed);
            int width = Random.Range(GridWidth.x, GridWidth.y);
            int height = Random.Range(GridHeight.x, GridHeight.y);

            var totalGeoWeight = geosphere.Select(shell => shell.weight).Sum();
            var orderShells = geosphere.OrderBy(shell => shell.layerOrder).ToArray();
            var currentHeight = math.max(height - horizonHeight, 0);
            var maxHeight = currentHeight;
            foreach (MinesTilemapLevelGeneratorWindow.MineDataShell currentShell in orderShells)
            {
                // 生成默认地块
                var minHeight = maxHeight - Mathf.CeilToInt(currentHeight * (currentShell.weight / (float)totalGeoWeight));
                var drawBlocks = new Vector2Int[width * (maxHeight - minHeight)];
                for (int j = 0; j < drawBlocks.Length; j++)
                {
                    drawBlocks[j] = new Vector2Int(j % width, j / width + minHeight);
                }

                MapTool.DrawTilemap(new Size(width, height), ObstaclesMineTilemap, currentShell.defaultTileBase, drawBlocks);

                // 生成矿物
                foreach (MinesTilemapLevelGeneratorWindow.MineDataSingle mineDataSingle in currentShell.mineDataSingles)
                {
                    var startPoints = Enumerable.Range(0, mineDataSingle.resourceCount)
                                                .Select(i => new Vector2Int(Random.Range(0, width), Random.Range(minHeight, maxHeight)))
                                                .ToArray();

                    var mineBlocks= MapTool.RandomWalk(new Size(width, maxHeight - minHeight), startPoints, mineDataSingle.resourceSize, random)
                                           .Select(block =>
                                            {
                                                block.y += minHeight;
                                                return block;
                                            })
                                           .ToArray();
                    
                    MapTool.DrawTilemap(new Size(width, height), ObstaclesMineTilemap, mineDataSingle.tileBase, mineBlocks);
                }

                // 更新高度
                maxHeight = math.max(minHeight, 0);
            }
        }

        [Serializable]
        public class MineGenerateData
        {
            public string name = "Layer";
            // 预制体
            public GameObject mine;
            // 生成方式
            public GenerateMethods generateMethods;
        }

        [Header("地图拓展")]
        [InspectorName("天空高度")]
        [HideInInspector]
        public int horizonHeight = 5;
        [InspectorName("地平线迭代次数")]
        [HideInInspector]
        public int maxHorizonIteration = 10;
        [InspectorName("地平线TileBase")]
        [HideInInspector]
        public TileBase horizonTileBase;

        /// <summary>
        /// 地平线生成
        /// </summary>
        public void HorizonGenerate()
        {
            // 设置随机数种子
            var random = new System.Random(GlobalSeed);
            Random.InitState(GlobalSeed);
            int width = Random.Range(GridWidth.x, GridWidth.y);
            int height = Random.Range(GridHeight.x, GridHeight.y);
            // 找到地平线高度
            int skyHeight = height - horizonHeight;

            // 生成地平线
            var startPoints = new Vector2Int[width];
            for (int i = 0; i < width; i++)
            {
                startPoints[i] = new Vector2Int(i, skyHeight);
            }

            var horizonBlocks = MapTool.RandomWalk(new Size(width, height), startPoints, maxHorizonIteration, random);
            MapTool.DrawTilemap(new Size(width, height), ObstaclesMineTilemap, horizonTileBase, horizonBlocks);
        }
    }
}
#endif