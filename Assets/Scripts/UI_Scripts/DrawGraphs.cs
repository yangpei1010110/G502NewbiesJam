using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace UI_Scripts
{
    public class DrawGraphs : MonoBehaviour
    {
        [SerializeField, Range(10, 500),]   public int   resolution = 10;
        [SerializeField, Range(0.5f, 10f),] public float length     = 10f;
        [SerializeField, Range(0.1f, 2f),]  public float scale      = 0.5f;
        [SerializeField, Range(0, 1),]      public float functionRange;

        [SerializeField] public GameObject pointPrefab;


        private void Awake()
        {
            Transform[] points = new Transform[resolution * resolution];
            if (!pointPrefab)
            {
                throw new Exception("No point prefab");
            }

            float step = length / resolution;

            for (int i = 0; i < points.Length; i++)
            {
                Transform point = points[i] = Instantiate(pointPrefab).transform;
                point.localScale = Vector3.one * step * scale;
            }

            _transformAccessArray = new TransformAccessArray(points);
            _computeUpdate = new ComputeUpdate()
            {
                _time          = Time.time,
                _functionRange = functionRange,
                _resolution    = resolution,
                _length        = length,
            };
        }

        public static Func<float, float, float, float>[] waves = new Func<float, float, float, float>[]
        {
            WavesLib._Sin4,
            WavesLib._Ripple2,
            // WavesLib._Ripple1,
            // WavesLib._Ripple,
            WavesLib._Sin2,
            WavesLib._Ripple3,
        };

        private TransformAccessArray _transformAccessArray;

        private static Func<float, float, float, Vector3>[] _w = new Func<float, float, float, Vector3>[]
        {
            WavesLib2.Origin,
            WavesLib2.Sin,
            WavesLib2.Sphere0,
            WavesLib2.Sphere1,
            WavesLib2.Sphere2,
        };

        private struct ComputeUpdate : IJobParallelForTransform
        {
            [ReadOnly] public float _time;
            [ReadOnly] public float _functionRange;
            [ReadOnly] public int   _resolution;
            [ReadOnly] public float _length;

            public void Execute(int index, TransformAccess transform)
            {
                float step = 2f    / _resolution;
                int   x    = index % _resolution;
                int   z    = index / _resolution;

                float u = (x + 0.5f) * step - 1f;
                float v = (z + 0.5f) * step - 1f;

                transform.position = _length * WavesLib2.SmoothMultipleWaves(u, v, _time, _functionRange, _w);
            }
        }

        private ComputeUpdate _computeUpdate;

        private void Update()
        {
            _computeUpdate._time          = Time.time;
            _computeUpdate._functionRange = functionRange;
            _computeUpdate._resolution    = resolution;
            _computeUpdate._length        = length;

            _computeUpdate.Schedule(_transformAccessArray);
        }

        private void OnDestroy()
        {
            _transformAccessArray.Dispose();
        }
    }
}