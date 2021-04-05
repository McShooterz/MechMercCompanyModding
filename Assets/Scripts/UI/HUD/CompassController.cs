using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour
{
    [SerializeField]
    RawImage compassImage;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpdateCompassBar(Transform rotationReference)
    {
        // set compass bar texture coordinates
        compassImage.uvRect = new Rect((rotationReference.eulerAngles.y / 360f) - .5f, 0f, 1f, 1f);
    }
}
