using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitFllow : MonoBehaviour
{
    public Transform B;   //A要跟随的B
    public float smoothTime = 0.01f;  //A平滑移动的时间
    private Vector3 AVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, B.position + new Vector3(0, 0, 0), ref AVelocity, smoothTime);
    }
}
