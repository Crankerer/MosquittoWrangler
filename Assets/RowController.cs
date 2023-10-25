using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RowController : MonoBehaviour
{    
    [SerializeField]
    private Image highlightImage;
    
    [SerializeField]
    private float timeNew = 0.1f;
    
    [SerializeField]
    private TextMeshProUGUI time;
    
    [SerializeField]
    private TextMeshProUGUI sender;

    [SerializeField]
    private TextMeshProUGUI topic;

    [SerializeField]
    private TextMeshProUGUI content;
    
    [SerializeField]
    private Color _newUpdateColor = Color.red;
    
    [SerializeField]
    private Color _defaultColor = Color.white;
    
    private float _timePassed = 0;

    private void Awake()
    {
        _timePassed = timeNew;
    }

    public void SetData(RowData data)
    {
        time.SetText(data.time);
        sender.SetText(data.sender);
        topic.SetText(data.topic);
        content.SetText(data.content);
    }

    public string GetTopic()
    {
        return topic.text;
    }
    
    public string GetSender()
    {
        return sender.text;
    }
    
    public string GetPayload()
    {
        return content.text;
    }

    public void TriggerUpdateVisuals()
    {
        highlightImage.color = _newUpdateColor;     
        _timePassed = 0;
    }

    private void FixedUpdate()
    {
        if (_timePassed > timeNew)
            return;

        highlightImage.color = Color.Lerp(_newUpdateColor, _defaultColor, (_timePassed / timeNew));

        _timePassed += Time.deltaTime;
    }

    public void SetColor(Color color)
    {
        highlightImage.color = color;
        _defaultColor = color;
    }
}
