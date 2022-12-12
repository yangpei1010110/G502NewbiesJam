using System;
using System.Collections.Generic;
using UnityEngine;

namespace EffectScripts
{
    public class ParticleEffectRenderScript : MonoBehaviour
    {
        public Vector3 particlePositionCenter;
        public float   particleRange;

        private List<Vector4> customData = new List<Vector4>();

        private void Awake()
        {
            Collider2D component = this.GetComponent<Collider2D>();
            particlePositionCenter = component.bounds.center;

            var particleSystem = this.GetComponent<ParticleSystem>();
            particleSystem.GetCustomParticleData(customData, ParticleSystemCustomData.Custom1);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(particlePositionCenter, particleRange);
        }
    }
}