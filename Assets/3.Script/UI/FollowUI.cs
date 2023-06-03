using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour
{
    public GameObject target_obj;
    private RectTransform UITrans;
    private Camera mainCamera;

    private Vector3 distance = new Vector3(-200, 150, 0);

    private void Awake()
    {
        TryGetComponent(out UITrans);
        mainCamera = Camera.main;
        target_obj = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        if (!target_obj.activeSelf)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 ScreenPosition = mainCamera.WorldToScreenPoint(target_obj.transform.position);

        UITrans.position = ScreenPosition + distance;
    }
}
