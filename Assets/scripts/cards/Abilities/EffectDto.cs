using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class EffectDto : MonoBehaviour
{
    public string effect;
    public bool preStand;
    public bool Used;
    public int Price;
    public TMP_Text text;
    public string name;
    public bool isDice, permanent,DynamicHover;
}
