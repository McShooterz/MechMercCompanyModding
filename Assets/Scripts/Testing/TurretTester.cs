#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTester : MonoBehaviour
{
    [SerializeField]
    Camera currentCamera;

    [SerializeField]
    TurretController turretController;

    [SerializeField]
    GameObject aimIndicator;

    [SerializeField]
    LayerMask targetingLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        TurretSetting turretSetting = new TurretSetting();

        turretSetting.HorizontalLimit = 360;
        turretSetting.HorizontalSpeed = 30;
        turretSetting.VerticalLimitUp = 20;
        turretSetting.VerticalLimitDown = 20;
        turretSetting.VerticalSpeed = 10;

        turretController.SetTurretSetting(turretSetting);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f, targetingLayerMask))
            {
                aimIndicator.transform.position = hit.point;

                turretController.AimAtPosition(hit.point);
            }
        }
    }
}
#endif
