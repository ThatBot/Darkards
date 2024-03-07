using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform normalMarker;
    [SerializeField] private Transform overheadMarker;
    [SerializeField] private Transform sideMarker;

    private int currentPos = 0; // 0 -> Normal | 1 -> Overhead

    // Update is called once per frame
    void Update()
    {
        if(PauseController.Instance.IsPaused) return;

        if(Input.GetKeyDown(KeyCode.W) && currentPos == 0)
        {
            transform.position = overheadMarker.position;
            transform.rotation = overheadMarker.rotation;
            currentPos = 1;
        }
        else if(Input.GetKeyDown(KeyCode.S) && currentPos == 1)
        {
            transform.position = normalMarker.position;
            transform.rotation = normalMarker.rotation;
            currentPos = 0;
        }
        else if(Input.GetKeyDown(KeyCode.D) && currentPos == 0)
        {
            transform.position = sideMarker.position;
            transform.rotation = sideMarker.rotation;
            currentPos = 2;
        }
        else if(Input.GetKeyDown(KeyCode.A) && currentPos == 2)
        {
            transform.position = normalMarker.position;
            transform.rotation = normalMarker.rotation;
            currentPos = 0;
        }

        Ray _mouseRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_mouseRay, out RaycastHit _hit) && Input.GetMouseButtonDown(0))
        {
            if(_hit.transform.CompareTag("CardPlayArea"))
            {
                int _lane = Int32.Parse(_hit.transform.name);
                CardManager.Instance.PlayCard(_lane - 1);
            }
        }
    }
}
