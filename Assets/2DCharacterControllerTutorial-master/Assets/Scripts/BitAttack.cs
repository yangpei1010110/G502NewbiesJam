using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitAttack : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
           {
              GetComponent<Collider2D>().enabled = true;
            }
          else if (Input.GetMouseButtonDown(1))
          {
               GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //other.gameObject.GetComponent<AssembleHealth>().TakeDamage(damage);
        Debug.Log(2);
    }

}
