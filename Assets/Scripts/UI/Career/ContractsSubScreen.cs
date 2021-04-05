using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractsSubScreen : MonoBehaviour
{
    [SerializeField]
    ContractListUI contractListUI;

    [SerializeField]
    FactionLogoUI employerFactionLogoUI;

    [SerializeField]
    Camera planetPreviewCamera;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Text employerNameText;

    [SerializeField]
    Text enemyNameText;

    [SerializeField]
    Text reputationText;

    [SerializeField]
    Text infamyText;

    [SerializeField]
    Text planetText;

    [SerializeField]
    Text difficultyText;

    [SerializeField]
    Text missionCountText;

    [SerializeField]
    Text durationText;

    [SerializeField]
    Text datesText;

    [SerializeField]
    Text travelCoverageText;

    [SerializeField]
    Text bountyText;

    [SerializeField]
    Text missionPayText;

    [SerializeField]
    Text contractPayText;

    [SerializeField]
    Button acceptButton;

    [SerializeField]
    Button cancelButton;

    [SerializeField]
    ContractData selectedContract;

    [SerializeField]
    GameObject planetPreview = null;

    void OnEnable()
    {
        contractListUI.BuildList(GlobalDataManager.Instance.currentCareer.AvailableContracts);
    }

    void Update()
    {
        if (planetPreview != null)
        {
            planetPreview.transform.RotateAround(planetPreview.transform.position, Vector3.up, 50f * Time.unscaledDeltaTime);
        }
    }

    public void SelectIndex(int index)
    {
        Career career = GlobalDataManager.Instance.currentCareer;

        selectedContract = career.AvailableContracts[index];

        ContractDefinition contractDefinition = selectedContract.ContractDefinition;

        employerFactionLogoUI.SetFactionLogo(selectedContract.EmployerDefinition.FactionLogo);

        nameText.text = selectedContract.ContractDefinition.GetDisplayName();
        descriptionText.text = selectedContract.ContractDefinition.GetDescription();

        employerNameText.text = selectedContract.EmployerDefinition.GetDisplayName();

        enemyNameText.text = selectedContract.EnemyDefinition.GetDisplayName();

        reputationText.text = career.reputation.ToString() + " / " + contractDefinition.ReputationRequired.ToString();

        if (career.reputation < contractDefinition.ReputationRequired)
        {
            reputationText.color = Color.red;
        }
        else
        {
            reputationText.color = Color.white;
        }

        infamyText.text = career.infamy.ToString() + " / " + contractDefinition.InfamyRequired.ToString();

        if (career.infamy < contractDefinition.InfamyRequired)
        {
            infamyText.color = Color.red;
        }
        else
        {
            infamyText.color = Color.white;
        }

        planetText.text = selectedContract.PlanetDefinition.GetDisplayName();

        difficultyText.text = (selectedContract.DifficultyEstimated + 1).ToString();

        durationText.text = selectedContract.Duration.ToString() + " days";

        datesText.text = selectedContract.StartDate.Display + " - " + selectedContract.EndDate.Display;

        missionCountText.text = selectedContract.MissionSetups.Length.ToString();

        travelCoverageText.text = StaticHelper.FormatMoney(selectedContract.TravelCoverage);

        bountyText.text = StaticHelper.FormatMoney(Mathf.CeilToInt(selectedContract.BountyKillPay * career.bountyPayModifier));

        missionPayText.text = StaticHelper.FormatMoney(Mathf.CeilToInt(selectedContract.MissionPay * career.missionPayModifier));

        contractPayText.text = StaticHelper.FormatMoney(Mathf.CeilToInt(selectedContract.ContractPayPotential * career.contractPayModifier));

        if (career.reputation < contractDefinition.ReputationRequired)
        {
            acceptButton.interactable = false;
            cancelButton.interactable = false;
        }
        else if (selectedContract == GlobalDataManager.Instance.currentCareer.currentContract)
        {
            acceptButton.interactable = false;
            cancelButton.interactable = true;
        }
        else
        {
            acceptButton.interactable = true;
            cancelButton.interactable = false;
        }

        if (planetPreview != null)
        {
            Destroy(planetPreview);
        }

        planetPreview = selectedContract.PlanetDefinition.GetPrefab();

        if (planetPreview)
        {
            planetPreview = Instantiate(planetPreview, planetPreviewCamera.transform);

            //planetPreview.transform.localScale = new Vector3(selectedContract.PlanetDefinition.Scale, selectedContract.PlanetDefinition.Scale, selectedContract.PlanetDefinition.Scale);

            planetPreview.transform.localPosition = new Vector3(0f, 0f, 1.1f);
        }
    }

    public void ClickAcceptButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        GlobalDataManager.Instance.currentCareer.currentContract = selectedContract;

        acceptButton.interactable = false;
        cancelButton.interactable = true;

        contractListUI.RefreshList(GlobalDataManager.Instance.currentCareer.AvailableContracts);
    }

    public void ClickCancelButton()
    {
        AudioManager.Instance.PlayButtonClick(0);

        GlobalDataManager.Instance.currentCareer.currentContract = null;

        acceptButton.interactable = true;
        cancelButton.interactable = false;

        contractListUI.RefreshList(GlobalDataManager.Instance.currentCareer.AvailableContracts);
    }
}