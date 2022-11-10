using System;
using UnityEngine;

namespace UI_Scripts
{
    public static class WavesLib2
    {
        public static Vector3 SmoothMultipleWaves(float u, float v, float time, float range, params Func<float, float, float, Vector3>[] waves)
        {
            float step     = 1f   / waves.Length;
            float halfStep = step / 2;

            Vector3 result = Vector3.zero;

            range = Mathf.Clamp(range, 0 +halfStep , 1);
            for (int i = 0; i < waves.Length; i++)
            {
                if (i == 0) 
                {
                    
                }
                
                float rangeMid = i * step + halfStep;
                float length   = Mathf.Abs(range - rangeMid);
                length = Mathf.Min(step, length);
                float force = step - length;
                result += force >= 0.01f ? force * waves[i](u, v, time) : Vector3.zero;
            }

            return result;
        }

        public static Vector3 Sin(float u, float v, float t)
        {
            float x = u;
            float y = v;
            float z = Mathf.Sin(Mathf.PI * (u + t));
            return new Vector3(x, y, z);
        }

        public static Vector3 Wave(float u, float v, float t)
        {
            Vector3 p;
            p.x = u;
            p.y = Mathf.Sin(Mathf.PI * (u + v + t));
            p.z = v;
            return p;
        }

        public static Vector3 Origin(float u, float v, float t)
        {
            Vector3 p;
            p.x = u;
            p.y = 0f;
            p.z = v;
            return p;
        }

        public static Vector3 Sphere0(float u, float v, float t)
        {
            float   scale = 0.9f + 0.1f * Mathf.Sin(Mathf.PI * (8f * v + t));
            float   r     = Mathf.Cos(0.5f * Mathf.PI * v); // v 作为 0-1 的 45° 角来计算 x 坐标
            Vector3 p;
            p.x = scale * r * Mathf.Sin(Mathf.PI * u);
            p.y = scale * Mathf.Sin(0.5f         * Mathf.PI * v); // v 作为 0-1 的 45° 角来计算 y 坐标
            p.z = scale * r * Mathf.Cos(Mathf.PI * u);
            return p;
        }

        public static Vector3 Sphere1(float u, float v, float t)
        {
            float   scale = 0.9f + 0.1f * Mathf.Sin(Mathf.PI * (6f * u + 2f * v + t));
            float   r     = Mathf.Cos(0.5f * Mathf.PI * v); // v 作为 0-1 的 45° 角来计算 x 坐标
            Vector3 p;
            p.x = scale * r * Mathf.Sin(Mathf.PI * u);
            p.y = scale * Mathf.Sin(0.5f         * Mathf.PI * v); // v 作为 0-1 的 45° 角来计算 y 坐标
            p.z = scale * r * Mathf.Cos(Mathf.PI * u);
            return p;
        }

        public static Vector3 Sphere2(float u, float v, float t)
        {
            float   scale = 0.9f + 0.1f * Mathf.Sin(Mathf.PI * (8f * u + t));
            float   r     = Mathf.Cos(0.5f * Mathf.PI * v); // v 作为 0-1 的 45° 角来计算 x 坐标
            Vector3 p;
            p.x = scale * r * Mathf.Sin(Mathf.PI * u);
            p.y = scale * Mathf.Sin(0.5f         * Mathf.PI * v); // v 作为 0-1 的 45° 角来计算 y 坐标
            p.z = scale * r * Mathf.Cos(Mathf.PI * u);
            return p;
        }
    }
}