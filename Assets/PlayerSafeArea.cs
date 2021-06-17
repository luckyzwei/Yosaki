using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSafeArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = Vector3.zero;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.transform.position = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.position = Vector3.zero;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        collision.transform.position = Vector3.zero;
    }
}
