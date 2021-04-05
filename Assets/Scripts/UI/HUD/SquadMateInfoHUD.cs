using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadMateInfoHUD : MonoBehaviour
{
    [SerializeField]
    Text pilotText;

    [SerializeField]
    Text mechText;

    [SerializeField]
    Text orderText;

    [SerializeField]
    Image highLightImage;

    public Text PilotText
    {
        get
        {
            return pilotText;
        }
    }

    public Text MechText
    {
        get
        {
            return mechText;
        }
    }

    public Text OrderText
    {
        get
        {
            return orderText;
        }
    }

    public Image HighLightImage
    {
        get
        {
            return highLightImage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
