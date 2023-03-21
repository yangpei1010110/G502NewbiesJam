using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace GameDemo.Scripts.ScriptableObjects
{
    public class MapDataStatic
    {
        private static MapDataStatic _instance;

        public static MapDataStatic Instance
        {
            get
            {
                _instance ??= new MapDataStatic();
                return _instance;
            }
        }

        private bool[] _mapData;

        private int _mapWidth = 256;

        public int MapWidth
        {
            get => _mapWidth;
            set
            {
                if (value < 0 || value % 2 == 1)
                {
                    throw new Exception("Map width must be a positive even number.");
                }
                else
                {
                    _mapWidth = value;
                }
            }
        }

        private int _mapHeight = 256;

        public int MapHeight
        {
            get => _mapHeight;
            set
            {
                if (value < 0 || value % 2 == 1)
                {
                    throw new Exception("Map width must be a positive even number.");
                }
                else
                {
                    _mapHeight = value;
                }
            }
        }

        public int2[] AllMapData
        {
            get
            {
                var allMapData = new List<int2>();
                for (int x = 0; x < MapWidth; x++)
                {
                    for (int y = 0; y < MapHeight; y++)
                    {
                        if (this[new int2(x, y)])
                        {
                            allMapData.Add(ToWorldPos(new int2(x, y)));
                        }
                    }
                }

                return allMapData.ToArray();
            }
        }

        public int2 MapMappingOffset => new(MapWidth  / 2, MapHeight  / 2);
        public int2 MapOrigin        => new(-MapWidth / 2, -MapHeight / 2);

        public int2 ToMapPos(int2 pos)
        {
            return pos + MapMappingOffset;
        }

        public int2 ToWorldPos(int2 pos)
        {
            return pos - MapMappingOffset;
        }

        private MapDataStatic()
        {
            if (MapWidth % 2 == 1 || MapHeight % 2 == 1)
            {
                throw new Exception("Map width and height must be even numbers.");
            }

            _mapData = new bool[MapWidth * MapHeight];
        }

        public bool this[int2 xy]
        {
            get
            {
                if (!IsMapInside(xy))
                {
                    throw new Exception("Out of range.");
                }
                else
                {
                    return _mapData[xy.y * MapWidth + xy.x];
                }
            }
        }

        public bool IsMapInside(int2 xy)
        {
            xy = ToMapPos(xy);
            return xy.x >= 0 || xy.x < MapWidth || xy.y >= 0 || xy.y < MapHeight;
        }

        public bool SetMapBlock(int2[] values, int2[] ignore = null)
        {
            ignore ??= Array.Empty<int2>();
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = ToMapPos(values[i]);
            }

            for (int i = 0; i < ignore.Length; i++)
            {
                ignore[i] = ToMapPos(ignore[i]);
            }


            foreach (int2 value in values)
            {
                // 超出范围
                if (!IsMapInside(value))
                {
                    return false;
                }

                // 已经占用
                if (_mapData[value.y * MapWidth + value.x] && !ignore.Contains(value))
                {
                    return false;
                }
            }

            foreach (int2 value in values)
            {
                _mapData[value.y * MapWidth + value.x] = true;
            }

            return true;
        }

        public bool ClearMapBlock(int2[] values, int2[] ignore = null)
        {
            ignore ??= Array.Empty<int2>();
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = ToMapPos(values[i]);
            }

            for (int i = 0; i < ignore.Length; i++)
            {
                ignore[i] = ToMapPos(ignore[i]);
            }

            foreach (int2 value in values)
            {
                if (!IsMapInside(value))
                {
                    return false;
                }
            }

            foreach (int2 value in values)
            {
                if (!ignore.Contains(value))
                {
                    _mapData[value.y * MapWidth + value.x] = false;
                }
            }

            return true;
        }
    }
}