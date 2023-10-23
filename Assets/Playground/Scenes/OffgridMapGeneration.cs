using System;
using System.Drawing.Printing;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private float[,]     _blockMap;
        private Transform[,] _sprite;

        [SerializeField] [LabelText("强度")] [Range(0f, 1f)]
        float _rate = 1f;
        float _oldRate;

        [SerializeField] [LabelText("地图种子")]
        private int _seed;
        private System.Random _random;


        [Button("随机种子并重新生成")]
        private void RegeneratorSeedAndRebuild()
        {
            _seed = new System.Random().Next();
            Rebuild();
        }

        private void Update()
        {
            if (Math.Abs(_oldRate - _rate) > float.Epsilon)
            {
                _oldRate = _rate;
                Recalculate();
            }
        }

        private void Start()
        {
            RegeneratorSeedAndRebuild();
        }

        [Button("重新生成")]
        private void Rebuild()
        {
            _random = new System.Random(_seed);
            MapBlockRoot.MMDestroyAllChildren();
            _blockMap = null;
            _sprite = null;
            _blockMap = new float[Size.y, Size.x];
            for (int y = 0; y < Size.y; y++)
            for (int x = 0; x < Size.x; x++)
            {
                _blockMap[y, x] = Convert.ToSingle(_random.NextDouble() - 0.5);
            }

            Refresh();
        }

        private void Refresh()
        {
            if (_sprite == null)
            {
                _sprite = new Transform[Size.y, Size.x];
                for (int y = 1; y < Size.y - 1; y++)
                for (int x = 1; x < Size.x - 1; x++)
                {
                    _sprite[y, x] = Instantiate(MapBlockPrefab, MapBlockRoot).transform;
                }
            }

            for (int y = 1; y < Size.y - 1; y++)
            for (int x = 1; x < Size.x - 1; x++)
            {
                _sprite[y, x].GetComponent<SpriteRenderer>().color = IsRed(new Vector2Int(x, y)) ? Color.red : Color.blue;
            }

            Recalculate();
        }

        private void Recalculate()
        {
            for (int y = 1; y < Size.y - 1; y++)
            for (int x = 1; x < Size.x - 1; x++)
            {
                var pos = new Vector2Int(x, y);
                float top;
                float down;
                float left;
                float right;
                if (IsRed(pos))
                {
                    top = Top(pos) * _rate + y + 1;
                    down = Down(pos) * _rate + y;
                    left = Left(pos) * _rate + x;
                    right = Right(pos) * _rate + x + 1;
                }
                else
                {
                    top = Top(pos) * _rate + y + 1;
                    down = Down(pos) * _rate + y;
                    left = Left(pos) * _rate + x;
                    right = Right(pos) * _rate + x + 1;
                }


                _sprite[y, x].localScale = new Vector3(right - left, top - down, 1) * 0.99f;
                _sprite[y, x].position = new Vector3((left + right) / 2f, (top + down) / 2f, 0f);
            }
        }

        private float Top(in Vector2Int xy)
        {
            if (xy.x < 1 || xy.x > Size.x - 2 || xy.y < 1 || xy.y > Size.y - 2)
            {
                return 0f;
            }
            else
            {
                if (IsRed(xy))
                {
                    return _blockMap[xy.y + 1, xy.x + 1];
                }
                else
                {
                    return _blockMap[xy.y + 1, xy.x];
                }
            }
        }

        private float Down(in Vector2Int xy)
        {
            if (xy.x < 1 || xy.x > Size.x - 2 || xy.y < 1 || xy.y > Size.y - 2)
            {
                return 0f;
            }
            else
            {
                if (IsRed(xy))
                {
                    return _blockMap[xy.y, xy.x];
                }
                else
                {
                    return _blockMap[xy.y, xy.x + 1];
                }
            }
        }

        private float Left(in Vector2Int xy)
        {
            if (xy.x < 1 || xy.x > Size.x - 2 || xy.y < 1 || xy.y > Size.y - 2)
            {
                return 0f;
            }
            else
            {
                if (IsRed(xy))
                {
                    return _blockMap[xy.y + 1, xy.x];
                }
                else
                {
                    return _blockMap[xy.y, xy.x];
                }
            }
        }

        private float Right(in Vector2Int xy)
        {
            if (xy.x < 1 || xy.x > Size.x - 2 || xy.y < 1 || xy.y > Size.y - 2)
            {
                return 0f;
            }
            else
            {
                if (IsRed(xy))
                {
                    return _blockMap[xy.y, xy.x + 1];
                }
                else
                {
                    return _blockMap[xy.y + 1, xy.x + 1];
                }
            }
        }

        private bool IsRed(in Vector2Int xy)
        {
            return (xy.x + xy.y) % 2 == 0;
        }
    }
}