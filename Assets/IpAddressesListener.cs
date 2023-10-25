using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class IpAddressesListener : MonoBehaviour
{
    [SerializeField]
    private ConnectionService connectionService;
    
    [SerializeField]
    private TextMeshProUGUI ipAddressesField;
    
    [SerializeField]
    private TMP_InputField newIpAddressesInputField;
    
    void Start()
    {
        OnIpAddressesUpdate(connectionService.GetIpAddresses());
        connectionService.IpAddressesUpdate += OnIpAddressesUpdate;
    }

    private void OnIpAddressesUpdate(List<string> ipAddresses)
    {
        ipAddressesField.SetText("");

        string ipList = "";
        
        foreach (var ip in ipAddresses)
            ipList += ip + "\n";

        ipAddressesField.SetText(ipList);
    }

    public void AddIpAddress()
    {
        if(!ValidateIpAddress(newIpAddressesInputField.text))
            return;
        
        connectionService.AddIpAddress(newIpAddressesInputField.text);
    }

    private bool ValidateIpAddress(string ip)
    {
        IPAddress address;
        return IPAddress.TryParse(ip, out address);
    }
}
