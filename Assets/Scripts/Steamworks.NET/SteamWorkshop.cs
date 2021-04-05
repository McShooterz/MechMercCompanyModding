using UnityEngine;
using Steamworks;
using System.Collections.Generic;

internal struct SteamWorkshopItem
{
    public string ContentFolderPath;
    public string Description;
    public string PreviewImagePath;
    public string[] Tags;
    public string Title;
}

public class SteamWorkshop : MonoBehaviour
{
    private SteamWorkshopItem currentSteamWorkshopItem;
    private PublishedFileId_t publishedFileID;

    ModLoadingScreen.ReturnSteamWorksPublishedID returnSteamWorksPublishedID;

    public static SteamWorkshop Instance { get; private set; }

    public List<string> GetListOfSubscribedItemsPaths()
    {
        uint subscribedCount = SteamUGC.GetNumSubscribedItems();
        PublishedFileId_t[] subscribedFiles = new PublishedFileId_t[subscribedCount];
        SteamUGC.GetSubscribedItems(subscribedFiles, (uint)subscribedFiles.Length);

        ulong sizeOnDisk = 0;
        string installLocation = string.Empty;
        uint timeStamp = 0;

        var result = new List<string>();

        foreach (PublishedFileId_t file in subscribedFiles)
        {
            SteamUGC.GetItemInstallInfo(file, out sizeOnDisk, out installLocation, 1024, out timeStamp);
            result.Add(installLocation);
        }

        return result;
    }

    public void UploadContent(string itemTitle, string itemDescription, string contentFolderPath, string[] tags, string previewImagePath)
    {
        currentSteamWorkshopItem = new SteamWorkshopItem
        {
            Title = itemTitle,
            Description = itemDescription,
            ContentFolderPath = contentFolderPath,
            Tags = tags,
            PreviewImagePath = previewImagePath
        };

        CreateItem();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {        
        /*if (SteamManager.Initialized)
        {
            print("Uploading test mod");
            string name = "Test Mod";
            string description = "Test Mod Description";
            string contentPath = Application.streamingAssetsPath + "/Mods/TestMod";
            string imagePath = Application.streamingAssetsPath + "/Mods/TestMod.png";
            UploadContent(name, description, contentPath, new string[0], imagePath);
        }*/
    }

    void CreateItem()
    {
        SteamAPICall_t steamAPICall = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
        CallResult<CreateItemResult_t> steamAPICallResult = CallResult<CreateItemResult_t>.Create();
        steamAPICallResult.Set(steamAPICall, CreateItemResult);
    }

    private void CreateItemResult(CreateItemResult_t param, bool bIOFailure)
    {
        if (param.m_eResult == EResult.k_EResultOK)
        {
            publishedFileID = param.m_nPublishedFileId;
            UpdateItem();
            returnSteamWorksPublishedID(publishedFileID.m_PublishedFileId);
        }
        else
        {
            Debug.Log("Couldn't create a new item");
        }
    }

    private void UpdateItem()
    {
        UGCUpdateHandle_t updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), publishedFileID);

        SteamUGC.SetItemTitle(updateHandle, currentSteamWorkshopItem.Title);
        SteamUGC.SetItemDescription(updateHandle, currentSteamWorkshopItem.Description);
        SteamUGC.SetItemContent(updateHandle, currentSteamWorkshopItem.ContentFolderPath);
        SteamUGC.SetItemTags(updateHandle, currentSteamWorkshopItem.Tags);
        SteamUGC.SetItemPreview(updateHandle, currentSteamWorkshopItem.PreviewImagePath);
        SteamUGC.SetItemVisibility(updateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);

        SteamAPICall_t steamAPICall = SteamUGC.SubmitItemUpdate(updateHandle, "");
        CallResult<SubmitItemUpdateResult_t> steamAPICallResult = CallResult<SubmitItemUpdateResult_t>.Create();
        steamAPICallResult.Set(steamAPICall, UpdateItemResult);
    }

    private void UpdateItemResult(SubmitItemUpdateResult_t param, bool bIOFailure)
    {
        if (param.m_eResult == EResult.k_EResultOK)
        {
            Debug.Log("Sucessfully submitted item to Steam");
        }
        else
        {
            Debug.Log("Couldn't submit the item to Steam");
        }
    }

    public void SetPublishCallback(ModLoadingScreen.ReturnSteamWorksPublishedID callbackFunction)
    {
        returnSteamWorksPublishedID = callbackFunction;
    }

    public void UpdateContent(string itemTitle, string itemDescription, string contentFolderPath, string[] tags, string previewImagePath, ulong publishedFileID)
    {
        UGCUpdateHandle_t updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), new PublishedFileId_t(publishedFileID));

        SteamUGC.SetItemTitle(updateHandle, itemTitle);
        SteamUGC.SetItemDescription(updateHandle, itemDescription);
        SteamUGC.SetItemContent(updateHandle, contentFolderPath);
        SteamUGC.SetItemTags(updateHandle, tags);
        SteamUGC.SetItemPreview(updateHandle, previewImagePath);
        SteamUGC.SetItemVisibility(updateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);

        SteamAPICall_t steamAPICall = SteamUGC.SubmitItemUpdate(updateHandle, "");
        CallResult<SubmitItemUpdateResult_t> steamAPICallResult = CallResult<SubmitItemUpdateResult_t>.Create();
        steamAPICallResult.Set(steamAPICall, UpdateItemResult);
    }
}
