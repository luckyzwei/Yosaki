using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NataRotate : MonoBehaviour
{
    [SerializeField]
    private GameObject rotateObject;

    [SerializeField]
    private float rotateSpeed = 0f;

    private float currentAngle;

    void Update()
    {
        currentAngle += Time.deltaTime * rotateSpeed;

        rotateObject.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

        if (currentAngle >= 360f)
        {
            currentAngle = currentAngle - 360f;
        }
    }
}
