using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ===============================
// EnemyManager
// 当たったらEnemyを呼び出す
// ===============================

public class EnemyManager : MonoBehaviour
{
    private List<EnemyBaseState<EnemyState>> enemyList = new List<EnemyBaseState<EnemyState>>();
    private List<EnemyBaseState<BossState>> bossList = new List<EnemyBaseState<BossState>>();
    private UnityEvent<float> onEnemySlow = new UnityEvent<float>() { };
    EnemySystem enemyManager = null;

    [Header("プレイヤーのタグ名")]
    [SerializeField] private string playerTag = "Player";
    [Header("敵のsystem")]
    [SerializeField] private EnemySystem system = null;


    // 開始処理
    public void Start()
    {
        // EnemySystemに自分を渡す
        enemyManager = system.GetComponent<EnemySystem>();
        enemyManager.RegisterEnemyManager(this);
    }


    // Enemyを取得
    public void RegisterEnemy(EnemyBaseState<EnemyState> enemy)
    {
        // Enemyを追加する
        enemyList.Add(enemy);
    }
    // Bossを取得
    public void RegisterEnemy(EnemyBaseState<BossState> boss)
    {
        // Enemyを追加する
        bossList.Add(boss);
    }


    // Enemyを倒したときの処理
    public void UnregisterEnemy(EnemyBaseState<EnemyState> enemy)
    {
        enemyList.Remove(enemy);

        if (enemyList.Count + bossList.Count == 0)
        {
            // 敵が倒されたらEnemySystemに知らせる
            enemyManager.UnregisterEnemyManager(this);
        }
    }
    // Bossを倒したときの処理
    public void UnregisterEnemy(EnemyBaseState<BossState> boss)
    {
        bossList.Remove(boss);

        if (enemyList.Count + bossList.Count == 0)
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


    // Enemyのスピード低下関数追加
    public void AddOnEnemySlow(UnityAction<float> action)
    {
        onEnemySlow.AddListener(action);
    }
    // Enemyのスピード低下関数削除
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
            // Enemyを有効化
            foreach (EnemyBaseState<EnemyState> enemy in enemyList)
            {
                enemy.gameObject.SetActive(true);  // 登録された敵を有効化
            }
            // Bossを有効化
            foreach (EnemyBaseState<BossState> boss in bossList)
            {
                boss.gameObject.SetActive(true);  // 登録された敵を有効化
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        MultiTag tag = other.GetComponent<MultiTag>();

        if (tag != null && tag.HasTag(playerTag))
        {
            // Enemyを無効化
            foreach (EnemyBaseState<EnemyState> enemy in enemyList)
            {
                if (enemy != null)
                {
                    enemy.ResetEnemy();
                    enemy.gameObject.SetActive(false);
                }
            }

            Debug.Log("プレイヤーがエリア外に出たため、敵を非アクティブにしました。");
        }
    }


    // プレイヤーの位置を渡して、最も近い Enemy の位置を返す
    public Vector3 GetNearestEnemyPosition(Vector3 playerPosition)
    {
        // BossとEnemyの一番近い物を入れる
        EnemyBaseState<EnemyState> enemyNearest = null;
        EnemyBaseState<BossState> bossNearest = null;
        float shortestDistance = float.MaxValue;

        // 一番近いEnemyを探る
        foreach (EnemyBaseState<EnemyState> enemy in enemyList)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                enemyNearest = enemy;
            }
        }
        // 一番近いBossを探る
        foreach (EnemyBaseState<BossState> boss in bossList)
        {
            if (boss == null || !boss.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(playerPosition, boss.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                enemyNearest = null;
                bossNearest = boss;
            }
        }

        // 見つかった場合はその位置を返す。
        if (enemyNearest != null)
        {
            return enemyNearest.transform.position;
        }
        else if(bossNearest != null)
        {
            return bossNearest.gameObject.transform.position;
        }

        return Vector3.zero;
    }


    // 一番近いEnemy・Bossにフラグを渡す
    public void NearEnemyFlag(Vector3 playerPosition)
    {
        EnemyBaseState<EnemyState> enemyNearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyBaseState<EnemyState> enemy in enemyList)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.SetEnemyAttackFlag(false);

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                enemyNearest = enemy;
            }
        }


        // Nullチェックを追加
        if (enemyNearest != null)
        {
            enemyNearest.SetEnemyAttackFlag(true);
        }
    }


    public void EnemyFlagFalse()
    {
        foreach (EnemyBaseState<EnemyState> enemy in enemyList)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.SetEnemyAttackFlag(false);
        }
    }
}
