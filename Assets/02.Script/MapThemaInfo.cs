using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapThemaInfo", menuName = "ScriptableObjects/MapThemaInfo", order = 3)]
public class MapThemaInfo : ScriptableObject
{
    [SerializeField]
    public Sprite backGround;

    [SerializeField]
    public Sprite leftTile;

    [SerializeField]
    public Sprite centerTile;

    [SerializeField]
    public Sprite rightTile;
}
