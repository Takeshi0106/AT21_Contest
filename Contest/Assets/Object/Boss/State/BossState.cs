using UnityEngine;

// =================================
// BOSSの状態管理クラス
// =================================


public class BossState : EnemyBaseState<BossState>
{
    private BossStatusEffectManager m_BossStatusEffectManager = null; // Bossの状態管理クラス


    private void Start()
    {
        CharacterStart(); // キャラクター初期化
        EnemyStart(); // エネミークラス初期化

        // HPマネージャーにDie関数を渡す
        hpManager.SetOnDeathEvent(Die);

        // エネミーマネジャーにスローイベントをセット
        enemyManager.AddOnEnemySlow(SetEnemySpead);
        // 
        m_BossStatusEffectManager = this.gameObject.GetComponent<BossStatusEffectManager>();

        // 状態をセット
        currentState = new BossStandingState();
        // 状態の開始処理
        currentState.Enter(this);

        attackFlag = true;

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
        StateUpdate();

        // HandleDamage();
    }

    // ダメージ処理 Boss用
    public void HandleDamage()
    {
        damagerFlag = false;
        hitCounter = false;

        // 保存したコライダーのタグが元に戻る可のチェック
        // CleanupInvalidDamageColliders();

        for (int i = 0; i < collidedInfos.Count; i++)
        {
            var info = collidedInfos[i];

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
                if(attackInterface == null)
                {
                    Debug.Log("AttackInterface が見つかりません");
                }

                Debug.Log($"Enemyのダメージ: {attackInterface.GetOtherAttackDamage()}（{(isCounterAttack ? "カウンター" : "通常")}）");
                Debug.Log(Time.frameCount + ": Counter Hit!");
#endif

                // スタンダメージをあたえる
                m_BossStatusEffectManager.Damage(attackInterface.GetOtherStanAttackDamage());

                // スタン状態に移行するかのチェック
                if(m_BossStatusEffectManager.GetStanFlag())
                {
                    this.ChangeState(new BossStanState()); // スタン状態に移行
                }

                if (isCounterAttack) { hitCounter = true; }
                // ダメージをあたえる
                hpManager.TakeDamage(attackInterface.GetOtherAttackDamage());

                // エフェクト
                DamageEffect(info.collider);

                info.hitFlag = true;

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
        if(!hitCounter)
        {
            Debug.Log("カウンター以外で倒された");
        }
        Debug.Log($"{gameObject.name} が死亡しました");
#endif

        enemyRigidbody.useGravity = false; // 重力をOFFにする
        this.GetComponent<Collider>().enabled = false; // コライダーを無効にする

        ChangeState(new BossDeadState()); // Dead状態に変更
    }


    // ゲッター
    public BossStatusEffectManager GetBossStatusEffectManager() { return m_BossStatusEffectManager; }
}
