using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace aDevGame.sceneResources.scripts
{
    public class SpriteText : MonoBehaviour
    {
        void Start()
        {
            var thisTransform = transform;
            var parent = thisTransform.parent;

            var parentRenderer = parent.GetComponent<Renderer>();
            var render = GetComponent<Renderer>();
            render.sortingLayerID = parentRenderer.sortingLayerID;
            render.sortingOrder = parentRenderer.sortingOrder + 1;

            var spriteTransform = parent.transform;
            var pos = spriteTransform.position;
            var text = GetComponent<TextMeshPro>();
            text.text = string.Format("{0}, {1}", pos.x, pos.y);

            // scale init
            var rect = this.GetComponent<RectTransform>().rect;
            rect.Set(0, 0, 10, 10);
            // thisTransform.localScale = new Vector3(0.1f, 0.1f, 1);
            // compute
            var parentScale = parent.localScale;
            var thisScale = new Vector3(0.1f, 0.1f, 1);
            var minScale = math.min(parentScale.x, parentScale.y);
            thisScale.x *= parentScale.x / minScale;
            thisScale.y *= parentScale.y / minScale;
            thisTransform.localScale = thisScale;
        }
    }
}