using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackController_Systems : MonoBehaviour
{
    [SerializeField] float SpawnTime = 0;
    [SerializeField] float SpawnCount = 0;
    [SerializeField] GameObject Clover;
    [SerializeField] float BasePosX = 0;

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
            GameObject CL_NPC = Instantiate(BackNpcs[RandomNPCint]);
            BackNPC_Controller backNPC = CL_NPC.GetComponent<BackNPC_Controller>();
            backNPC.BasePosX = BasePosX;
        }
        else 
        { 
        SpawnCount += Time.deltaTime;
        }
    }
}
