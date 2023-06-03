using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBackView : ICameraSetStrategy
{
    Vector3 destPos = -0.33f * Vector3.forward;
    Quaternion destRot = Quaternion.identity;
    
    public void Action(CameraControl camera)
    {
        camera.ChangeCameraPosition(destPos, destRot);
    }
}

public class CameraSideView : ICameraSetStrategy
{
    Vector3 destPos = 0.238f * Vector3.right;
    Quaternion destRot = Quaternion.Euler(0, -90, 0);

    public void Action(CameraControl camera)
    {
        camera.ChangeCameraPosition(destPos, destRot);
    }
}

public class CameraTopView : ICameraSetStrategy
{
    Vector3 destPos = 0.33f * Vector3.up;
    Quaternion destRot = Quaternion.Euler(90, 0, 0);

    public void Action(CameraControl camera)
    {
        camera.ChangeCameraPosition(destPos, destRot);
    }
}
