using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechChassisInformationUI : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    [SerializeField]
    Text classText;

    [SerializeField]
    Text tonnageText;

    [SerializeField]
    Text armorMaxText;

    [SerializeField]
    Text internalText;

    [SerializeField]
    Text jumpCapableText;

    [SerializeField]
    Text descriptionText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInformation(MechChassisDefinition mechChassisDefinition)
    {
        nameText.text = mechChassisDefinition.GetDisplayName();
        classText.text = "Class: " + mechChassisDefinition.UnitClassDisplay.ToUpper();
        tonnageText.text = "Tonnage: " + mechChassisDefinition.Tonnage.ToString("0.#");
        armorMaxText.text = "Max Armor:" + mechChassisDefinition.MaxArmor.ToString("0.#");
        internalText.text = "Internals: " + mechChassisDefinition.InternalsTotal.ToString("0.#");

        bool jumpCapable = mechChassisDefinition.IsJumpCapable();

        if (jumpCapable)
        {
            jumpCapableText.text = "Jump Capable: True";
        }
        else
        {
            jumpCapableText.text = "Jump Capable: False";
        }

        descriptionText.text = mechChassisDefinition.GetDescription();
    }
}
