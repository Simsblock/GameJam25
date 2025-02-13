using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct SerializableDictionary<T0,T1>
{
    private Dictionary<T0, T1> Dictionary;
    
    [SerializeField]
    public T0[] Keys;
    [SerializeField]
    public T1[] Values;

    //Methods
    public IReadOnlyDictionary<T0,T1> GetValue()
    {
        if (Keys.Length != Values.Length) throw new ArgumentException($"Key amount ({Keys.Length}) is not equal to Value ({Values.Length}) amount");
        else
        {
            for (int i = 0; i < Keys.Length; i++)
            {
                Dictionary.Add(Keys[i], Values[i]);
            }
        }
        return Dictionary;
    }
}