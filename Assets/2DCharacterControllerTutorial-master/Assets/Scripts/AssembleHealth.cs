using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssembleHealth : MonoBehaviour
{

      public int Health; // 定义物体的血量变量
    // Start is called before the first frame update
    void Start()
    {
        
    }
 public void TakeDamage(int damage)
    {
        Health -= damage;

        // 可以在此处进行相应的处理，例如判断是否死亡等
        if (Health <= 0)
        {
             Destroy(gameObject);// 物体死亡的逻辑
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
