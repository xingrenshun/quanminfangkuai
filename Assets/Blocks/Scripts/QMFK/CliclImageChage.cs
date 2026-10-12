using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CliclImageChage : MonoBehaviour
{
    public Sprite On;
    public Sprite Off;
    public bool IsOn
    {
        get;set;
    }
    public void ChangeImage(bool isON,Action action=null)
    {
        IsOn = isON;
        if (isON)
        {
            this.GetComponent<Image>().sprite = On;
        }
        else
        {
            this.GetComponent<Image>().sprite = Off;
        }
        action?.Invoke();
    }
    public void ClickButton(Action action=null)
    {
        IsOn = !IsOn;
        ChangeImage(IsOn,action);
    }
}
