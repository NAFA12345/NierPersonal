using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScrolling : MonoBehaviour
{
    [SerializeField] float ZScale;
    [SerializeField] float RepositionRatio;

    public float scrollingSpeed;

    Vector3 offsetPosition;
    float repotision;

    private void Awake()
    {
        offsetPosition = new Vector3(0, 0, 3f * ZScale);
        repotision = -RepositionRatio * ZScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z <= repotision)
        {
            Reposition();
        }
        transform.Translate(Vector3.back * scrollingSpeed * Time.deltaTime);
    }

    void Reposition()
    {
        transform.position = transform.position + offsetPosition;
    }
}
