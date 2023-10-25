using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConnectionService", menuName = "Services/ConnectionService")]
public class ConnectionService : ScriptableObject
{
    public event Action ConnectionEstablished;
    public event Action TryConnecting;
    public event Action ConnectionEnded;

    public event Action<List<string>> IpAddressesUpdate; 

    [SerializeField]
    private List<string> ipAddresses = new List<string>();
    
    [SerializeField]
    private bool _connected;
    private string _currentConnectionAddress;

    private void OnEnable()
    {
        LoadConfig();
    }

    public bool IsConnected()
    {
        return _connected;
    }

    public string GetCurrentIp()
    {
        return _currentConnectionAddress;
    }

    public void Connected()
    {
        _connected = true;
        
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                ConnectionEstablished?.Invoke()
        );
    }
    
    public void Connecting()
    {        
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
            TryConnecting?.Invoke()
        );
    }

    public void Disconnected()
    {
        _connected = false;
        
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
            ConnectionEnded?.Invoke()
        );
    }

    public void AddIpAddress(string ip)
    {
        ipAddresses.Add(ip);
        SaveConfig();
        IpAddressesUpdate?.Invoke(ipAddresses);
    }

    public List<string> GetIpAddresses()
    {
        return ipAddresses;
    }

    public void SetCurrentIpAddress(string ip)
    {
        _currentConnectionAddress = ip;
    }

    public bool IpAlreadyAdded(string newIp)
    {
        foreach (string ip in ipAddresses)
        {
            if (string.Equals(newIp, ip))
                return true;
        }
        
        return false;
    }

    private void SaveConfig()
    {
        ConfigElement config = ConfigManager.Config;
        config.ips = ipAddresses;
        ConfigManager.SaveConfig();
    }

    private void LoadConfig()
    {
        ConfigManager.LoadConfig();
        ConfigElement config = ConfigManager.Config;
        ipAddresses = config.ips;
        IpAddressesUpdate?.Invoke(ipAddresses);
    }
}
