using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolController : MonoBehaviour
{


    public float DellTime = 4;
    float DellCount = 0;
    bool Dell = false;

    public Animator animator;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        DellCount += Time.deltaTime;
        if (DellTime < DellCount) 
        {

            Destroy(gameObject);
            /*
            animator.Play("Á‚¦‚é•]‰¿");
            if (!Dell)
            {
                Destroy(gameObject,2);
                Dell = true;
            }*/
        
        }
    }
}
