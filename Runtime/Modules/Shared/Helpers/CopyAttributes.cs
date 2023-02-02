using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CopyAttributes : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;
    [SerializeField] private Camera uiCamera;

    private void LateUpdate()
    {
        //uiCamera.CopyFrom(worldCamera);
        uiCamera.fieldOfView = worldCamera.fieldOfView;
    }
}
