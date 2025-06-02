using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// =================================
// BOSSの状態管理クラス
// =================================


public class BossState : EnemyBaseState<BossState>
{
    private BossStatusEffectManager m_BossStatusEffectManager = null; // Bossの状態管理クラス


    private void Start()
    {
        EnemyStart(); // エネミークラス初期化

        // HPマネージャーにDie関数を渡す
        hpManager.SetOnDeathEvent(Die);

        // エネミーマネジャーにスローイベントをセット
        enemyManager.AddOnEnemySlow(SetEnemySpead);
        // 
        m_BossStatusEffectManager = this.gameObject.GetComponent<BossStatusEffectManager>();

        // 状態をセット
        // currentState = new EnemyStandingState();
        // 状態の開始処理
        // currentState.Enter(this);

#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        enemyRenderer = this.gameObject.GetComponent<Renderer>();

        if (enemyWeponManager == null)
        {
            Debug.Log("EnemyState : WeponManagerが見つかりません");
            return;
        }
        if (m_BossStatusEffectManager == null)
        {
            Debug.Log("BossState : EffectManagerが見つかりません");
            return;
        }
#endif

        // エネミーオブジェクトに自分を渡す
        enemyManager.RegisterEnemy(this);

        SetEnemySpead(0.8f);

        this.gameObject.SetActive(false);
    }

    // 更新処理
    void Update()
    {
        /*
        if(m_BossStatusEffectManager.GetStanFlag())
        {
            // ChangeState()
        }
        */

        // StateUpdate();

        HandleDamage();
    }

    // ダメージ処理（通常攻撃＋カウンター攻撃対応）
    override public void HandleDamage()
    {
        damagerFlag = false;
        hitCounter = false;

        // 保存したコライダーのタグが元に戻る可のチェック
        CleanupInvalidDamageColliders();

        foreach (var info in collidedInfos)
        {
            // すでにダメージ処理済み,タグコンポーネントがnullならスキップ
            if (info.multiTag == null || damagedColliders.Contains(info.collider)) { continue; }

            // プレイヤーの攻撃タグがあるかを調べる
            if (info.multiTag.HasTag(playerAttackTag))
            {
                damagerFlag = true;

                // プレイヤーの基本ダメージを入れる変数
                float baseDamage = 0.0f;
                // プレイヤーの攻撃アップ倍率を入れる変数
                float multiplier = 1.0f;
                // プレイヤーのスタンゲージダメージ処理を入れる変数
                float stanDamage = 0.0f;
                // 最終ダメージを入れる変数
                float finalDamage = 0f;

                // カウンタータグがあるか調べる
                bool isCounterAttack = info.multiTag.HasTag(playerCounterTag);
                // 投げられたボブジェクトがを調べる
                bool isThrowAttack = info.multiTag.HasTag(playerThrowTag);

                // 一度ダメージ処理したコライダーを保存
                damagedColliders.Add(info.collider);

                // カウンターの場合の処理
                if (isCounterAttack)
                {
                    // ダメージを計算するためにパラメータを取得
                    baseDamage = playerState.GetPlayerCounterManager().GetCounterDamage();
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                    stanDamage = playerState.GetPlayerCounterManager().GetCounterStanDamage();

                    hitCounter = true;
                }
                // 投げる攻撃の場合の処理
                else if (isThrowAttack)
                {
                    ThrowObjectState throwState = info.collider.gameObject.GetComponentInParent<ThrowObjectState>();
                    baseDamage = throwState.GetThrowDamage(); // ダメージを取得
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                    stanDamage=throwState.GetThrowStanDamage(); // スタンダメージ取得
                }
                // 通常こうげきの場合の処理
                else
                {
                    // ダメージを計算するためにパラメータを取得
                    var weaponManager = playerState.GetPlayerWeponManager();
                    var weaponData = weaponManager.GetWeaponData(playerState.GetPlayerWeponNumber());

                    baseDamage = weaponData.GetDamage(playerState.GetPlayerConbo());
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                    stanDamage = weaponData.GetStanDamage(playerState.GetPlayerConbo());
                }

                // ダメージ計算
                finalDamage = baseDamage * multiplier;

#if UNITY_EDITOR
                Debug.Log($"Enemyのダメージ: {finalDamage}（{(isCounterAttack ? "カウンター" : "通常")}）");
                Debug.Log(Time.frameCount + ": Counter Hit!");
#endif

                // ダメージをあたえる
                hpManager.TakeDamage(finalDamage);
                // スタンダメージをあたえる
                m_BossStatusEffectManager.Damage(stanDamage);


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

        // ChangeState(new EnemyDeadState()); // Dead状態に変更
    }
}
