using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerSE() 
    { 
    audioSource.Play();
    }
}
