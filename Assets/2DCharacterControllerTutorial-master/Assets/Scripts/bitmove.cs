using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bitmove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 获取鼠标在游戏中的世界坐标
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
    // 计算物体朝向鼠标方向的角度
    float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

    // 使物体朝向鼠标方向旋转
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
