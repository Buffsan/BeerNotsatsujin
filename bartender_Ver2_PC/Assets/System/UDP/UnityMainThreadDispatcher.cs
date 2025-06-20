using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> executionQueue = new Queue<Action>();

    private static UnityMainThreadDispatcher instance;
    
    public static UnityMainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject("UnityMainThreadDispatcher");
            instance = obj.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(obj); // シーン切り替えでも削除されないようにする
        }
        return instance;
    }

    void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                var action = executionQueue.Dequeue();
                action.Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }
}