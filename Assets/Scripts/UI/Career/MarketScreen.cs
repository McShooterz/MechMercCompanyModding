using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketScreen : MonoBehaviour
{

    [SerializeField]
    GameObject mechsMarketGameObject;

    [SerializeField]
    GameObject componentsMarketGameObject;

    [SerializeField]
    GameObject marketMechDesignButtonPrefab;

    [SerializeField]
    GameObject marketMechDataButtonPrefab;

    [SerializeField]
    GameObject marketComponentButtonPrefab;

    [SerializeField]
    MarketMechDesignButton[] marketMechDesignButtons = new MarketMechDesignButton[0];

    [SerializeField]
    MarketMechDataButton[] playerMarketMechDataButtons = new MarketMechDataButton[0];

    [SerializeField]
    MarketComponentButton[] marketComponentButtons = new MarketComponentButton[0];

    [SerializeField]
    MarketComponentButton[] playerComponentButtons = new MarketComponentButton[0];

    [SerializeField]
    Transform marketMechListContent;

    [SerializeField]
    Transform playerMechListContent;

    [SerializeField]
    Transform marketComponentListContent;

    [SerializeField]
    Transform playerComponentListContent;

    [SerializeField]
    ScrollRect playerComponentScrollRect;

    [SerializeField]
    ScrollRect marketComponentScrollRect;

    [SerializeField]
    Vector2 mechDataButtonSize;

    [SerializeField]
    Color selectedButtonColor;

    [SerializeField]
    Color defaultButtonColor;

    [SerializeField]
    Button sellMechButton;

    [SerializeField]
    Button buyMechButton;

    [SerializeField]
    Button sellComponentButton;

    [SerializeField]
    Button buyComponentButton;

    [SerializeField]
    Text mechInfoText;

    [SerializeField]
    Text ComponentInfoText;

    [SerializeField]
    Transform mechPreviewCameraTransform;

    [SerializeField]
    MechMetaController mechPreview;

    MechDesign selectedBuyMechDesign;

    MechData selectedSellMechData;

    ComponentDefinition selectedComponentDefinition;

    void Awake()
    {
        mechInfoText.text = "";

        buyMechButton.gameObject.SetActive(false);
        sellMechButton.gameObject.SetActive(false);

        mechsMarketGameObject.SetActive(true);
        componentsMarketGameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        mechDataButtonSize = new Vector2(mechDataButtonSize.x / 1920 * Screen.width, mechDataButtonSize.y / 1080 * Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (mechPreview != null)
        {
            mechPreview.transform.RotateAround(mechPreview.transform.position, Vector3.up, 100f * Time.unscaledDeltaTime);
        }
    }

    void ClearSelectedComponent()
    {
        ComponentInfoText.text = "";

        buyComponentButton.gameObject.SetActive(false);
        sellComponentButton.gameObject.SetActive(false);

        selectedComponentDefinition = null;
    }

    void BuildMarketComponentsList(ComponentType componentType)
    {
        foreach (MarketComponentButton marketComponentButton in marketComponentButtons)
        {
            Destroy(marketComponentButton.gameObject);
        }

        List<InventoryComponentEntry> componentEntriesOfType = new List<InventoryComponentEntry>();

        marketComponentButtons = new MarketComponentButton[componentEntriesOfType.Count];

        for (int i = 0; i < marketComponentButtons.Length; i++)
        {
            InventoryComponentEntry inventoryComponentEntry = componentEntriesOfType[i];

            GameObject marketComponentButtonObject = Instantiate(marketComponentButtonPrefab, marketComponentListContent);
            marketComponentButtons[i] = marketComponentButtonObject.GetComponent<MarketComponentButton>();
            marketComponentButtons[i].LayoutElement.preferredWidth = mechDataButtonSize.x;
            marketComponentButtons[i].LayoutElement.preferredHeight = mechDataButtonSize.y;
            marketComponentButtons[i].Initialize(inventoryComponentEntry.componentDefinition, inventoryComponentEntry.Count, (int)(inventoryComponentEntry.componentDefinition.MarketValue * 1.25f), SelectMarketComponent);
        }
    }

    void BuildPlayerComponentsList(ComponentType componentType)
    {
        foreach (MarketComponentButton marketComponentButton in playerComponentButtons)
        {
            Destroy(marketComponentButton.gameObject);
        }

        List<InventoryComponentEntry> componentEntriesOfType = new List<InventoryComponentEntry>();

        playerComponentButtons = new MarketComponentButton[componentEntriesOfType.Count];

        for (int i = 0; i < playerComponentButtons.Length; i++)
        {
            InventoryComponentEntry inventoryComponentEntry = componentEntriesOfType[i];

            GameObject marketComponentButtonObject = Instantiate(marketComponentButtonPrefab, playerComponentListContent);
            playerComponentButtons[i] = marketComponentButtonObject.GetComponent<MarketComponentButton>();
            playerComponentButtons[i].LayoutElement.preferredWidth = mechDataButtonSize.x;
            playerComponentButtons[i].LayoutElement.preferredHeight = mechDataButtonSize.y;
            playerComponentButtons[i].Initialize(inventoryComponentEntry.componentDefinition, inventoryComponentEntry.Count, (int)(inventoryComponentEntry.componentDefinition.MarketValue * 0.75f), SelectPlayerComponent);
        }
    }

    public void SelectMarketComponent(ComponentDefinition componentDefinition, MarketComponentButton marketComponentButton)
    {
        selectedComponentDefinition = componentDefinition;

        sellComponentButton.gameObject.SetActive(false);
        buyComponentButton.gameObject.SetActive(true);

        for (int i = 0; i < marketComponentButtons.Length; i++)
        {
            if (marketComponentButtons[i] == marketComponentButton)
            {
                marketComponentButtons[i].BackgroundImage.color = selectedButtonColor;
            }
            else
            {
                marketComponentButtons[i].BackgroundImage.color = defaultButtonColor;
            }
        }

        foreach (MarketComponentButton componentButton in playerComponentButtons)
        {
            componentButton.BackgroundImage.color = defaultButtonColor;
        }

        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        stringBuilder.AppendLine(selectedComponentDefinition.GetDisplayInformation());

        stringBuilder.Append("\nCost: " + StaticHelper.FormatMoney((int)(selectedComponentDefinition.MarketValue * 1.25f)));

        ComponentInfoText.text = stringBuilder.ToString();
    }

    public void SelectPlayerComponent(ComponentDefinition componentDefinition, MarketComponentButton marketComponentButton)
    {
        selectedComponentDefinition = componentDefinition;

        sellComponentButton.gameObject.SetActive(true);
        buyComponentButton.gameObject.SetActive(false);

        for (int i = 0; i < playerComponentButtons.Length; i++)
        {
            if (playerComponentButtons[i] == marketComponentButton)
            {
                playerComponentButtons[i].BackgroundImage.color = selectedButtonColor;
            }
            else
            {
                playerComponentButtons[i].BackgroundImage.color = defaultButtonColor;
            }
        }

        foreach (MarketComponentButton componentButton in marketComponentButtons)
        {
            componentButton.BackgroundImage.color = defaultButtonColor;
        }

        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        stringBuilder.AppendLine(selectedComponentDefinition.GetDisplayInformation());

        stringBuilder.Append("\nSell Value: " + StaticHelper.FormatMoney((int)(selectedComponentDefinition.MarketValue * 1.25f)));

        ComponentInfoText.text = stringBuilder.ToString();
    }

}
