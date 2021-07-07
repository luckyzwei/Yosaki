using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectZRotation : MonoBehaviour
{
    [SerializeField]
    private float minAngle = 0f;
    [SerializeField]
    private float maxAngle = 360f;

    private void OnEnable()
    {
        this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
    }
}
