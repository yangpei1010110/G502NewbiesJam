using System.Collections.Generic;
using UnityEngine;

namespace GameDemo.Scripts.Extensions
{
    public static class TransformExtension
    {
        public static IList<Transform> GetAllChildren(this Transform transform)
        {
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            return children;
        }
    }
}