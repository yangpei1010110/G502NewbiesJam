using MoreMountains.TopDownEngine;
using UnityEngine;

namespace aDevGame.sceneResources.scripts
{
    public class DamageClickTest : MonoBehaviour
    {
        private void OnMouseEnter()
        {
            Debug.Log("OnMouseEnter");
            gameObject.GetComponent<Health>().Damage(5f, this.gameObject, 0.5f, 0.5f, Vector3.up);
        }
    }
}