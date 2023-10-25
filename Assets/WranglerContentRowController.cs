using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WranglerContentRowController : MonoBehaviour
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

    private Queue<RowController> _currentRows = new Queue<RowController>();
    private Queue<RowData> _currentRowsData = new Queue<RowData>();

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
        //_currentRowsData.Enqueue(data);

        if(_pause)
            return;
        
        if(IsTopicOnBlackList(data.topic))
            return;

        _colorSwitch = 1 - _colorSwitch;
        
        filterManager.UpdateFilter(data.topic);

        if (_currentRows.Count > maxRows)
            UpdateRow(data);
        else
            AddNewRow(data);
    }

    private void AddNewRow(RowData data)
    {
        _currentRows.Enqueue(Instantiate(rowPrefab, transform).GetComponent<RowController>());
        _currentRows.Last().SetColor(Color.Lerp(color1, color2, _colorSwitch));
        _currentRows.Last().SetData(data);
        
        UpdateScrollbar();
        UpdateContentHeight();
    }

    private void UpdateRow(RowData data)
    {
        RowController rowRowController = _currentRows.Dequeue();
        rowRowController.transform.SetAsLastSibling();
        rowRowController.GetComponent<Image>().color = Color.Lerp(color1, color2, _colorSwitch);
        rowRowController.SetData(data);
        _currentRows.Enqueue(rowRowController);
    }

    private void UpdateScrollbar()
    {
        scrollbar.value = 0;
    }

    private void UpdateContentHeight()
    {
        _height = _currentRows.Count * ((RectTransform)rowPrefab.transform).sizeDelta.y;
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
        foreach (RowController row in _currentRows)
        {
            Destroy(row.gameObject);
        }
        _currentRows.Clear();
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
        foreach (RowController row in _currentRows)
        {
            foreach (string topic in _topicsBlacklist)
            {
                if (row.GetTopic() == topic)
                {
                    _currentRows = new Queue<RowController>(_currentRows.Where(x => x != row));
                    Destroy(row.gameObject);
                }
            }
        }

        UpdateScrollbar();
        UpdateContentHeight();
    }
}
