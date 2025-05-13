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
    private List<EnemyManager> enemyMan = new List<EnemyManager>();


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

        SceneManager.LoadScene("ResultScene");
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
}
