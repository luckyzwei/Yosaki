using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsRewardManager : SingletonMono<ContentsRewardManager>
{
    private List<ObscuredInt> bonusDefenseEnemyCut = new List<ObscuredInt>() { 1, 2, 3, 4, 5 };

    private void Start()
    {
        StartCoroutine(RandomizeRoutine());
    }

    public int GetDefenseReward_BlueStone(int enemyNum)
    {
        return enemyNum * GameBalance.bonusDungeonGemPerEnemy;
    }

    public int GetDefenseReward_Marble(int enemyNum)
    {
        return enemyNum * GameBalance.bonusDungeonMarblePerEnemy;
    }
    private IEnumerator RandomizeRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(2.0f);

        while (true)
        {
            Randomize();
            yield return delay;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Randomize();
    }

    private void Randomize()
    {
        bonusDefenseEnemyCut.ForEach(e => e.RandomizeCryptoKey());
    }
}
