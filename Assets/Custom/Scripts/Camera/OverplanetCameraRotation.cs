using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverplanetCameraRotation : MonoBehaviour
{
    public Transform cameraTransformToFollow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraTransformToFollow != null)
        {
            this.transform.LookAt(new Vector3(-cameraTransformToFollow.position.x, this.transform.position.y, -cameraTransformToFollow.position.z));
        }
    }
}
