using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TesterService", menuName = "Services/TesterService")]
public class TesterService : ScriptableObject
{
    public event Action<RowData> NewTestMessage;
    public event Action<string> DeleteTestMessage;

    public void AddTestMessage(RowData data)
    {        
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
            NewTestMessage?.Invoke(data)
        );
    }

    public void DeleteMessage(string topic)
    {        
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
            DeleteTestMessage?.Invoke(topic)
        );
    }
}
