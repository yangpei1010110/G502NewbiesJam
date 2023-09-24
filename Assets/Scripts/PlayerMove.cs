using System;
using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum State
    {
        OnGround,
        OnAir,
    }

    public  State            PlayerState  = State.OnGround;
    public  float            JumpVelocity = 20f;
    public  float            MoveForce    = 20f;
    public  float            MaxForce     = 20f;
    public  float            MaxSpeed     = 20f;
    private Rigidbody2D      rb;
    private CircleCollider2D circleCollider2D;
    private Collider2D[]     results = new Collider2D[25];
    //疲劳值设置
    private float minTired = 0.0f;
    public float MinTired
    {
        get { return minTired; }
    }
    private float maxTired = 10.0f;
    public float MaxTired
    {
        get { return maxTired; }
    }
    private float currentTired;
    public float CurrentTired
    {
        get { return currentTired; }
    }
    private float TiredIndex;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        StartCoroutine(StopTopDownController2D());
    }

    private IEnumerator StopTopDownController2D()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            var topDownController2D = GetComponent<TopDownController2D>();
            if (topDownController2D != null && topDownController2D.enabled)
            {
                topDownController2D.enabled = false;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateChange();
        if (Input.GetKey(KeyCode.Space) && PlayerState == State.OnGround)
        {
            var velocity = rb.velocity;
            velocity.y = JumpVelocity;
            rb.velocity = velocity;
        }

        var force = Vector2.zero;
        if (PlayerState == State.OnGround)
        {
            force += new Vector2(Input.GetAxis("Horizontal"), 0) * MoveForce;
        }
        else
        {
            force += new Vector2(Input.GetAxis("Horizontal"), 0) * (MoveForce * 0.1f);
        }

        force = Vector2.ClampMagnitude(force, MaxForce);
        rb.AddForce(force);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }

    private void StateChange()
    {
        var castCount = Physics2D.OverlapCircleNonAlloc((Vector2)transform.position + circleCollider2D.offset + Vector2.down * 0.1f, circleCollider2D.radius, results);
        for (int i = 0; i < castCount; i++)
        {
            var result = results[i];
            if (result == circleCollider2D)
            {
                continue;
            }

            if (result.CompareTag("Mine"))
            {
                PlayerState = State.OnGround;
                return;
            }
        }

        PlayerState = State.OnAir;
    }
    //计算体力值，根据当前的速度来判断角色时走还是跑还是站立不动
    // void CalculateTired()
    // {
    //     if (currentSpeed <= walkSpeed && currentTired >= minTired)
    //     {
    //         //角色站立不动，疲劳值每秒-1
    //         currentTired -= Time.deltaTime;
    //     }
    //     else if (currentSpeed >= runSpeed && currentTired <= maxTired)
    //     {
    //         //角色跑动，疲劳值+1
    //         currentTired += Time.deltaTime;
    //     }
    //     else
    //     {
    //         //角色走动，疲劳值不变
    //         return;
    //     }
    //     //将疲劳值限制在最大最小之间
    //     currentTired = Mathf.Clamp(currentTired, minTired, maxTired);
    // }
}