using System;
using TMPro;
using UnityEngine;

namespace aDevGame.sceneResources.scripts.builder
{
    public class BuilderStructure : MonoBehaviour
    {
        public Vector2Int center;
        public Vector2Int size;

        public string      structureName;
        public TextMeshPro displayName;

        public float buildCost;
        public float buildProgress;
        public bool  isBuilt;

        public Action onBuilt;
        public Action workUpdate;

        private void Awake()
        {
            displayName = transform.Find("name").GetComponent<TextMeshPro>();
        }

        private void Start()
        {
            var build = transform.Find("build");
            build.localScale = new Vector3(size.x, size.y, 1);
            Build(0f);
            displayName.GetComponent<RectTransform>().sizeDelta = build.localScale;
        }

        public void Build(float workValue)
        {
            buildProgress += workValue;
            if (buildProgress >= buildCost)
            {
                isBuilt = true;
                onBuilt?.Invoke();
                displayName.text = structureName;
            }
        }

        public void Update()
        {
            if (isBuilt)
            {
                workUpdate?.Invoke();
            }
            else
            {
                if (!$"{structureName}-{buildProgress}/{buildCost}".Equals(displayName.text))
                {
                    displayName.text = $"{structureName}-{buildProgress}/{buildCost}";
                }
            }
        }
    }
}