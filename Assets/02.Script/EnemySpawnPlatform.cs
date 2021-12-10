using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPlatform : MonoBehaviour
{
    private BoxCollider2D collider;
    private Rect colliderWorldRect;
    private float minXPos;
    private float maxXPos;
    private float yPos;

    private float yOffset = 1f;

    [SerializeField]
    private float xOffset = 2f;

    private void Awake()
    {
        SetMovePos();
    }

    private void SetMovePos()
    {
        collider = GetComponent<BoxCollider2D>();
        colliderWorldRect = collider.GetWorldBounds();
        minXPos = colliderWorldRect.xMin;
        maxXPos = colliderWorldRect.xMax;
        yPos = colliderWorldRect.yMax;
    }

    public Vector3 GetRandomSpawnPos()
    {
        return new Vector3(Random.Range(minXPos+ xOffset, maxXPos- xOffset), yPos + yOffset);
    }

}


