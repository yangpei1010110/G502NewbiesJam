using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace UI_Scripts
{
    public static class WavesLib
    {
        public static float SmoothMultipleWaves(float x, float time, float range, params Func<float, float, float>[] waves)
        {
            float step     = 1f   / waves.Length;
            float halfStep = step / 2;

            float result = 0;
            for (int i = 0; i < waves.Length; i++)
            {
                float rangeMid = i * step + halfStep;
                float length   = Mathf.Abs(range - rangeMid);
                length = Mathf.Min(step, length);
                float force = step - length;
                result += force >= 0.01f ? force * waves[i](x, time) : 0f;
            }

            return result;
        }

        public static float _SmoothMultipleWaves(float x, float z, float time, float range, params Func<float, float, float, float>[] waves)
        {
            float step     = 1f   / waves.Length;
            float halfStep = step / 2;

            float result = 0;
            for (int i = 0; i < waves.Length; i++)
            {
                float rangeMid = i * step + halfStep;
                float length   = Mathf.Abs(range - rangeMid);
                length = Mathf.Min(step, length);
                float force = step - length;
                result += force >= 0.01f ? force * waves[i](x, z, time) : 0f;
                // result += force * waves[i](x, z, time);
            }

            return result;
        }

        /// <summary>
        /// 叠加波形
        /// </summary>
        public static float MultipleWaves(float x, float time, params Func<float, float, float>[] waves)
        {
            float result = 0;
            foreach (Func<float, float, float> wave in waves)
            {
                result += wave(x, time);
            }

            return result;
        }

        public static float _Sin(float x, float z, float time)
        {
            return (Sin(x, time) + Sin(z, time)) * 0.5f;
        }

        public static float Sin(float x, float time)
        {
            return Mathf.Sin(Mathf.PI * (x + time));
        }

        public static float _Sin2(float x, float z, float time)
        {
            return (Sin2(x, time) + Sin2(z, time)) * 0.5f;
        }

        public static float Sin2(float x, float time)
        {
            return Mathf.Sin(Mathf.PI * 2f * (x + time)) * 0.5f;
        }

        public static float _Sin3(float x, float z, float time)
        {
            return (Sin3(x, time) + Sin3(z, time)) * 0.5f;
        }

        public static float Sin3(float x, float time)
        {
            return Mathf.Sin(Mathf.PI * (x + 0.5f * time));
        }

        public static float _Sin4(float x, float z, float time)
        {
            float y = Mathf.Sin(Mathf.PI * (x + 0.5f * time));
            y += 0.5f * Mathf.Sin(2f * Mathf.PI * (z + time));
            y += Mathf.Sin(Mathf.PI * (x + z + 0.25f * time));
            return y * (1f / 2.5f);
        }

        public static float _Cos(float x, float z, float time)
        {
            return (Cos(x, time) + Cos(z, time)) * 0.5f;
        }

        public static float Cos(float x, float time)
        {
            return Mathf.Cos(Mathf.PI * (x + time));
        }

        public static float _Ripple(float x, float z, float time)
        {
            return (Ripple(x, time) + Ripple(z, time)) * 0.5f;
        }

        public static float Ripple(float x, float time)
        {
            float d = Mathf.Abs(x);
            float y = d;
            return y;
        }

        public static float _Ripple1(float x, float z, float time)
        {
            return (Ripple1(x, time) + Ripple1(z, time)) * 0.5f;
        }

        public static float Ripple1(float x, float time)
        {
            float d = Mathf.Abs(x);
            float y = Mathf.Sin(4f * Mathf.PI * d);
            return y;
        }

        public static float _Ripple2(float x, float z, float time)
        {
            return (Ripple2(x, time) + Ripple2(z, time)) * 0.5f;
        }

        public static float Ripple2(float x, float time)
        {
            float d = Mathf.Abs(x);
            float y = Mathf.Sin(Mathf.PI * (4f * d - time));
            y /= 1f + 10f * d;
            return y;
        }

        public static float _Ripple3(float x, float z, float time)
        {
            float d = Mathf.Sqrt(x * x + z * z);
            float y = Mathf.Sin(Mathf.PI * (4f * d - time));
            return y / (1f + 10f * d);
        }
    }
}