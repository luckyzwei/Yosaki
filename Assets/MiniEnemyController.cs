using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniEnemyController : MonoBehaviour
{
    [SerializeField]
    private float waitingCount = 0f;
    [SerializeField]
    private GameObject enemyGameObject;
    private void OnEnable()
    {
        enemyGameObject.SetActive(false);
        StartCoroutine(StartMoveRoutine());
    }

    private IEnumerator StartMoveRoutine()
    {
        WaitForSeconds randMoveDelay = new WaitForSeconds(waitingCount);

        while (true)
        {
            yield return randMoveDelay;
            if (enemyGameObject != null)
            {
                enemyGameObject.SetActive(true);
            }
        }
    }
}
