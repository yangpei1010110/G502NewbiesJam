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
        public override void Generate()
        {
            TargetGrid.GetComponent<MMTilemapCleaner>().CleanAllChildren();
            //原有的生成器方法
            base.Generate();
            GeosphereGenerate();
            // 地平线生成
            HorizonGenerate();
            // 新物资生成
            // MinesGenerate();
            
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

                MapTool.DrawTilemap(new Size(width, height), ObstaclesTilemap, currentShell.defaultTileBase, drawBlocks);

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
                    
                    MapTool.DrawTilemap(new Size(width, height), ObstaclesTilemap, mineDataSingle.tileBase, mineBlocks);
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

        // [Header("额外生成对象")]
        // [InspectorName("可生成对象")]
        // public List<MineGenerateData> MineObjects;
        // private GameObject _mineSceneGameObject;
        // private string     _mineSceneGameObjectName = "Mines";
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
            MapTool.DrawTilemap(new Size(width, height), ObstaclesTilemap, horizonTileBase, horizonBlocks);
        }


        // public void MinesGenerate()
        // {
        //     // 找到矿物的场景父类或生成
        //     _mineSceneGameObject ??= GameObject.Find(_mineSceneGameObjectName);
        //     if (!_mineSceneGameObject)
        //     {
        //         _mineSceneGameObject = new GameObject(_mineSceneGameObjectName);
        //         _mineSceneGameObject.name = _mineSceneGameObjectName;
        //     }
        //
        //     // 清理原有全部子类
        //     foreach (Transform child in _mineSceneGameObject.transform.GetAllChildren())
        //     {
        //         DestroyImmediate(child.gameObject);
        //     }
        //
        //     // 设置随机数种子
        //     var random = new System.Random(GlobalSeed);
        //     UnityEngine.Random.InitState(GlobalSeed);
        //     int width = UnityEngine.Random.Range(GridWidth.x, GridWidth.y);
        //     int height = UnityEngine.Random.Range(GridHeight.x, GridHeight.y);
        //     // 找到障碍物的网格
        //     int[,] obstacleGrid = MMGridGenerator.TilemapToGrid(ObstaclesTilemap, width, height);
        //     // 添加出生点和终点也为障碍物
        //     var spawnGrid = TargetGrid.WorldToCell(InitialSpawn.position) - MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
        //     var exitGrid = TargetGrid.WorldToCell(Exit.position) - MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
        //     obstacleGrid[spawnGrid.x, spawnGrid.y] = 1;
        //     obstacleGrid[exitGrid.x, exitGrid.y] = 1;
        //
        //     // 生成矿物
        //     foreach (MineGenerateData mineGenerateData in MineObjects)
        //     {
        //         int[,] grid;
        //         switch (mineGenerateData.generateMethods)
        //         {
        //             case GenerateMethods.Perlin:
        //                 grid = MMGridGeneratorPerlinNoise.Generate(width, height, Random.value);
        //                 break;
        //             case GenerateMethods.PerlinGround:
        //                 grid = MMGridGeneratorPerlinNoiseGround.Generate(width, height, Random.value);
        //                 break;
        //             case GenerateMethods.Random:
        //                 grid = MMGridGeneratorRandom.Generate(width, height, random.Next(), 10);
        //                 break;
        //             default:
        //                 throw new NotImplementedException();
        //         }
        //
        //         for (int i = 0; i < width; i++)
        //         {
        //             for (int j = 0; j < height; j++)
        //             {
        //                 // 如果高于地平线则不生成
        //                 if (j >= horizonHeight)
        //                 {
        //                     grid[i, j] = 0;
        //                 }
        //
        //                 // 如果该处有障碍物则不生成
        //                 if (obstacleGrid[i, j] >= 1)
        //                 {
        //                     grid[i, j] = 0;
        //                 }
        //
        //                 // 设置新的障碍物
        //                 if (grid[i, j] >= 1)
        //                 {
        //                     obstacleGrid[i, j] = 1;
        //                 }
        //             }
        //         }
        //
        //         // 生成对象
        //         var cellPosition = TargetGrid.WorldToCell(mineGenerateData.mine.transform.position);
        //         for (int i = 0; i < width; i++)
        //         {
        //             for (int j = 0; j < height; j++)
        //             {
        //                 if (grid[i, j] >= 1)
        //                 {
        //                     // 生成对象
        //                     GameObject mine = Instantiate(mineGenerateData.mine, _mineSceneGameObject.transform);
        //                     // 设置位置
        //                     cellPosition.x = i;
        //                     cellPosition.y = j;
        //                     cellPosition += MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
        //                     var position = TargetGrid.GetCellCenterWorld(cellPosition);
        //                     position.z = -1f;
        //                     mine.transform.position = position;
        //                     // 设置名称
        //                     mine.name = mineGenerateData.name;
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}