using MoreMountains.TopDownEngine;
using UnityEngine;

public class StopBlockOnDeath : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Health>().OnDeath += () => GetComponent<Loot>().SpawnOneLoot();
    }
}