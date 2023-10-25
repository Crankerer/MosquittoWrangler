using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseListener : MonoBehaviour
{
    [SerializeField]
    private WranglerContentRowController wranglerContentRowController;

    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Color pauseColor;
    
    [SerializeField]
    private Color defaultColor;

    void Start()
    {
        wranglerContentRowController.PauseUpdate += OnPauseUpdate;
    }

    private void OnPauseUpdate(bool value)
    {
        if(value)
            backgroundImage.color = pauseColor;
        else
            backgroundImage.color = defaultColor;
    }
}
