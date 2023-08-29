using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayerMove : MonoBehaviour
{
    public  float       forcemode      = 1f;
    public  float       forceMagnitude = 10f;
    public  float       speed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * forcemode, ForceMode2D.Impulse);
            rb.AddForce(Vector2.up * forceMagnitude);
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * speed, 0);
        rb.AddForce(movement);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(3);
    }
}