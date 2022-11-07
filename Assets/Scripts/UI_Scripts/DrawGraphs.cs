using System;
using UnityEngine;

namespace UI_Scripts
{
    public class DrawGraphs : MonoBehaviour
    {
        [SerializeField, Range(10, 500),]   public int   resolution = 10;
        [SerializeField, Range(0.5f, 10f),] public float length     = 10f;
        [SerializeField, Range(0.1f, 2f),]  public float scale      = 0.5f;
        [SerializeField, Range(0, 1),]      public float functionRange;

        [SerializeField] public GameObject pointPrefab;

        private Transform[] _points;

        private void Awake()
        {
            _points = new Transform[resolution];
            if (!pointPrefab)
            {
                throw new Exception("No point prefab");
            }

            for (int i = 0; i < _points.Length; i++)
            {
                float step = length / _points.Length;

                Transform point    = _points[i] = Instantiate(pointPrefab).transform;
                Vector3   position = point.localPosition;

                position.x = (i) * step - length / 2f;

                point.localPosition = position;
                point.localScale    = (Vector3.one * step) * scale;
            }
        }

        private void Update()
        {
            for (int i = 0; i < _points.Length; i++)
            {
                Transform point    = _points[i];
                Vector3   position = point.localPosition;

                position.y = (1f - functionRange) * WavesLib.Sin(position.x, Time.time)
                           + functionRange        * WavesLib.MultipleWaves(position.x, Time.time, WavesLib.Sin, WavesLib.Sin2, WavesLib.Sin3);

                point.localPosition = position;
            }
        }
    }
}