using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float DefaultLength = 5.0f;
    public GameObject Dot;
    public SteamVRInputModule SteamVRInputModule;

    private LineRenderer lineRenderer = null; 

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        float TargetLength = DefaultLength;

        RaycastHit Hit = CreateRaycast(TargetLength);

        Vector3 EndPosition = transform.position + (transform.forward * TargetLength);

        if (Hit.collider != null)
        {
            EndPosition = Hit.point;
        }

        Dot.transform.position = EndPosition;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, EndPosition);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, DefaultLength);

        return hit;
    }
}
