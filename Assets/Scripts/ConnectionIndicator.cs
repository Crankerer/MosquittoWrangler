using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ConnectionIndicator : MonoBehaviour
{
    [SerializeField]
    private ConnectionService connectionService;
    
    [SerializeField]
    private Color connectedColor = Color.green;
    
    [SerializeField]
    private Color disconnectedColor = Color.red;
    
    [SerializeField]
    private Color tryConnectingColor = Color.yellow;
    
    [SerializeField]
    private Sprite connectedSprite;
    
    [SerializeField]
    private Sprite tryConnectingSprite;
    
    [SerializeField]
    private Sprite disconnectedSprite;

    private Image _icon;
    
    void Start()
    {
        _icon = GetComponent<Image>();
        connectionService.ConnectionEstablished += OnConnected;
        connectionService.TryConnecting += OnTryConnecting;
        connectionService.ConnectionEnded += OnDisconnected;
    }

    private void OnTryConnecting()
    {
        _icon.sprite = tryConnectingSprite;
        _icon.color = tryConnectingColor;
    }

    private void OnDisconnected()
    {
        _icon.sprite = disconnectedSprite;
        _icon.color = disconnectedColor;
    }

    private void OnConnected()
    {
        _icon.sprite = connectedSprite;
        _icon.color = connectedColor;
    }
}
