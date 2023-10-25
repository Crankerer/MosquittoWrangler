using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailMessageViewHandler : MonoBehaviour
{
    [SerializeField]
    private MessagesService messagesService;

    [SerializeField]
    private TMP_InputField topic;
    
    [SerializeField]
    private TMP_InputField payload;

    [SerializeField]
    private Transform windowTransform;
    
    void Awake()
    {
        windowTransform.gameObject.SetActive(false);
        messagesService.NewDetailMessage += OnNewDetailMessage;
    }

    private void OnNewDetailMessage(RowData obj)
    {
        windowTransform.gameObject.SetActive(true);
        topic.text = obj.topic;
        payload.text = obj.content;
    }
}
