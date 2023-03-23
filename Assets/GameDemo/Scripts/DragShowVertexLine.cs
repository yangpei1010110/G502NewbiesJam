using System.Linq;
using GameDemo.Scripts.Children;
using GameDemo.Scripts.Extensions;
using GameDemo.Scripts.ScriptableObjects;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GameDemo.Scripts
{
    public class DragShowVertexLine : MonoBehaviour
                                    , IBeginDragHandler
                                    , IEndDragHandler

    {
        public bool isBlock = false;
        public bool isBase  = false;

        public MapDataStatic MapData => MapDataStatic.Instance;

        public int2 LocalBlockPos => new int2((int)this.transform.position.x, (int)this.transform.position.z);

        public int2[] localBlockOffset;

        public int2[] GetLocalBlocks(int2 offset)
        {
            return localBlockOffset.Select(o => offset + o).ToArray();
        }

        void Start()
        {
            // 初始化坐标
            this.transform.position = math.round(this.transform.position);

            var children = this.transform.GetAllChildren()
                               .Where(t => t.CompareTag("BaseCube"))
                               .ToArray();

            // 初始化子坐标
            localBlockOffset = children
                              .Select(child =>
                               {
                                   var childPos = child.localPosition;
                                   child.localPosition = math.round(childPos);
                                   return new int2((int)childPos.x, (int)childPos.z);
                               })
                              .ToArray();

            if (!MapData.SetMapBlock(GetLocalBlocks(LocalBlockPos)))
            {
                throw new System.Exception("Map block is not empty.");
            }


            // 绑定事件到子对象
            {
                foreach (var child in children)
                {
                    child.AddComponent<EventDragHandlerToParent>();
                }
            }
        }

        private bool    _isDrag = false;
        private Vector3 _lastPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDrag       = true;
            _lastPosition = eventData.pointerCurrentRaycast.worldPosition - this.transform.position;
        }

        private void Update()
        {
            // 拖动
            if (_isDrag && !isBase)
            {
                var mousePosition = Mouse.current.position.ReadValue();
                var ray           = Camera.main.ScreenPointToRay(mousePosition);

                // get the point on the plane with x z
                var plane = new Plane(Vector3.up, Vector3.zero);
                if (plane.Raycast(ray, out var distance))
                {
                    var point = ray.GetPoint(distance);

                    if (isBlock)
                    {
                        var newPoint  = (Vector3)math.round(point - _lastPosition);
                        var newPoint2 = new int2((int)newPoint.x, (int)newPoint.z);
                        var thisPos   = this.transform.position;
                        var thisPoint = new int2((int)thisPos.x, (int)thisPos.z);

                        if (newPoint2.x != thisPoint.x || newPoint2.y != thisPoint.y)
                        {
                            if (MapData.SetMapBlock(GetLocalBlocks(newPoint2), GetLocalBlocks(thisPoint)))
                            {
                                MapData.ClearMapBlock(GetLocalBlocks(thisPoint), GetLocalBlocks(newPoint2));
                                this.transform.position = newPoint;
                            }
                        }
                    }
                    else
                    {
                        this.transform.position = point - _lastPosition;
                    }
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDrag = false;
        }

        private void OnDrawGizmos()
        {
            foreach (int2 xz in MapData.AllMapData)
            {
                Gizmos.DrawSphere(new Vector3(xz.x, 0, xz.y), 1f);
            }
        }
    }
}