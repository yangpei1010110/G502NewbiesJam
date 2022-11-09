using System;
using System.Collections;
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
            _points = new Transform[resolution * resolution];
            if (!pointPrefab)
            {
                throw new Exception("No point prefab");
            }

            for (int j = 0; j < resolution; j++)
            {
                for (int i = 0; i < resolution; i++)
                {
                    float step = length / resolution;

                    Transform point    = _points[j * resolution + i] = Instantiate(pointPrefab).transform;
                    Vector3   position = point.localPosition;

                    position.x = (i) * step - length / 2f;
                    position.z = (j) * step - length / 2f;

                    point.localPosition = position;
                    point.localScale    = (Vector3.one * step) * scale;
                }
            }

            StartCoroutine(BoxUpdate());
        }

        private IEnumerator BoxUpdate()
        {
            int   count       = 0;
            int   returnFrame = 1000;
            float targetFrame = 90f;
            while (true)
            {
                foreach (Transform point in _points)
                {
                    count++;


                    if (count % returnFrame == 0)
                    {
                        returnFrame += 1f / Time.unscaledDeltaTime > targetFrame ? 50 : -50;
                        returnFrame =  Mathf.Clamp(returnFrame, 50, 10000);
                        yield return null;
                    }

                    Vector3 position = point.localPosition;

                    position.y = 1f * WavesLib.SmoothMultipleWaves(position.x, Time.time, functionRange,
                                                                   WavesLib.Sin,
                                                                   WavesLib.Ripple2,
                                                                   // WavesLib.Ripple1,
                                                                   // WavesLib.Ripple,
                                                                   WavesLib.Sin2
                    );

                    point.localPosition = position;
                }
            }
        }
    }
}