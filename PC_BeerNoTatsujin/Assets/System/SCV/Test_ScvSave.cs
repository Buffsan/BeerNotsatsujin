using System.IO;
using System.Text;
using UnityEngine;

public class Test_ScvSave : MonoBehaviour
{
    private float time;
    private StreamWriter sw;

    SCV SampleSaveCsvScript;

    void Start()
    {
        SampleSaveCsvScript = GetComponent<SCV>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            SampleSaveCsvScript.SaveData("F", " ", time.ToString());
        }
    }
}
