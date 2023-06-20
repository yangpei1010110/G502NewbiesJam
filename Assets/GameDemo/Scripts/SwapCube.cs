using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace GameDemo.Scripts
{
    public class SwapCube : MonoBehaviour, IPointerDownHandler
    {
        public GameObject[] cubes;
        public Vector3      swapOffset = new Vector3(-5, 0, 0);

        private void Start()
        {
            cubes = Resources.LoadAll<GameObject>("GameDemo/Prefab/MapPrefabs");
        }

        public void OnPointerDown(PointerEventData eventData) 
        {
            GameObject go = cubes[Random.Range(0, cubes.Length)];

            var t = this.transform;
            go = Instantiate(go, t.position + swapOffset, t.rotation, GameObject.Find("Map").transform);
            var script = go.GetComponent<DragShowVertexLine>();
            script.isBlock = true;
            script.enabled = true;
        }
    }
}