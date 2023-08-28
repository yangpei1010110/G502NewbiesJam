using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //获得角色刚体
    private Rigidbody2D rb;
    //获得角色碰撞器
    private BoxCollider2D cil;
    [Header("移动参数")]
    //设置角色的移动速度
    public float speed = 8f;

    //判断x轴力的方向
    public float xVelocity;
    //判断y轴力的方向
    public float yVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cil = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundMovement();
    }
    void GroundMovement()
    {
        //获得在键盘上输入的移动
        xVelocity = Input.GetAxis("Horizontal");
        yVelocity = Input.GetAxis("Vertical");
        //使角色移动
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        rb.velocity = new Vector2(yVelocity * speed, rb.velocity.x);
        //调用判断角色面向左右的函数
        FilpDirctionx();
        //调用判断角色面向上下的函数
        FilpDirctiony();
    }
    //判断游戏角色面向左右的函数
    void FilpDirctionx()
    {
        //如果x轴的速度小于0，游戏角色会面向左
        if (xVelocity < 0)
        {
         transform.localScale = new Vector3(-1,1,1);   
        }
        //如果x轴的速度大于0，游戏角色会面向右
        if (xVelocity > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
    }
    //判断游戏角色面向上下的函数
    void FilpDirctiony()
    {
        //如果y轴的速度小于0，游戏角色向下
        if (yVelocity < 0)
        {
            transform.localScale = new Vector3(1,-1,1);
        }
        //如果y轴的速度大于0，游戏角色向上
        if (yVelocity > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
    }
}
