using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform normalMarker;
    [SerializeField] private Transform overheadMarker;

    private int currentPos = 0; // 0 -> Normal | 1 -> Overhead

    // Update is called once per frame
    void Update()
    {
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
    }
}
