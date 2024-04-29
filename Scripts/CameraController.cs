using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float cameraHeight;
    private Camera mainCam;
    private Transform mainCamTransform;

    private PlayerInputManager inputManager;

    void Start()
    {
        mainCam = Camera.main;
        mainCamTransform = mainCam.transform;
        cameraHeight = mainCamTransform.position.y;
        inputManager = PlayerInputManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(inputManager.GetCameraMovementDirection());
    }

    private void MoveCamera(Vector2 direction)
    {
        Vector3 dir = new Vector3(direction.x, 0f, direction.y);
        Vector3 newPos = mainCamTransform.position + dir * cameraSpeed * Time.deltaTime;
        newPos.y = cameraHeight;
        mainCamTransform.position = newPos;
    }
}
