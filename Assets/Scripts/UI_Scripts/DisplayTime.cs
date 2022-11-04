using System;
using TMPro;
using UnityEngine;

namespace UI_Scripts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DisplayTime : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI display;

        private void Start()
        {
            display = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            display.SetText(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        }
    }
}