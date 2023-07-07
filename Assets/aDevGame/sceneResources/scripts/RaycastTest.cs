using UnityEngine;

namespace aDevGame.sceneResources.scripts
{
    public class RaycastTest : MonoBehaviour
    {
        private RaycastHit2D[] results = new RaycastHit2D[10];
        #if UNITY_EDITOR
        void Update()
        {
            var size = Physics2D.RaycastNonAlloc(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, results);
            if (Input.GetMouseButtonDown(0) && size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    var hit2D = results[i];
                    Debug.Log(hit2D.collider.gameObject.name);
                }
            }
        }
        #endif
    }
}