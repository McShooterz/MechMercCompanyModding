using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using SickscoreGames.HUDNavigationSystem;

public class MiniMapController : MonoBehaviour
{
    [SerializeField]
    RectTransform mapContainer;

    [SerializeField]
    Image mapImage;

    //[SerializeField]
    //HNSMapProfile mapProfile;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    /*
    public void SetMapImage(HNSMapProfile profile)
    {
        mapProfile = profile;
        mapImage.sprite = profile.MapTexture;
        mapImage.preserveAspect = true;
        mapImage.SetNativeSize();
    }

    public void UpdateMinimap(Transform reference, float scale)
    {
        // assign map rotation
        Quaternion identityRotation = transform.rotation;
        Quaternion mapRotation = identityRotation;
        mapRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, reference.eulerAngles.y);

        // calculate map position
        Vector2 unitScale = mapProfile.GetMapUnitScale();
        Vector3 posOffset = mapProfile.MapBounds.center - reference.position;
        Vector3 mapPos = new Vector3(posOffset.x * unitScale.x, posOffset.z * unitScale.y, 0f) * scale;

        // set map position, rotation and scale
        mapPos = reference.MinimapRotationOffset(mapPos);
        mapContainer.localPosition = new Vector2(mapPos.x, mapPos.y);
        mapContainer.rotation = mapRotation;
        mapContainer.localScale = new Vector3(scale, scale, 1f);
    }*/
}
