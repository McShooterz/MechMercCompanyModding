using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeTextUI : MonoBehaviour
{
    [SerializeField]
    Text componentText;

    [SerializeField]
    string localKey;

    // Start is called before the first frame update
    void Start()
    {
        componentText.text = ResourceManager.Instance.GetLocalization(localKey);

        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
