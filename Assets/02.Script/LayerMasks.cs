using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMasks
{
    private static string Enemy = "Enemy";
    public static int EnemyLayerMask = 1 << LayerMask.NameToLayer(Enemy);

    private static string Platform = "Platform";
    public static int PlatformLayerMask_Ray = 1 << LayerMask.NameToLayer(Platform);
    public static int PlatformLayerMask =  LayerMask.NameToLayer(Platform);

    private static string EnemyWall = "EnemyWall";
    public static int EnemyWallLayerMask = 1 << LayerMask.NameToLayer(EnemyWall);

    private static string DropItem = "DropItem";
    public static int DropItemLayerMask = LayerMask.NameToLayer(DropItem);

    private static string DropItemSpawned = "DropItemSpawned";
    public static int DropItemSpawnedLayerMask = LayerMask.NameToLayer(DropItemSpawned);
}

public static class Tags
{
    public static string Player = "Player";
    public static string Pet = "Pet";
    public static string Platform = "Platform";
    public static string Boss = "Boss";
}
