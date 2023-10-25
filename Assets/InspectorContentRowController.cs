using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InspectorContentRowController : MonoBehaviour
{
    public event Action<bool> PauseUpdate;
    
    [SerializeField]
    private FilterManager filterManager;
    
    [SerializeField]
    private MessagesService messagesService;
    
    [SerializeField]
    private GameObject rowPrefab;

    [SerializeField]
    private Scrollbar scrollbar;

    [SerializeField]
    private Color color1;
    
    [SerializeField]
    private Color color2;

    [SerializeField]
    private int maxRows = 1000;

    private Dictionary<string, RowController> _inspectorDict = new Dictionary<string, RowController>();

    private List<string> _topicsBlacklist = new List<string>();
    
    private float _colorSwitch;

    private RectTransform _rectTransform;
    private float _height;
    private bool _pause;

    private void Awake()
    {
        _rectTransform = GetComponent(typeof(RectTransform)) as RectTransform;
        _height = ((RectTransform)rowPrefab.transform).sizeDelta.y;
        
        messagesService.NewMessage += OnNewMessage;
        messagesService.DistributeAllMessagesAgain();
    }

    private void OnNewMessage(RowData data)
    {
        if(_pause)
            return;
        
        if(IsTopicOnBlackList(data.topic))
            return;

        _colorSwitch = 1 - _colorSwitch;
        
        filterManager.UpdateFilter(data.topic);

        AddNewOrUpdate(data);
    }

    private void AddNewOrUpdate(RowData data)
    {
        RowController row;

        if (_inspectorDict.TryGetValue(data.topic, out row))
        {
            row.SetData(data);
            row.TriggerUpdateVisuals();
        }
        else if (_inspectorDict.Count < maxRows)
        {
            row = Instantiate(rowPrefab, transform).GetComponent<RowController>();

            row.SetData(data);
            row.SetColor(Color.Lerp(color1, color2, _colorSwitch));
            row.TriggerUpdateVisuals();
            
            _inspectorDict.TryAdd(data.topic, row);
        }

        UpdateScrollbar();
        UpdateContentHeight();
    }

    private void UpdateScrollbar()
    {
        scrollbar.value = 0;
    }

    private void UpdateContentHeight()
    {
        _height = _inspectorDict.Count * ((RectTransform)rowPrefab.transform).sizeDelta.y;
        _rectTransform.sizeDelta = new Vector2(0, _height);
    }

    public void Resume()
    {
        _pause = false;
        PauseUpdate?.Invoke(_pause);
    }
    
    public void Pause()
    {
        _pause = true;
        PauseUpdate?.Invoke(_pause);
    }

    public void TogglePauseResume()
    {
        _pause = !_pause;
        PauseUpdate?.Invoke(_pause);
    }

    public void ClearAllRows()
    {
        foreach (KeyValuePair<string, RowController> row in _inspectorDict)
        {
            Destroy(row.Value.gameObject);
        }
        _inspectorDict.Clear();
    }

    public void AddTopicToBlacklist(string topic)
    {
        _topicsBlacklist.Add(topic);
    }
    
    public void RemoveTopicFromBlacklist(string whitetopic)
    {
        int index = _topicsBlacklist.IndexOf(whitetopic);
        _topicsBlacklist.RemoveAt(index);
    }

    private bool IsTopicOnBlackList(string newTopic)
    {          
        foreach (string topic in _topicsBlacklist)
        {
            if (topic == newTopic)
                return true;
        }
        
        return false;
    }

    public void UpdateRows()
    {
        RowController row;
        foreach (string topic in _topicsBlacklist)
        {
            if (_inspectorDict.TryGetValue(topic, out row))
            {
                Destroy(row.gameObject);
                _inspectorDict.Remove(topic);   
            }
        }

        UpdateScrollbar();
        UpdateContentHeight();
    }
}
