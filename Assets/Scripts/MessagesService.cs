using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessagesService", menuName = "Services/MessagesService")]
public class MessagesService : ScriptableObject
{
    public event Action<RowData> NewMessage;
    public event Action<RowData> SendMqttMsg;
    public event Action<RowData> NewDetailMessage;

    [SerializeField]
    private List<RowData> messageList = new List<RowData>();
    
    [SerializeField]
    private int maxMessageInStorage = 1000;

    public void DistributeNewMessage(string sender, string topic, string value)
    {
        RowData data = new RowData("","","","");

        data.time = DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString("000");
        data.topic = topic;
        data.sender = sender;
        data.content = value;
        
        if(messageList.Count > maxMessageInStorage)
            messageList.Clear();
        
        messageList.Add(data);
        NewMessage?.Invoke(data);
    }

    public void SendMqttMessage(string topic, string value)
    {
        RowData row = new RowData("","","","");
        row.topic = topic;
        row.sender = "Mosquitto Wrangler";
        row.content = value;
        
        SendMqttMsg?.Invoke(row);
    }

    public void DistributeAllMessagesAgain()
    {
        foreach (RowData data in messageList)
        {
            NewMessage?.Invoke(data);
        }
    }

    public void ToggleDetailMessageView(RowData data)
    {
        NewDetailMessage?.Invoke(data);
    }
}
