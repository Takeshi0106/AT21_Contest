using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// =====================================
// EnemyManagerを管理して、resultsceneを呼ぶ
// ======================================

public class EnemySystem : MonoBehaviour
{
    [Header("Playerのオブジェクト")]
    [SerializeField] private GameObject player = null;

    private List<EnemyManager> enemyMan = new List<EnemyManager>();


    void Update()
    {
        EnemyManager nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyManager enemy in enemyMan)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.EnemyFlagFalse();

            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        nearest.NearEnemyFlag(player.transform.position);
    }



    // EnemyManagerの数を取得
    public void RegisterEnemyManager(EnemyManager enemy)
    {
        enemyMan.Add(enemy);
    }


    // resultsceneに移行
    private void LoadResultScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("PlayerWinScene");
    }


    // EnemyMangaerの全ての敵を倒したときの処理
    public void UnregisterEnemyManager(EnemyManager enemy)
    {
        enemyMan.Remove(enemy);

        if (enemyMan.Count == 0)
        {
            Debug.Log("すべての敵を倒しました！");
            LoadResultScene();
        }
    }


    // セットされているエネミーを遅くする
    public void EnemySlow(float slowSpead)
    {
        foreach (EnemyManager enemy in enemyMan)
        {
            enemy.EnemySlow(slowSpead);
        }
    }


    // プレイヤーの位置を渡して、最も近い EnemyState の位置を返す
    public Vector3 GetNearestEnemyPositionFromAll(Vector3 playerPosition)
    {
        EnemyManager nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyManager enemy in enemyMan)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        Vector3 nearestPosition = nearest.GetNearestEnemyPosition(playerPosition);

        return nearestPosition;
    }



}
