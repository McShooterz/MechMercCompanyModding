using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechDesignInformationUI : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    [SerializeField]
    Text designDetailsText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDesign(MechDesign mechDesign)
    {
        if (mechDesign != null)
        {
            nameText.text = mechDesign.DesignName;

            designDetailsText.text = mechDesign.GetDisplayInformation();
        }
        else
        {
            nameText.text = "";

            designDetailsText.text = "";
        }
    }
}
