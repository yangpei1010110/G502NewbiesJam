using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace WorldInterface.View
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class ViewCollider : MonoBehaviour
    {
        private void Awake()
        {
            _lightMask = GetComponent<MeshFilter>().mesh;
            {
                Camera  camera                = Camera.main;
                Rect    viewportRect          = camera.rect;
                Vector3 bottomLeft            = camera.ViewportToWorldPoint(new Vector3(viewportRect.x, viewportRect.y, camera.nearClipPlane));
                Vector3 topRight              = camera.ViewportToWorldPoint(new Vector3(viewportRect.width, viewportRect.height, camera.nearClipPlane));
                Rect    cameraWorldScreenRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
                _viewRect = new Rect(Player.transform.position.x, Player.transform.position.y, cameraWorldScreenRect.width, cameraWorldScreenRect.height);
            }
            {
                // 初始化检测盒

                _subViewCollider           = gameObject.GetOrAddComponent<BoxCollider2D>();
                _subViewCollider.isTrigger = true;
                _subViewCollider.size      = _viewRect.size;
                {
                    _viewList.Clear();
                }
            }
        }

        private void Update()
        {
            Vector3 player = Player.transform.position;

            // 计算鼠标指向视线
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = player.z;
                viewLine        = mousePosition - player;
                viewLine.z      = player.z;
                viewLine        = viewLine.normalized;
                ViewRadian      = math.atan2(viewLine.y, viewLine.x);
            }
            // 获取视角内顶点数
            {
                _viewPoint.Clear();
                //top left
                {
                    Vector3 vertex = player + new Vector3(-_viewRect.width / 2f, _viewRect.height / 2f);
                    float   radian = math.atan2(vertex.y - player.y, vertex.x - player.x);
                    _viewPoint.Add(radian, (vertex - player).normalized * viewRange + player);
                }
                //top right
                {
                    Vector3 vertex = player + new Vector3(_viewRect.width / 2f, _viewRect.height / 2f);
                    float   radian = math.atan2(vertex.y - player.y, vertex.x - player.x);
                    _viewPoint.Add(radian, (vertex - player).normalized * viewRange + player);
                }
                // bottom left
                {
                    Vector3 vertex = player + new Vector3(-_viewRect.width / 2f, -_viewRect.height / 2f);
                    float   radian = math.atan2(vertex.y - player.y, vertex.x - player.x);
                    _viewPoint.Add(radian, (vertex - player).normalized * viewRange + player);
                }
                // bottom right
                {
                    Vector3 vertex = player + new Vector3(_viewRect.width / 2f, -_viewRect.height / 2f);
                    float   radian = math.atan2(vertex.y - player.y, vertex.x - player.x);
                    _viewPoint.Add(radian, (vertex - player).normalized * viewRange + player);
                }

                foreach (Collider2D view in _viewList)
                {
                    // 计算需要投射光线的顶点 还有 2 个偏一点的弧度
                    foreach (Vector3 vertex in view.CreateMesh(true, true).vertices)
                    {
                        float distance = (vertex - player).magnitude;
                        float radian   = math.atan2(vertex.y - player.y, vertex.x - player.x);
                        _viewPoint.TryAdd(radian, (vertex - player).normalized * viewRange + player);

                        float   lowRadian = radian   - 0.0001f;
                        float   lowX      = player.x + distance * math.cos(lowRadian);
                        float   lowY      = player.y + distance * math.sin(lowRadian);
                        Vector3 lowVertex = new Vector3(lowX, lowY, player.z);
                        _viewPoint.TryAdd(lowRadian, (lowVertex - player).normalized * viewRange + player);

                        float   highRadian = radian   + 0.0001f;
                        float   highX      = player.x + distance * math.cos(highRadian);
                        float   highY      = player.y + distance * math.sin(highRadian);
                        Vector3 highVertex = new Vector3(highX, highY, player.z);
                        _viewPoint.TryAdd(highRadian, (highVertex - player).normalized * viewRange + player);
                    }
                }
            }
            // 过滤顶点是否被遮挡
            {
                _vertices.Clear();
                _vertices.Add(Vector3.zero);
                // check player point and view point if not collider then add to vertices
                foreach (KeyValuePair<float, Vector3> kv in _viewPoint)
                {
                    Vector3        point           = kv.Value;
                    RaycastHit2D[] raycastHit2DAll = Physics2D.LinecastAll(player, point);
                    // 如果根本没有碰撞
                    if (raycastHit2DAll.Length <= 1)
                    {
                        _vertices.Add(point - player);
                    }
                    // 如果玩家在碰撞体内部
                    else if ((raycastHit2DAll[1].point - (Vector2)player).magnitude < 0.01f)
                    {
                        continue;
                    }
                    // 有碰撞
                    else
                    {
                        _vertices.Add(raycastHit2DAll[1].point - (Vector2)player);
                    }
                }
            }
            {
                // 生成遮罩
                _triangles.Clear();
                for (int i = 1; i < _vertices.Count - 1; i++)
                {
                    _triangles.Add(0);
                    _triangles.Add(i + 1);
                    _triangles.Add(i);
                }

                _triangles.Add(0);
                _triangles.Add(1);
                _triangles.Add(_vertices.Count - 1);
            }
            {
                // 遮罩赋值
                _lightMask.Clear();
                if (_vertices.Count > 1)
                {
                    _lightMask.SetVertices(_vertices.ToArray());
                    _lightMask.SetTriangles(_triangles.ToArray(), 0);
                }
            }
        }

        private void OnDrawGizmos()
        {
            // 编辑器绘制玩家坐标到鼠标的矢量
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Player.transform.position, Player.transform.position + viewLine);

            Gizmos.color = Color.blue;
            for (int i = 1; i < _vertices.Count; i++)
            {
                // 编辑器绘制玩家到顶点的线
                Gizmos.DrawLine(Player.transform.position, Player.transform.position + _vertices[i]);
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                _viewList.Add(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                _viewList.Remove(other);
            }
        }

        #region 视角变量

        [LabelText("视角目标"),]   public  GameObject Player;
        [LabelText("视角"),]     private Vector3    viewLine; // 需要归一化
        [LabelText("光线检测范围"),] public  float      viewRange = 15f;

        private Rect _viewRect; // 视角检测盒

        private float _viewAngle; // 视角角度

        private float ViewAngle
        {
            get => _viewAngle;
            set
            {
                _viewAngle  = value = math.clamp(value, -360f, 360f);
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
                _viewRadian = value = math.clamp(value, -math.PI * 2f, math.PI * 2f);
                _viewAngle  = math.degrees(value);
            }
        } // 视角弧度

        #endregion

        #region 限制检测数量变量

        // 限制检测范围用碰撞盒
        private                      BoxCollider2D    _subViewCollider;
        [LabelText("光线检测对象")] public List<Collider2D> _viewList = new();

        #endregion

        #region 光影遮罩变量

        private                       Mesh                             _lightMask;                       // 光照遮罩
        private                       IList<Vector3>                   _vertices  = new List<Vector3>(); // 顶点
        private                       IList<int>                       _triangles = new List<int>();     // 三角形
        [LabelText("光线检测顶点")] private SortedDictionary<float, Vector3> _viewPoint = new();               // 弧度与顶点

        #endregion
    }
}