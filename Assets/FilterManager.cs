using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterManager : MonoBehaviour
{   
    [SerializeField]
    private WranglerContentRowController contentRowController;
    
    [SerializeField]
    private InspectorContentRowController inspectorRowController;
    
    [SerializeField]
    private GameObject filterPrefab;

    private List<FilterController> _filterControllers = new List<FilterController>();

    public void UpdateFilter(string topic)
    {
        if(CheckTopicAlreadyExist(topic))
            return;
        
        GameObject filter = Instantiate(filterPrefab, transform);

        FilterController filterController = filter.GetComponent<FilterController>();
        filterController.SetFilterManager(this);
        filterController.UpdateTopic(topic);
        
        _filterControllers.Add(filterController);
    }

    public void UpdateFilter(string topic, bool isOn)
    {
        if(isOn)
            inspectorRowController?.RemoveTopicFromBlacklist(topic);
        else
            inspectorRowController?.AddTopicToBlacklist(topic);
        
        inspectorRowController?.UpdateRows();
        

        if(isOn)
            contentRowController?.RemoveTopicFromBlacklist(topic);
        else
            contentRowController?.AddTopicToBlacklist(topic);
        
        contentRowController?.UpdateRows();
    }

    private bool CheckTopicAlreadyExist(string newTopic)
    {
        foreach (FilterController filterController in _filterControllers)
        {
            if (filterController.GetTopic() == newTopic)
                return true;
        }
        return false;
    }
}
