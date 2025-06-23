using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// エネミーの状態
// =====================================

public class EnemyState : EnemyBaseState<EnemyState>
{
    DamageResponseManager enemyDamageResponseManager;

    // 初期化処理
    void Start()
    {
        CharacterStart(); // キャラクター初期化
        EnemyStart(); // エネミークラス初期化

        enemyDamageResponseManager = this.gameObject.GetComponent<DamageResponseManager>();

        // HPマネージャーにDie関数を渡す
        hpManager.SetOnDeathEvent(Die);

        // エネミーマネジャーにスローイベントをセット
        enemyManager.AddOnEnemySlow(SetEnemySpead);

        // 状態をセット
        currentState = new EnemyStandingState();
        // 状態の開始処理
        currentState.Enter(this);
        // エネミーオブジェクトに自分を渡す
        enemyManager.RegisterEnemy(this);

        SetEnemySpead(0.8f);

        this.gameObject.SetActive(false);
    }



    // 更新処理
    void Update()
    {
        StateUpdate();
        enemyDamageResponseManager.FlinchUpdate(enemySpeed);
    }



    // ダメージ処理（通常攻撃＋カウンター攻撃対応）
    virtual public void HandleDamage()
    {
        damagerFlag = false;
        hitCounter = false;

        for (int i = 0; i < collidedInfos.Count; i++)
        { 
            var info = collidedInfos[i];

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log($"{info.collider.name} :  HitFlag :{info.hitFlag} / HitID{info.hitID}");
            }
#endif

            // すでにダメージ処理済み,タグコンポーネントがnullならスキップ
            if (info.multiTag == null || info.hitFlag) { continue; }

            // プレイヤーの攻撃タグがあるかを調べる
            if (info.multiTag.HasTag(playerAttackTag))
            {
                damagerFlag = true;

                // カウンタータグがあるか調べる
                bool isCounterAttack = info.multiTag.HasTag(playerCounterTag);
                // 投げられたボブジェクトがを調べる
                bool isThrowAttack = info.multiTag.HasTag(playerThrowTag);

                // 一度ダメージ処理したコライダーを保存
                // damagedColliders.Add(info.collider);

                var attackInterface = info.collider.GetComponentInParent<AttackInterface>();

#if UNITY_EDITOR

                Debug.Log($"Enemyのダメージ: {attackInterface.GetOtherAttackDamage()}（{(isCounterAttack ? "カウンター" : "通常")}）");
                Debug.Log(Time.frameCount + ": Counter Hit!");
#endif
                // 怯み処理
                if (enemyDamageResponseManager.FlinchDamage(attackInterface.GetOtherStanAttackDamage()))
                {
                    ChangeState(new EnemyFlinchState());
                }
                // ダメージをあたえる
                hpManager.TakeDamage(attackInterface.GetOtherAttackDamage());

                // ダメージ処理をした
                info.hitFlag = true;

                if(isCounterAttack) { hitCounter = true; }

                // attackInterface.HitAttack();

                break; // 一度ヒットで処理終了
            }
        }
    }



    protected void Die()
    {
        enemyManager.RemoveOnEnemySlow(SetEnemySpead);
        // EnemyManagerに自分が倒れたことを知らせる
        enemyManager.UnregisterEnemy(this);

        if (playerState != null && dropWeapon != null && hitCounter)
        {
            // Playerに武器を渡す
            playerState.GetPlayerWeponManager().AddWeapon(dropWeapon);
        }

#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} が死亡しました");
#endif

        enemyRigidbody.useGravity = false; // 重力をOFFにする
        this.GetComponent<Collider>().enabled = false; // コライダーを無効にする

        ChangeState(new EnemyDeadState()); // Dead状態に変更
    }

    // ゲッター
    public DamageResponseManager GetEnemyDamageResponseManager() { return enemyDamageResponseManager; }
}
