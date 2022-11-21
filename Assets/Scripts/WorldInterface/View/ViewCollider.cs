using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace WorldInterface.View
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider2D))]
    public class ViewCollider : MonoBehaviour
    {
        #region 视角变量

        [LabelText("视角目标"),] public GameObject Player;
        [LabelText("视角"),]   public Vector3    viewLine; // 需要归一化

        private Rect viewRect; // 视角检测盒

        [LabelText("视角检测区域盒"),] private float _viewAngle; // 视角角度

        private float ViewAngle
        {
            get => _viewAngle;
            set
            {
                _viewAngle  = value = math.clamp(value, 0f, 360f);
                _viewRadian = math.radians(value);
            }
        } // 视角角度

        [LabelText("视角角度"),] private float _viewRadian;

        [LabelText("视角弧度"),]
        public float ViewRadian
        {
            get => _viewRadian;
            set
            {
                _viewRadian = value = math.clamp(value, 0f, math.PI * 2f);
                _viewAngle  = math.degrees(value);
            }
        } // 视角弧度

        #endregion

        #region 限制检测数量变量

        // 限制检测范围用碰撞盒
        private          BoxCollider2D     _subViewCollider;
        private readonly IList<GameObject> _viewList = new List<GameObject>();

        #endregion

        #region 光影遮罩变量

        private Mesh                        _lightMask;                                          // 光照遮罩
        private IList<Vector3>              _vertices  = new List<Vector3>();                    // 顶点
        private IList<int>                  _triangles = new List<int>();                        // 三角形
        private IDictionary<float, Vector3> _viewPoint = new SortedDictionary<float, Vector3>(); // 视角点

        #endregion

        private void Awake()
        {
            _lightMask ??= new Mesh();
            {
                // set viewRect is camera to screenRect
                var camera = Camera.main;
                // var rect   = camera.rect;
                // var width  = rect.width * Screen.width;
                // var height = rect.height * Screen.height;
                // var x      = (Screen.width - width) / 2;
                // var y      = (Screen.height - height) / 2;

                var viewportRect          = camera.rect;
                var bottomLeft            = camera.ViewportToWorldPoint(new Vector3(viewportRect.x, viewportRect.y, camera.nearClipPlane));
                var topRight              = camera.ViewportToWorldPoint(new Vector3(viewportRect.width, viewportRect.height, camera.nearClipPlane));
                var cameraWorldScreenRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
                // cameraWorldScreenRect.size *= 2f;

                // var cameraWorldWidth  = camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
                // var cameraWorldHeight = camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
                viewRect = new Rect(Player.transform.position.x, Player.transform.position.y, cameraWorldScreenRect.width, cameraWorldScreenRect.height);
            }
            {
                // 初始化检测盒

                _subViewCollider           = gameObject.GetOrAddComponent<BoxCollider2D>();
                _subViewCollider.isTrigger = true;
                _subViewCollider.size      = viewRect.size;
                {
                    _viewList.Clear();
                }
            }
        }

        // private void Update()
        // {
        //     var player = Player.transform.position;
        //     // view line is player to mouse
        //     viewLine = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player).normalized;
        //     // 获取视角内顶点数
        //     {
        //         _vertices.Clear();
        //         _vertices.Add(player);
        //         // add top left, top right, bottom left, bottom right
        //         _vertices.Add(player + new Vector3(viewRect.xMin, viewRect.yMax));
        //         _vertices.Add(player + new Vector3(viewRect.xMax, viewRect.yMax));
        //         _vertices.Add(player + new Vector3(viewRect.xMin, viewRect.yMin));
        //         _vertices.Add(player + new Vector3(viewRect.xMax, viewRect.yMin));
        //
        //         foreach (GameObject view in _viewList)
        //         {
        //             // 检测是否在视角内
        //             float radian = math.dot(viewLine, view.transform.position);
        //             if (radian > ViewRadian)
        //             {
        //                 _viewPoint.Add(radian, view.transform.position);
        //             }
        //         }
        //     }
        //     {
        //         // check player point and view point if not collider then add to vertices
        //         foreach (Vector3 point in _viewPoint.Values)
        //         {
        //             if (!Physics2D.Linecast(player, point))
        //             {
        //                 _vertices.Add(point);
        //             }
        //         }
        //     }
        //     {
        //         // 生成遮罩
        //         _triangles.Clear();
        //         for (int i = 1; i < _vertices.Count; i++)
        //         {
        //             _triangles.Add(0);
        //             _triangles.Add(i);
        //             _triangles.Add(i + 1);
        //         }
        //     }
        //     {
        //         // 生成遮罩
        //         _lightMask.Clear();
        //         _lightMask.vertices  = _vertices.ToArray();
        //         _lightMask.triangles = _triangles.ToArray();
        //     }
        //     {
        //         // set mask is red
        //         GetComponent<MeshFilter>().mesh             = _lightMask;
        //         GetComponent<MeshRenderer>().material.color = Color.red;
        //     }
        // }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player"))
            {
                _viewList.Add(col.gameObject);
            }
        }
    }
}