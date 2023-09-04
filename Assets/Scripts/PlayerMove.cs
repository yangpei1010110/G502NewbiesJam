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

    public  State       PlayerState  = State.OnGround;
    public  float       JumpVelocity = 20f;
    public  float       MoveForce    = 20f;
    public  float       MaxForce     = 20f;
    public  float       MaxSpeed     = 20f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (Input.GetKey(KeyCode.Space) && PlayerState == State.OnGround)
        {
            var velocity = rb.velocity;
            velocity.y = JumpVelocity;
            rb.velocity = velocity;
        }

        if (PlayerState == State.OnGround)
        {
            var force = Vector2.zero;
            force += new Vector2(Input.GetAxis("Horizontal"), 0) * MoveForce;
            force = Vector2.ClampMagnitude(force, MaxForce);
            rb.AddForce(force);
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Mine"))
        {
            PlayerState = State.OnGround;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Mine"))
        {
            PlayerState = State.OnAir;
        }
    }
}