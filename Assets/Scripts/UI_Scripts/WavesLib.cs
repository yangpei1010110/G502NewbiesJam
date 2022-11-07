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
            float step     = range / waves.Length;
            float halfStep = step  / 2;

            float result = 0;
            for (int i = 0; i < waves.Length; i++)
            {
                float rangeMid = i * step + halfStep;
                float length   = Mathf.Abs(rangeMid - range);
                result += waves[i](x + step * i, time);
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

        public static float Sin(float x, float time)
        {
            return Mathf.Sin(Mathf.PI * (x + time));
        }

        public static float Sin2(float x, float time)
        {
            return Mathf.Sin(Mathf.PI * 2f * (x + time)) * 0.5f;
        }

        public static float Sin3(float x, float time)
        {
            return Mathf.Sin(Mathf.PI * (x + 0.5f * time));
        }

        public static float Cos(float x, float time)
        {
            return Mathf.Cos(Mathf.PI * (x + time));
        }

        public static float Ripple(float x, float time)
        {
            return Mathf.Sin(4f * Mathf.PI * Mathf.Abs(x));
        }
    }
}