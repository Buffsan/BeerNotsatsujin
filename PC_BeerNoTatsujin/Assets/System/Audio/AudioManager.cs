using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] GameObject AudioPlayObj;
    public AudioClip BGM;
    public AudioSource BgmSource;
    AudioPlay audioPlay;
    void Start()
    {
        BgmSource = GetComponent<AudioSource>();
        if (BGM != null)
        {
            BgmSource.clip = BGM;
            BgmSource.Play();
        }

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBGM(AudioClip BGMs) 
    {
        if (BGM != BGMs)
        {
            BGM = BGMs;
            BgmSource.clip = BGMs;
            BgmSource.Play();
        }

    }

    public void isPlaySE(AudioClip Clip)
    {
        GameObject CL_AudioPlay = Instantiate(AudioPlayObj);

        AudioPlay audio = CL_AudioPlay.GetComponent<AudioPlay>();

        audio.isCL_PlaySE(Clip);



        //CL_AudioPlay.isStatic = true;
        CL_AudioPlay.SetActive(true);

    }
}