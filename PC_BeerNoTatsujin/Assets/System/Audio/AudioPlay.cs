using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    [SerializeField] AudioSource Asource;
    void Update()
    {
        if (Asource != null)
        {
            if (!Asource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        else { Debug.Log("‰¹‚ª‚Ë‚¥"); }
    }
    public void isCL_PlaySE(AudioClip Clip)
    {
        //Debug.Log(Clip);
        Asource.clip = Clip;
        Asource.Play();
    }
}
