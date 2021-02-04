using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class SteamVRInputModule : BaseInputModule
{
    public Camera Cam;
    public SteamVR_Input_Sources TargetSource;
    public SteamVR_Action_Boolean ClickAction;

    private GameObject CurrentObject = null;
    private PointerEventData Data = null;

    protected override void Awake()
    {
        base.Awake();

        Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        Data.Reset();
        Data.position = new Vector2(Cam.pixelWidth / 2, Cam.pixelHeight / 2);

        eventSystem.RaycastAll(Data, m_RaycastResultCache);
        Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        CurrentObject = Data.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(Data, CurrentObject);

        if (ClickAction.GetStateDown(TargetSource))
        {
            ProcessPress(Data);
        }

        if (ClickAction.GetStateUp(TargetSource))
        {
            ProcessRelease(Data);
        }
    }

    public PointerEventData GetData() {
        return Data;
    }

    private void ProcessPress(PointerEventData data) {

    }

    private void ProcessRelease(PointerEventData data) {

    }
}
