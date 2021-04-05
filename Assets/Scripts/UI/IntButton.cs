using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntButton : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    [SerializeField]
    int index;

    public delegate void CallBackFunction(int index);
    public CallBackFunction callBackFunction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(EncyclopediaDefinition encyclopediaDefinition, int indexValue, CallBackFunction function)
    {
        index = indexValue;

        nameText.text = encyclopediaDefinition.GetDisplayName();

        callBackFunction = function;
    }

    public void ClickButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        callBackFunction(index);
    }
}
