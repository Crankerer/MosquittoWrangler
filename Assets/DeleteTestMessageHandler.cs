using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteTestMessageHandler : MonoBehaviour
{
    [SerializeField]
    private TesterService testerService;
    
    [SerializeField]
    private TextMeshProUGUI topic;

    public void DeleteTestMessageWithTopic()
    {
        testerService.DeleteMessage(topic.text);
    }
}
