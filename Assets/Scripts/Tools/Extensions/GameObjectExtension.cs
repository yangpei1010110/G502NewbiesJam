using System;
using UnityEngine;

namespace Tools.Extensions
{
    public static class GameObjectExtension
    {
        public static T GetOrAddComponentWithInit<T>(this GameObject gameObject, Action<T> beforeAction) where T : Component
        {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponentWithInit(beforeAction);
        }

        public static T AddComponentWithInit<T>(this GameObject gameObject, Action<T> beforeAction) where T : Component
        {
            bool oldActive = gameObject.activeSelf;
            gameObject.SetActive(false);
            T component = gameObject.AddComponent<T>();
            beforeAction(component);
            gameObject.SetActive(oldActive);
            return component;
        }
    }
}