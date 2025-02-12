using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Highlight
{
 
    public static void SelectHighlightOn(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError("SelectHighlight On no Object passed");
            return;
        }
        Color c = go.GetComponent<Image>().color;
        if(c == null || c.a == 0.99)
        {
            if (c == null)
            {
                Debug.LogError("No colour component on SelectHighlight off");
            }
            return;
        }
        c.a = 0.99f;
        c *= 0.8f;
        go.GetComponent<Image>().color = c;
    }

    public static void SelectHighlightOff(GameObject go)
    {
        Debug.Log(go.name);
        if (go == null)
        {
            Debug.LogError("SelectHighlight off no Object passed");
            return;
        }
        Color c = go.GetComponent<Image>().color;
        if(c == null || c.a == 1)
        {
            if (c == null)
            {
                Debug.LogError("No colour component on SelectHighlight off");
            }
            return;
        }
        c = c * 1.25f;
        c.a = 1;
        go.GetComponent<Image>().color = c;
    }

    public static void FieldHighlightOn(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError("FieldHighlight on no Object passed");
            return;
        }
        Color c = go.GetComponent<Image>().color;
        if (c == null || c.a == 0.5)
        {
            if (c == null)
            {
                Debug.LogError("No colour component on FieldHighlight on");
            }
            return;
        }
        c.a = 0.5f;
        go.GetComponent <Image>().color = c;
    }

    public static void FieldHighlightOff(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError("FieldHighlight off no Object passed");
            return;
        }
        Color c = go.GetComponent<Image>().color;
        if (c == null || c.a == 0)
        {
            if (c == null)
            {
                Debug.LogError("No colour component on FieldHighlight off");
            }
            return;
        }
        c.a = 0f;
        go.GetComponent<Image>().color = c;
    }
}
