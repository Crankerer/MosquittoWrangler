using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterController : MonoBehaviour
{    
    [SerializeField]
    private FilterManager filterManager;
    
    [SerializeField]
    private TextMeshProUGUI topicLabel;
    
    [SerializeField]
    private Toggle toggle;

    public void UpdateTopic(string topic)
    {
        topicLabel.SetText(topic);
    }

    public void ToggleFilter()
    {
        filterManager.UpdateFilter(topicLabel.text, toggle.isOn);
    }

    public void SetFilterManager(FilterManager manager)
    {
        filterManager = manager;
    }

    public string GetTopic()
    {
        return topicLabel.text;
    }
}
