using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendTestMessageHandler : MonoBehaviour
{
    [SerializeField]
    private MessagesService messagesService;

    [SerializeField]
    private TextMeshProUGUI topic;
    
    [SerializeField]
    private TextMeshProUGUI payload; 
    
    public void SendMessage()
    {
        messagesService.SendMqttMessage(topic.text, payload.text);
    }
}
