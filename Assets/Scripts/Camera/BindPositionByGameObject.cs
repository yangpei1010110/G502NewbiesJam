// using System;
// using UnityEngine;
//
// namespace Camera
// {
//     public class BindPositionByGameObject : MonoBehaviour
//     {
//         public                   GameObject target;
//         [SerializeField] private GameObject current;
//         [SerializeField] private Vector3    offset;
//
//         private void Awake()
//         {
//             if (!target)
//             {
//                 throw new Exception("需要绑定游戏对象目标");
//             }
//
//             // check current is camera
//             current = gameObject;
//             if (current.GetComponent<UnityEngine.Camera>() == null)
//             {
//                 throw new Exception("当前对象不是摄像机");
//             }
//
//             offset = current.transform.position - target.transform.position;
//         }
//
//         private void Update()
//         {
//             // update position
//             current.transform.position = target.transform.position + offset;
//         }
//     }
// }