using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TesterContentRowController : MonoBehaviour
{
    [SerializeField]
    private TesterService testerService;
    
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

    private Dictionary<string, RowController> _testerDict = new Dictionary<string, RowController>();

    private float _colorSwitch;

    private RectTransform _rectTransform;
    private float _height;

    private void Start()
    {
        _rectTransform = GetComponent(typeof(RectTransform)) as RectTransform;
        _height = ((RectTransform)rowPrefab.transform).sizeDelta.y;

        testerService.NewTestMessage += OnNewTestMessage;
        testerService.DeleteTestMessage += OnDeleteTestMessage;
    }

    private void OnDeleteTestMessage(string topic)
    {
        RemoveTestMessage(topic);
    }

    private void OnNewTestMessage(RowData data)
    {
        AddNewRow(data);
    }

    private void AddNewRow(RowData data)
    {
        _colorSwitch = 1 - _colorSwitch;

        AddNewOrUpdate(data);
    }

    private void AddNewOrUpdate(RowData data)
    {
        RowController row;
        
        if (_testerDict.Count < maxRows)
        {
            row = Instantiate(rowPrefab, transform).GetComponent<RowController>();

            row.SetData(data);
            row.SetColor(Color.Lerp(color1, color2, _colorSwitch));
            row.TriggerUpdateVisuals();
            
            _testerDict.TryAdd(data.topic, row);
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
        _height = _testerDict.Count * ((RectTransform)rowPrefab.transform).sizeDelta.y;
        _rectTransform.sizeDelta = new Vector2(0, _height);
    }

    public void ClearAllRows()
    {
        foreach (KeyValuePair<string, RowController> row in _testerDict)
        {
            Destroy(row.Value.gameObject);
        }
        _testerDict.Clear();
    }

    private void RemoveTestMessage(string topic)
    {
        RowController row;

        if (_testerDict.TryGetValue(topic, out row))
            Destroy(row.gameObject);

        _testerDict.Remove(topic);
    }
}
