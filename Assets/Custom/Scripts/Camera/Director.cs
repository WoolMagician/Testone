using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedUtilities.Cameras;

public class Director : Singleton<Director>
{
    [HideInInspector]
    public MultiCameraController multiCameraController;

    public Camera MainCamera
    {
        get
        {
           return this.multiCameraController.Camera;
        }
    }

    private void Awake()
    {
        multiCameraController = this.GetComponent<MultiCameraController>();
    }

    public void SwitchToCamera(int cameraIndex)
    {
        this.multiCameraController.CurrentIndex = cameraIndex;
    }
}
