using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace WorldInterface.Action
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TakingCollider : MonoBehaviour
    {
        #region 玩家变量

        [LabelText("拾取者"),]    public GameObject Player;
        [LabelText("吸引触发范围"),] public float      SuckRange = 15f;
        [LabelText("吸引强度"),]   public float      SuckForce = 15f;
        [LabelText("吸引触发范围"),] public float      TakeRange = 15f;

        #endregion

        #region 碰撞盒变量

        private CircleCollider2D    _circleCollider2D;
        private HashSet<Collider2D> _collider2ds;

        #endregion

        private void Awake()
        {
            {
                // 初始化检测盒

                _circleCollider2D           = gameObject.GetOrAddComponent<CircleCollider2D>();
                _circleCollider2D.isTrigger = true;
                _circleCollider2D.radius    = SuckRange;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                if (!_collider2ds.Contains(other))
                {
                    _collider2ds.Add(other);
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (Collider2D other in _collider2ds)
            {
                Vector3 force = this.gameObject.transform.position - other.gameObject.transform.position;
                force =  force.normalized;
                force *= SuckForce;
                force *= Time.fixedDeltaTime;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
            }

            foreach (Collider2D other in _collider2ds)
            {
                float range = Vector3.Distance(this.gameObject.transform.position, other.gameObject.transform.position);
                if (range < TakeRange)
                {
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                if (_collider2ds.Contains(other))
                {
                    _collider2ds.Remove(other);
                }
            }
        }
    }
}