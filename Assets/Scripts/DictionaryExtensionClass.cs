using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtensionClass
{
    public static Tvalue TryGetValueOrDefault<Tvalue, Tkey>(this Dictionary<Tkey, Tvalue> dictionary, Tkey key)
    {
        if (dictionary.TryGetValue(key, out Tvalue value))
        {
            return value;
        }

        return default;
    }
}
