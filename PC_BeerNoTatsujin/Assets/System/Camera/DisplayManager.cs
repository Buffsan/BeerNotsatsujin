using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    void Start()
    {
        // 利用できるディスプレイの数を表示
        Debug.Log("Displays connected: " + Display.displays.Length);

        // 1番目以降のディスプレイを有効化
        // （Display.displays[0] はメインなので起動時から有効）
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}
