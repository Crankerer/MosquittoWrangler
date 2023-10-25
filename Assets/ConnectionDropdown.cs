using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class ConnectionDropdown : MonoBehaviour
{
    [SerializeField]
    private ConnectionService connectionService;

    [SerializeField]
    private TMP_InputField labelInputField;
    
    private TMP_Dropdown _dropdown;

    private void Start()
    {
        connectionService.IpAddressesUpdate += OnIpAddressesUpdate;
        OnIpAddressesUpdate(connectionService.GetIpAddresses());
    }

    private void OnIpAddressesUpdate(List<string> ipAddresses)
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.options.Clear();
        
        foreach (var ip in ipAddresses)
            _dropdown.options.Add(new TMP_Dropdown.OptionData(ip));

        UpdateIpAndLabel();
    }

    public void OnDropdownValueChanged()
    {
        UpdateIpAndLabel();
    }

    public void AddIpAddress()
    {
        if(!ValidateIpAddress(labelInputField.text))
            return;
        
        if(connectionService.IpAlreadyAdded(labelInputField.text))
            return;
        
        if(connectionService.IpAlreadyAdded(labelInputField.text))
            return;
        
        connectionService.AddIpAddress(labelInputField.text);
        connectionService.SetCurrentIpAddress(_dropdown.options[_dropdown.value + 1].text);
        labelInputField.text = _dropdown.options[_dropdown.value + 1].text;
    }
    
    private void UpdateIpAndLabel(string ip = "")
    {
        connectionService.SetCurrentIpAddress(_dropdown.options[_dropdown.value].text);
        labelInputField.text = _dropdown.options[_dropdown.value].text;
    }

    private bool ValidateIpAddress(string ip)
    {
        IPAddress address;
        return IPAddress.TryParse(ip, out address);
    }
}
