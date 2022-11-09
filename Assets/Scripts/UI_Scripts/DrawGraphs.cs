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

            for (int j = 0; j < resolution; j++)
            {
                for (int i = 0; i < resolution; i++)
                {
                    float step = length / resolution;

                    Transform point    = points[j * resolution + i] = Instantiate(pointPrefab).transform;
                    Vector3   position = point.localPosition;

                    position.x = (i) * step - length / 2f;
                    position.z = (j) * step - length / 2f;

                    point.localPosition = position;
                    point.localScale    = (Vector3.one * step) * scale;
                }
            }

            _transformAccessArray = new TransformAccessArray(points);
            computeUpdate = new ComputeUpdate()
            {
                _time          = Time.time,
                _functionRange = functionRange,
            };
        }

        private NativeArray<Vector3> nativeArray;

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

        private struct ComputeUpdate : IJobParallelForTransform
        {
            public float _time;
            public float _functionRange;

            public void Execute(int index, TransformAccess transform)
            {
                Vector3 nativeArrayPoint = transform.position;
                nativeArrayPoint.y = 3f * WavesLib._SmoothMultipleWaves(nativeArrayPoint.x, nativeArrayPoint.z, _time, _functionRange, waves);
                transform.position = nativeArrayPoint;
            }
        }

        private ComputeUpdate computeUpdate;

        private void Update()
        {
            computeUpdate._time          = Time.time;
            computeUpdate._functionRange = functionRange;

            computeUpdate.Schedule(_transformAccessArray);
        }

        private void OnDestroy()
        {
            _transformAccessArray.Dispose();
        }
    }
}