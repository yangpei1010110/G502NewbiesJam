using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using aDevGame.UI;
using GameDemo.Scripts.Extensions;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using Sirenix.OdinInspector;
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
            //原有的生成器方法
            base.Generate();
            // 新物资生成
            MinesGenerate();
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

        [Header("额外生成对象")]
        [InspectorName("可生成对象")]
        public List<MineGenerateData> MineObjects;
        private GameObject _mineSceneGameObject;
        private string     _mineSceneGameObjectName = "Mines";


        public void MinesGenerate()
        {
            // 找到矿物的场景父类或生成
            _mineSceneGameObject ??= GameObject.Find(_mineSceneGameObjectName);
            if (!_mineSceneGameObject)
            {
                _mineSceneGameObject = new GameObject(_mineSceneGameObjectName);
                _mineSceneGameObject.name = _mineSceneGameObjectName;
            }

            // 清理原有全部子类
            foreach (Transform child in _mineSceneGameObject.transform.GetAllChildren())
            {
                DestroyImmediate(child.gameObject);
            }

            // 设置随机数种子
            var random = new System.Random(GlobalSeed);
            UnityEngine.Random.InitState(GlobalSeed);
            int width = UnityEngine.Random.Range(GridWidth.x, GridWidth.y);
            int height = UnityEngine.Random.Range(GridHeight.x, GridHeight.y);
            // 找到障碍物的网格
            int[,] obstacleGrid = MMGridGenerator.TilemapToGrid(ObstaclesTilemap, width, height);
            // 添加出生点和终点也为障碍物
            var spawnGrid = TargetGrid.WorldToCell(InitialSpawn.position) - MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
            var exitGrid = TargetGrid.WorldToCell(Exit.position) - MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
            obstacleGrid[spawnGrid.x, spawnGrid.y] = 1;
            obstacleGrid[exitGrid.x, exitGrid.y] = 1;

            // 生成矿物
            foreach (MineGenerateData mineGenerateData in MineObjects)
            {
                int[,] grid;
                switch (mineGenerateData.generateMethods)
                {
                    case GenerateMethods.Perlin:
                        grid = MMGridGeneratorPerlinNoise.Generate(width, height, Random.value);
                        break;
                    case GenerateMethods.PerlinGround:
                        grid = MMGridGeneratorPerlinNoiseGround.Generate(width, height, Random.value);
                        break;
                    case GenerateMethods.Random:
                        grid = MMGridGeneratorRandom.Generate(width, height, random.Next(), 10);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        // 如果该处有障碍物则不生成
                        if (obstacleGrid[i, j] >= 1)
                        {
                            grid[i, j] = 0;
                        }

                        // 设置新的障碍物
                        if (grid[i, j] >= 1)
                        {
                            obstacleGrid[i, j] = 1;
                        }
                    }
                }

                // 生成对象
                var cellPosition = TargetGrid.WorldToCell(mineGenerateData.mine.transform.position);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (grid[i, j] >= 1)
                        {
                            // 生成对象
                            GameObject mine = Instantiate(mineGenerateData.mine, _mineSceneGameObject.transform);
                            // 设置位置
                            cellPosition.x = i;
                            cellPosition.y = j;
                            cellPosition += MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
                            mine.transform.position = TargetGrid.GetCellCenterWorld(cellPosition);
                            // 设置名称
                            mine.name = mineGenerateData.name;
                        }
                    }
                }
            }
        }
    }
}