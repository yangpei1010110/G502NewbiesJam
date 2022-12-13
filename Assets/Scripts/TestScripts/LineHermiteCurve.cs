using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace TestScripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineHermiteCurve : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        [LabelText("起始位置")] public Vector3 startPosition;
        [LabelText("起始速度")] public Vector3 startVelocity;
        [LabelText("终止位置")] public Vector3 endPosition;
        [LabelText("终止速度")] public Vector3 endVelocity;

        public int pointCount = 50;

        private void Awake()
        {
            _lineRenderer ??= GetComponent<LineRenderer>();
        }

        private Vector3 HermiteCurve(Vector3 startPos, Vector3 startVelocity, Vector3 endVelocity, Vector3 endPos, float time)
        {
            time = math.clamp(time, 0f, 1f);

            var result0 = (1 - 3 * math.pow(time, 2) + 2 * math.pow(time, 3)) * startPos;

            var result1 = (time - 2 * math.pow(time, 2) + math.pow(time, 3)) * startVelocity;

            var result2 = (-math.pow(time, 2) + math.pow(time, 3)) * endVelocity;

            var result3 = (3 * math.pow(time, 2) - 2 * math.pow(time, 3)) * endPos;

            return result0 + result1 + result2 + result3;
        }
        
        private void Update()
        {
            Vector3[] tempData = new Vector3[pointCount + 1];
            float     step     = 1f / pointCount;
            for (int i = 0; i < tempData.Length; i++)
            {
                tempData[i] = HermiteCurve(startPosition, startVelocity, endVelocity, endPosition, step * i);
            }

            _lineRenderer.positionCount = tempData.Length;
            _lineRenderer.SetPositions(tempData);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startPosition, 0.1f);
            Gizmos.DrawLine(startPosition, startPosition + startVelocity);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(endPosition, 0.1f);
            Gizmos.DrawLine(endPosition, endPosition + endVelocity);

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