using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackNPC_Controller : MonoBehaviour
{
    public enum Look 
    { 
    
        Right,
        Left,
    
    }
    public Look look = Look.Right;
    [SerializeField] float Speed = 1;

    [SerializeField] float Ypos = 0;

    // Start is called before the first frame update
    void Start()
    {
        int Randomint = Random.Range(0,2);
        if (Randomint == 0)
        {
            look = Look.Right;
        }
        else 
        {
            look = Look.Left;
        }

        if (look == Look.Right) 
        {

            transform.position = new Vector2(15, Ypos);
            transform.localScale = new Vector2 (4.5f,4.5f);
        
        }else if (look == Look.Left) 
        {
            transform.position = new Vector2(-15, Ypos);
            transform.localScale = new Vector2(-4.5f, 4.5f);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float MoveSpeed = Speed * Time.deltaTime;

        if (look == Look.Right)
        {

            transform.position = new Vector2(transform.position.x - MoveSpeed, Ypos);
            if (transform.position.x < -15) 
            {
                Destroy(gameObject);
            }

        }
        else if (look == Look.Left)
        {
            transform.position = new Vector2(transform.position.x + MoveSpeed, Ypos);
            transform.localScale = new Vector2(-4.5f, 4.5f);
            if (transform.position.x > 15)
            {
                Destroy(gameObject);
            }
        }
    }
}
