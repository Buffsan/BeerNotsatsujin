using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUDP : MonoBehaviour
{

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UDP_Move() 
    {
        rb.velocity = Vector2.right * 3;
    }


}
