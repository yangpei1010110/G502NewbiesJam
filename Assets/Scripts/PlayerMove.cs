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
}