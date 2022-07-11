﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFov : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera playerCamera;
    private float targetFov;
    private float fov;

    void Start()
    {
        playerCamera =GetComponent<Camera>();
        targetFov = playerCamera.fieldOfView;
        fov = targetFov;
        
    }

    // Update is called once per frame
    void Update()
    {
        float fovSpeed = 5f;
        fov = Mathf.Lerp(fov,targetFov,Time.deltaTime * fovSpeed);
        playerCamera.fieldOfView = fov;
        
    }
    public void SetCameraFov(float targetFov)
    {
        this.targetFov = targetFov;
    }
}
