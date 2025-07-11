using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectLimit : MonoBehaviour
{
    // Start is called before the first frame update
    float LimitCount = 0;
    public float LimitTime = 1;
    [SerializeField] ParticleSystem particleSystem;
    
    private void FixedUpdate()
    {
        LimitCount += Time.deltaTime;
        if (LimitTime < LimitCount)
        {
            particleSystem.emissionRate = 0;
        }
    }
}
