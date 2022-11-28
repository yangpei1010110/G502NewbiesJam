using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

namespace TestScripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererTest : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        public  Vector3[]    data;
        public  int          pointCount = 10;

        private void Awake()
        {
            _lineRenderer ??= GetComponent<LineRenderer>();
        }

        private float GetY(int j, int i, float x)
        {
            if (j == 0)
            {
                return data[i - 1].y;
            }

            float t_ij = data[i + j - 1].x;
            float t_i  = data[i     - 1].x;

            float t = x;

            return (
                       (t_ij - t)   * GetY(j - 1, i, x)
                     + (t    - t_i) * GetY(j - 1, i + 1, x)
                   )
                  /
                   (
                       t_ij - t_i
                   );
        }

        private void Update()
        {
            if (data != null && data.Length > 2)
            {
                Array.Sort(data, (vl, vr) => (int)math.ceil(math.abs(vl.z - vr.z)));
                float minX = float.MaxValue;
                float maxX = float.MinValue;
                for (int i = 0; i < data.Length; i++)
                {
                    minX = math.min(minX, data[i].x);
                    maxX = math.max(maxX, data[i].x);
                }

                Vector3[] tempData = new Vector3[pointCount + 1];
                float     step     = (maxX - minX) / pointCount;
                for (int i = 0; i < tempData.Length; i++)
                {
                    tempData[i].x = minX + step * i;
                    tempData[i].y = GetY(data.Length - 1, 1, tempData[i].x);
                }

                _lineRenderer.positionCount = tempData.Length;
                _lineRenderer.SetPositions(tempData);
            }
        }

        private void OnDrawGizmos()
        {
            if (data != null && data.Length > 0)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < data.Length; i++)
                {
                    Gizmos.DrawSphere(data[i], 0.1f);
                }
            }

            if (_lineRenderer != null && _lineRenderer.positionCount > 0)
            {
                Gizmos.color = Color.blue;
                for (int i = 0; i < _lineRenderer.positionCount; i++)
                {
                    Gizmos.DrawSphere(_lineRenderer.GetPosition(i), 0.02f);
                }
            }
        }
    }
}