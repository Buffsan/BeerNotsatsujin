using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour
{

    AudioManager manager => AudioManager.instance;
    [SerializeField] AudioClip Clip;

    public GameObject BeerObject;
    public GameObject BubbleObject;
    [SerializeField] GameObject BeerPoint;

    // Start is called before the first frame update
    void Start()
    {
        
        BubbleObject.transform.position = BeerPoint.transform.position;

    }

    // Update is called once per frame
   
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        manager.isPlaySE(Clip);
    }
}
