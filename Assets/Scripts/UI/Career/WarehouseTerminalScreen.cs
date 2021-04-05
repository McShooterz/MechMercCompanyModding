using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseTerminalScreen : MonoBehaviour
{
    [SerializeField]
    HomeBaseScreen homeBaseScreen;

    [SerializeField]
    InventorySubScreen inventorySubScreen;

    [SerializeField]
    InventoryMarketSubScreen inventoryMarketSubScreen;

    [SerializeField]
    Image inventoryButtonBackground;

    [SerializeField]
    Image marketButtonBackground;

    [SerializeField]
    Color activeColor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickClose();
        }
    }

    void OnEnable()
    {
        inventorySubScreen.gameObject.SetActive(false);
        inventoryMarketSubScreen.gameObject.SetActive(false);

        inventoryButtonBackground.color = activeColor;
        marketButtonBackground.color = Color.white;

        inventorySubScreen.gameObject.SetActive(true);
    }

    public void ClickInventoryButton()
    {
        inventorySubScreen.gameObject.SetActive(true);
        inventoryMarketSubScreen.gameObject.SetActive(false);

        inventoryButtonBackground.color = activeColor;
        marketButtonBackground.color = Color.white;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickMarketButton()
    {
        inventorySubScreen.gameObject.SetActive(false);
        inventoryMarketSubScreen.gameObject.SetActive(true);

        inventoryButtonBackground.color = Color.white;
        marketButtonBackground.color = activeColor;

        AudioManager.Instance.PlayButtonClick(0);
    }

    public void ClickClose()
    {
        homeBaseScreen.gameObject.SetActive(true);

        gameObject.SetActive(false);

        AudioManager.Instance.PlayButtonClick(0);
    }
}
