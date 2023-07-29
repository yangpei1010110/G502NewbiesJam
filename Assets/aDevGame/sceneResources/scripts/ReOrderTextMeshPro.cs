using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

namespace aDevGame.sceneResources.scripts
{
    public class ReOrderTextMeshPro : MonoBehaviour
    {
        [LabelText("Awake时修改文字渲染Order")]
        public bool autoReOrderOnAwake = true;

        private void Awake()
        {
            if (autoReOrderOnAwake)
            {
                ReOrder();
            }
        }

        /// <summary>
        /// 修改文字渲染Order
        /// </summary>
        [Button]
        public void ReOrder(string tmpName = "name", string spriteName = "build.sprite")
        {
            SpriteRenderer sprite;
            {
                Transform tempTransform = this.transform;
                foreach (string tempName in spriteName.Split('.'))
                {
                    tempTransform = tempTransform.Find(tempName);
                }

                sprite = tempTransform.GetComponent<SpriteRenderer>();
            }
            TextMeshPro tmpRenderer;
            {
                Transform tempTransform = this.transform;
                foreach (string tempName in tmpName.Split('.'))
                {
                    tempTransform = tempTransform.Find(tempName);
                }

                tmpRenderer = tempTransform.GetComponent<TextMeshPro>();
            }

            tmpRenderer.sortingLayerID = sprite.sortingLayerID;
            tmpRenderer.sortingOrder = sprite.sortingOrder + 1;

            // resize tmp form sprite scale
            tmpRenderer.GetComponent<RectTransform>().sizeDelta = sprite.transform.localScale;
        }
    }
}