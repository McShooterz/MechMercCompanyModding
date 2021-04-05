using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSubScreen : MonoBehaviour
{
    [SerializeField]
    Text missionText;

    [SerializeField]
    Image mapImage;

    [SerializeField]
    Button launchButton;

    [SerializeField]
    Camera planetPreviewCamera;

    [SerializeField]
    GameObject planetPreview;

    void Start()
    {
        Career career = GlobalDataManager.Instance.currentCareer;

        GameObject planetPrefab = career.currentContract.PlanetDefinition.GetPrefab();

        if (planetPrefab != null)
        {
            planetPreview = Instantiate(planetPrefab, planetPreviewCamera.transform);

            planetPreview.transform.localPosition = new Vector3(0f, 0f, 1.1f);
        }

        Texture2D mapTexture = career.currentContract.CurrentMission.MapDefinition.GetMapTexture();

        if (mapTexture != null)
        {
            mapImage.sprite = StaticHelper.GetSpriteUI(mapTexture);
        }
    }

    void Update()
    {
        if (planetPreview != null)
        {
            planetPreview.transform.RotateAround(planetPreview.transform.position, Vector3.up, 50f * Time.unscaledDeltaTime);
        }
    }

    void OnEnable()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        Career career = GlobalDataManager.Instance.currentCareer;
        ContractData contractData = career.currentContract;
        MissionSetup missionSetup = contractData.CurrentMission;
        MapDefinition mapDefinition = missionSetup.MapDefinition;

        bool dutyRosterValid = career.DutyRosterValid;

        stringBuilder.AppendLine("Mission: " + missionSetup.DisplayName);
        stringBuilder.AppendLine("Description: Description Required!");
        stringBuilder.AppendLine();

        if (dutyRosterValid)
        {
            stringBuilder.AppendLine("Duty Roster: Valid");
        }
        else
        {
            stringBuilder.AppendLine("Duty Roster: <color=red>Invalid</color>");
        }

        stringBuilder.AppendLine();

        stringBuilder.AppendLine("Enemy: " + contractData.EnemyDefinition.GetDisplayName());

        stringBuilder.AppendLine("Biome: " + StaticHelper.GetBiomeName(mapDefinition.Biome));

        if (mapDefinition.SkyWeatherElements.Length > 0 && missionSetup.weatherIndex != -1)
        {
            stringBuilder.AppendLine("Weather: " + mapDefinition.SkyWeatherElements[missionSetup.weatherIndex].GetDisplayName());
        }

        if (mapDefinition.SkyTimeElements.Length > 0 && missionSetup.timeOfDayIndex != -1)
        {
            stringBuilder.AppendLine("Time: " + mapDefinition.SkyTimeElements[missionSetup.timeOfDayIndex].GetDisplayName());
        }

        if (mapDefinition.CoolingModifier < 0)
        {
            stringBuilder.AppendLine("Cooling: " + (mapDefinition.CoolingModifier * 100.0f).ToString() + "%");
        }
        else
        {
            stringBuilder.AppendLine("Cooling: +" + (mapDefinition.CoolingModifier * 100.0f).ToString() + "%");
        }

        missionText.text = stringBuilder.ToString();

        launchButton.interactable = dutyRosterValid;
    }

    public void ClickLaunchButton()
    {
        GlobalDataManager.Instance.backSceneName = "MissionSummery";

        ResourceManager.Instance.StoreCareer(GlobalDataManager.Instance.currentCareer.CareerSave);

        GlobalData.Instance.PlayerMechSetup = GlobalDataManager.Instance.currentCareer.DutyRosterMechPlayer.MechSave;

        GlobalData.Instance.SetSquadMatesSetup(GlobalDataManager.Instance.currentCareer.DutyRosterMechs);
        GlobalData.Instance.SetSquadPilotsSetup(GlobalDataManager.Instance.currentCareer.DutyRosterPilots);

        GlobalData.Instance.MissionSetup.Copy(GlobalDataManager.Instance.currentCareer.currentContract.CurrentMission);

        AudioManager.Instance.PlayButtonClick(1);

        LoadingScreen.Instance.LoadScene(GlobalData.Instance.MissionSetup.MapDefinition.Scene);
    }
}
