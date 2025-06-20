using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackController_Systems : MonoBehaviour
{
    [SerializeField] float SpawnTime = 0;
    [SerializeField] float SpawnCount = 0;
    [SerializeField] GameObject Clover;

    public List<GameObject> BackNpcs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnCount = SpawnTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (SpawnTime < SpawnCount)
        {
            int RandomNPCint = Random.Range(0, BackNpcs.Count);

            SpawnCount = 0;
            Instantiate(BackNpcs[RandomNPCint]);
        }
        else 
        { 
        SpawnCount += Time.deltaTime;
        }
    }
}
