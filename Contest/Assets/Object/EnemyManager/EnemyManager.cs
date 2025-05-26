using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// ===============================
// EnemyManager
// 当たったらEnemyを呼び出す
// ===============================

public class EnemyManager : MonoBehaviour
{
    private List<EnemyState> enemies = new List<EnemyState>();
    private UnityEvent<float> onEnemySlow = new UnityEvent<float>() { };
    EnemySystem enemyManager = null;
    int cnt = 0;

    [Header("プレイヤーのタグ名")]
    [SerializeField] private string playerTag = "Player";
    [Header("敵のsystem")]
    [SerializeField] private EnemySystem system = null;


    // Enemyを数を取得
    public void RegisterEnemy(EnemyState enemy)
    {
        // Enemyを追加する
        enemies.Add(enemy);

        if (cnt == 0)
        {
            // EnemySystemに自分を渡す
            enemyManager = system.GetComponent<EnemySystem>();
            enemyManager.RegisterEnemyManager(this);

            cnt = 1;
        }
    }


    // 敵を倒したときの処理
    public void UnregisterEnemy(EnemyState enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            // 敵が倒されたらEnemySystemに知らせる
            enemyManager.UnregisterEnemyManager(this);
        }
    }


    // セットされているエネミーを遅くする
    public void EnemySlow(float slowSpead)
    {
        onEnemySlow.Invoke(slowSpead);
    }



    public void AddOnEnemySlow(UnityAction<float> action)
    {
        onEnemySlow.AddListener(action);
    }


    public void RemoveOnEnemySlow(UnityAction<float> action)
    {
        onEnemySlow.RemoveListener(action);
    }

    // プレイヤーが敵にぶつかった時の処理
    void OnTriggerEnter(Collider other)
    {
        MultiTag tag = other.GetComponent<MultiTag>();

        // 配列の中に同じものがあるかのチェック
        if (tag.HasTag(playerTag))
        {
            foreach (EnemyState enemy in enemies)
            {
                enemy.gameObject.SetActive(true);  // 登録された敵を有効化
            }
        }
    }



    // プレイヤーの位置を渡して、最も近い Enemy の位置を返す
    public Vector3 GetNearestEnemyPosition(Vector3 playerPosition)
    {
        EnemyState nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyState enemy in enemies)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        // 見つかった場合はその位置を返す。なければ Vector3.zero を返す（要調整可能）
        return nearest != null ? nearest.transform.position : Vector3.zero;
    }


    public void NearEnemyFlag(Vector3 playerPosition)
    {
        EnemyState nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyState enemy in enemies)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.SetEnemyAttackFlag(false);

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }
        // Nullチェックを追加
        if (nearest != null)
        {
            nearest.SetEnemyAttackFlag(true);
        }

    }


    public void EnemyFlagFalse()
    {
        foreach (EnemyState enemy in enemies)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.SetEnemyAttackFlag(false);
        }
    }
}
