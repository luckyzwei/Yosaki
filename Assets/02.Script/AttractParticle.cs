using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractParticle : MonoBehaviour
{
    [SerializeField]
    private Transform attractObject;

    void Update()
    {
        attractObject.transform.position = PlayerMoveController.Instance.transform.position;
    }
}
