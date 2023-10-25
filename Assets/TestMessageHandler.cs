using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestMessageHandler : MonoBehaviour
{
    [SerializeField]
    private TesterService testerService;

    [SerializeField]
    private TMP_InputField topicField;
    
    [SerializeField]
    private TMP_InputField payloadField; 
    
    public void AddNewTestMessage()
    {
        RowData row = new RowData("","","","");

        row.time = DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString("000");
        row.topic = topicField.text;
        row.content = payloadField.text;
        
        testerService.AddTestMessage(row);
    }
}
