using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private ICameraSetStrategy currentViewStrategy;

    float moveSpeed = 0.01f;

    private void Awake()
    {
        SetTopViewStrategy();
    }

    public void SetTopViewStrategy()
    {
        SetViewStrategy(new CameraTopView());
    }
    public void SetBackViewStrategy()
    {
        SetViewStrategy(new CameraBackView());
    }
    public void SetSideViewStrategy()
    {
        SetViewStrategy(new CameraSideView());
    }
    private void SetViewStrategy(ICameraSetStrategy strategy)
    {
        currentViewStrategy = strategy;
        currentViewStrategy.Action(this);
    }

    public void ChangeCameraPosition(Vector3 destPos, Quaternion destRot)
    {
        StartCoroutine(test_co(destPos, destRot));
    }

    private IEnumerator test_co(Vector3 destPos, Quaternion destRot)
    {
        int cnt = 0;

        while (Vector3.SqrMagnitude(transform.position - destPos) > 0.00001f && cnt++ < 1000)
        {
            transform.SetPositionAndRotation(Vector3.Slerp(transform.position, destPos, moveSpeed * Time.deltaTime * cnt),
                                            Quaternion.Slerp(transform.rotation, destRot, moveSpeed * Time.deltaTime * cnt));

            yield return null;
        }
        transform.position = destPos;
        transform.LookAt(Vector3.zero);
    }
}
