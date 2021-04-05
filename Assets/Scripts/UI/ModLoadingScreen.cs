using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ModLoadingScreen : MonoBehaviour
{
    [SerializeField]
    Transform listContent;

    [SerializeField]
    ModButton firstModButton;

    [SerializeField]
    List<ModButton> modButtons = new List<ModButton>();

    [SerializeField]
    GameObject modInformationRoot;

    [SerializeField]
    Image modPreviewImage;

    [SerializeField]
    Text modNameText;

    [SerializeField]
    Text modPathText;

    [SerializeField]
    Text modDescriptionText;

    [SerializeField]
    Text modSteamText;

    [SerializeField]
    Text modAuthorText;

    [SerializeField]
    Text modVersiontext;

    [SerializeField]
    Text gameVersionText;

    [SerializeField]
    Button uploadToSteamButton;

    [SerializeField]
    Button updateOnSteamButton;

    [SerializeField]
    Button startWithModsButton;

    [SerializeField]
    Color modButtonColorDefault;

    [SerializeField]
    Color modButtonColorHighlighted;

    ModInfo selectedModInfo;

    public delegate void ReturnSteamWorksPublishedID(ulong publishedID);

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        List<ModInfo> localMods = new List<ModInfo>();
        List<ModInfo> steamMods = new List<ModInfo>();

        // Get local mods
        if (Directory.Exists(Application.streamingAssetsPath + "/Mods"))
        {
            FileInfo[] files = ResourceManager.GetFilesByType(Application.streamingAssetsPath + "/Mods", SearchOption.AllDirectories, "ModInfo.xml");
            ModInfo modInfo;

            foreach (FileInfo file in files)
            {
                modInfo = ResourceManager.LoadDataFromXML<ModInfo>(file.FullName);

                if (modInfo != null)
                {
                    modInfo.SetModPath(file.Directory.FullName);
                    modInfo.SetModType("Local");
                    localMods.Add(modInfo);
                }
            }
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Mods");
        }

        // Get Steam Workshop mods
        if (SteamManager.Initialized)
        {
            List<string> steamworkshopModPaths = SteamWorkshop.Instance.GetListOfSubscribedItemsPaths();

            ModInfo modInfo;

            foreach (string modPath in steamworkshopModPaths)
            {
                modInfo = ResourceManager.LoadDataFromXML<ModInfo>(modPath + "/ModInfo.xml");

                if (modInfo != null)
                {
                    modInfo.SetModPath(modPath);
                    modInfo.SetModType("Steam Workshop");
                    steamMods.Add(modInfo);
                }
            }
        }

        if (localMods.Count == 0 && steamMods.Count == 0)
        {
            LoadingScreen.Instance.LoadScene("CachingScene");

            return;
        }

        // Order mods
        List<ModEntry> modEntries = new List<ModEntry>();

        if (localMods.Count > 0 || steamMods.Count > 0)
        {
            List<ModInfo> allMods = new List<ModInfo>();
            allMods.AddRange(localMods);
            allMods.AddRange(steamMods);

            if (File.Exists(Application.streamingAssetsPath + "/Mods/ModLoadOrder.xml"))
            {
                ModLoadOrder modLoadOrder = ResourceManager.LoadDataFromXML<ModLoadOrder>(Application.streamingAssetsPath + "/Mods/ModLoadOrder.xml");

                if (modLoadOrder != null)
                {
                    foreach (ModLoadOrder.ModEntry modLoadEntry in modLoadOrder.ModEntries)
                    {
                        for (int i = 0; i < allMods.Count; i++)
                        {
                            ModInfo modInfo = allMods[i];

                            if (modLoadEntry.Path == modInfo.ModPath)
                            {
                                ModEntry modEntry = new ModEntry();
                                modEntry.modInfo = modInfo;
                                modEntry.isActive = modLoadEntry.IsActive;
                                modEntries.Add(modEntry);

                                allMods.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (ModInfo modInfo in allMods)
            {
                ModEntry modEntry = new ModEntry();
                modEntry.modInfo = modInfo;
                modEntries.Add(modEntry);
            }

            // Create buttons
            if (modEntries.Count > 0)
            {
                firstModButton.Initialize(this, modEntries[0].modInfo, modEntries[0].isActive);
                modButtons.Add(firstModButton);

                for (int i = 1; i < modEntries.Count; i++)
                {
                    ModButton modButton = Instantiate(firstModButton.gameObject, listContent).GetComponent<ModButton>();
                    modButton.Initialize(this, modEntries[i].modInfo, modEntries[i].isActive);
                    modButtons.Add(modButton);
                }

                SelectModButton(firstModButton);
            }
            else
            {
                firstModButton.gameObject.SetActive(false);
                modInformationRoot.SetActive(false);
                startWithModsButton.interactable = false;
            }
        }
        else
        {
            firstModButton.gameObject.SetActive(false);

            LoadingScreen.Instance.LoadScene("CachingScene");

            return;
        }

        SteamWorkshop.Instance.SetPublishCallback(PublishedCallback);
    }

    private void OnEnable()
    {
        
    }

    public void SelectModButton(ModButton modButton)
    {
        if (selectedModInfo == modButton.ModInfo)
            return;

        foreach (ModButton button in modButtons)
        {
            if (button == modButton)
                button.BackGround.color = modButtonColorHighlighted;
            else
                button.BackGround.color = modButtonColorDefault;
        }

        SelectModInfo(modButton.ModInfo);
    }

    void SelectModInfo(ModInfo modInfo)
    {
        selectedModInfo = modInfo;

        modPreviewImage.sprite = selectedModInfo.PreviewImage;

        modNameText.text = selectedModInfo.DisplayName;

        modPathText.text = "Path: " + selectedModInfo.ModPath;

        modDescriptionText.text = "Description: " + selectedModInfo.Description;

        modAuthorText.text = "Author: " + selectedModInfo.Author;

        modVersiontext.text = "Mod Version: " + selectedModInfo.ModVersion;

        gameVersionText.text = "Game Version: " + selectedModInfo.BaseGameVersion;

        if (selectedModInfo.SteamPublishedID != 0)
        {
            modSteamText.text = "Steam ID: " + selectedModInfo.SteamPublishedID.ToString();
        }
        else
        {
            modSteamText.text = "Steam ID: N/A";
        }

        if (SteamManager.Initialized)
        {
            if (selectedModInfo.SteamPublishedID != 0)
            {
                uploadToSteamButton.gameObject.SetActive(false);
                updateOnSteamButton.gameObject.SetActive(true);
            }
            else
            {
                uploadToSteamButton.gameObject.SetActive(true);
                updateOnSteamButton.gameObject.SetActive(false);
            }
        }
        else
        {
            uploadToSteamButton.interactable = false;
            updateOnSteamButton.gameObject.SetActive(false);
        }
    }

    public void PublishedCallback(ulong publishedID)
    {
        if (selectedModInfo != null && publishedID != 0)
        {
            selectedModInfo.SteamPublishedID = publishedID;
            SelectModInfo(selectedModInfo);

            ResourceManager.SaveToXML(selectedModInfo, selectedModInfo.ModPath + "/ModInfo.xml");
        }
    }

    public void MoveButtonUp(ModButton modButton)
    {
        if (modButtons.Count < 2)
            return;

        int targetIndex = modButton.transform.GetSiblingIndex();

        if (targetIndex == 0)
            return;

        listContent.GetChild(targetIndex - 1).SetSiblingIndex(targetIndex);
        modButton.transform.SetSiblingIndex(targetIndex - 1);
    }

    public void MoveButtonDown(ModButton modButton)
    {
        if (modButtons.Count < 2)
            return;

        int targetIndex = modButton.transform.GetSiblingIndex();

        if (targetIndex == modButtons.Count - 1)
            return;

        listContent.GetChild(targetIndex + 1).SetSiblingIndex(targetIndex);
        modButton.transform.SetSiblingIndex(targetIndex + 1);
    }

    List<ModLoadOrder.ModEntry> GetModLoadEntries()
    {
        List<ModLoadOrder.ModEntry> modEntries = new List<ModLoadOrder.ModEntry>();

        foreach (Transform transform in listContent)
        {
            ModButton modButton = transform.GetComponent<ModButton>();

            if (modButton != null)
            {
                ModLoadOrder.ModEntry modEntry = new ModLoadOrder.ModEntry();
                modEntry.Path = modButton.ModInfo.ModPath;
                modEntry.IsActive = modButton.IsActive;
                modEntries.Add(modEntry);
            }
        }

        return modEntries;
    }

    public void ClickUploadToSteamButton()
    {
        SteamWorkshop.Instance.UploadContent(selectedModInfo.DisplayName, selectedModInfo.Description, selectedModInfo.ModPath, new string[0], selectedModInfo.ModPath + "/" + selectedModInfo.PreviewImagePath);
    }

    public void ClickUpdateOnSteamButton()
    {
        SteamWorkshop.Instance.UpdateContent(selectedModInfo.DisplayName, selectedModInfo.Description, selectedModInfo.ModPath, new string[0], selectedModInfo.ModPath + "/" + selectedModInfo.PreviewImagePath, selectedModInfo.SteamPublishedID);
    }

    public void ClickStartWithModsButton()
    {
        AudioManager.Instance.PlayButtonClick(1);

        ModLoadOrder modLoadOrder = new ModLoadOrder();
        modLoadOrder.LoadMods = true;

        modLoadOrder.ModEntries = GetModLoadEntries().ToArray();

        ResourceManager.SaveToXML(modLoadOrder, Application.streamingAssetsPath + "/Mods/ModLoadOrder.xml");

        LoadingScreen.Instance.LoadScene("CachingScene");
    }

    public void ClickStartWithoutModsButton()
    {
        AudioManager.Instance.PlayButtonClick(1);

        ModLoadOrder modLoadOrder = new ModLoadOrder();

        modLoadOrder.ModEntries = GetModLoadEntries().ToArray();

        ResourceManager.SaveToXML(modLoadOrder, Application.streamingAssetsPath + "/Mods/ModLoadOrder.xml");

        LoadingScreen.Instance.LoadScene("CachingScene");
    }

    public void ClickQuitToDesktop()
    {
        AudioManager.Instance.PlayButtonClick(1);

        Application.Quit();
    }

    class ModEntry
    {
        public ModInfo modInfo;

        public bool isActive;
    }
}
