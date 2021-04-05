using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMessageUI : MonoBehaviour
{
    [SerializeField]
    Text deathMessageText;

    [SerializeField]
    Text pilotStatusText;

    public Text DeathMessageText
    {
        get
        {
            return deathMessageText;
        }
    }

    public Text PilotStatusText
    {
        get
        {
            return pilotStatusText;
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
