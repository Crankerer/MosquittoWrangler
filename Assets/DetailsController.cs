
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetailsController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private MessagesService messagesService;

    [SerializeField]
    private RowController rowController;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            messagesService.ToggleDetailMessageView(new RowData("", rowController.GetSender(), rowController.GetTopic(),rowController.GetPayload()));
            Debug.Log("Right click");
        }
    }
}
