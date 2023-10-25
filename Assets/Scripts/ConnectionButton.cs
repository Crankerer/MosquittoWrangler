
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ConnectionButton : MonoBehaviour
{
    [SerializeField]
    private ConnectionService connectionService;
    
    [SerializeField]
    private string connectedLabel;
    
    [SerializeField]
    private string tryConnectingLabel;
    
    [SerializeField]
    private string disconnectedLabel;

    private TextMeshProUGUI _label;
    
    void Start()
    {
        _label = GetComponent<TextMeshProUGUI>();
        connectionService.ConnectionEstablished += OnConnected;
        connectionService.TryConnecting += OnTryConnecting;
        connectionService.ConnectionEnded += OnDisconnected;
    }

    private void OnTryConnecting()
    {
        _label.SetText(tryConnectingLabel);
    }

    private void OnDisconnected()
    {
        _label.SetText(connectedLabel);
    }

    private void OnConnected()
    {
        _label.SetText(disconnectedLabel);
    }
}
