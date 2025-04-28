using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// エネミーの状態
// =====================================

public class EnemyState : BaseCharacterState<EnemyState>
{
    [Header("Playerの攻撃タグ名")]
    [SerializeField] private string playerAttackTag = "PlayerAttack";
    [Header("Playerのカウンタータグ")]
    [SerializeField] private string playerCounterTag = "CounterAttack";
    [Header("Playerの投げるタグ")]
    [SerializeField] private string playerThrowTag = "ThrowAttack";

    [Header("Playerに倒されたときに渡す武器")]
    [SerializeField] private BaseAttackData dropWeapon;
    [Header("倒した時に武器を渡すプレイヤーの名前")]
    [SerializeField] private GameObject player;
    [Header("敵を管理するマネージャーの名前")]
    [SerializeField] private GameObject enemyManagerObject;

    // 衝突したオブジェクトを保存するリスト
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();
    // Enemyのウェポンマネージャー
    [HideInInspector] private WeponManager enemyWeponManager;
    // PlayerのState
    [HideInInspector] private PlayerState playerState;
    // PlayerのState
    [HideInInspector] private EnemyManager enemyManager;
    // EnemyのHPマネージャー 
    private HPManager hpManager;

    // 現在のコンボ数
    private int enemyConbo = 0;
    // 現在使っている武器のナンバー
    private int weponNumber = 0;
    // カウンター攻撃で倒れたかのチェック
    private bool hitCounter = false;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // Enemyのレンダラー
    [HideInInspector] public Renderer enemyRenderer;
#endif

    // Start is called before the first frame update
    void Start()
    {
        // 状態をセット
        currentState = EnemyStandingState.Instance;

        // 状態の開始処理
        currentState.Enter(this);

        // ウェポンマネージャー
        enemyWeponManager = this.gameObject.GetComponent<WeponManager>();
        // hpManager
        hpManager = this.gameObject.GetComponent<HPManager>();
        // PlayerState
        playerState = player.GetComponent<PlayerState>();
        // エネミーマネージャー
        enemyManager = enemyManagerObject.GetComponent<EnemyManager>();

        // HPマネージャーにDie関数を渡す
        hpManager.onDeath.AddListener(Die);
        // エネミーオブジェクトに自分を渡す
        enemyManager.RegisterEnemy(this);


#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        enemyRenderer = this.gameObject.GetComponent<Renderer>();

        if (enemyWeponManager == null)
        {
            Debug.Log("EnemyState : WeponManagerが見つかりません");
            return;
        }
#endif
    }



    void Update()
    {
        StateUpdate();
    }



    // ダメージ処理（通常攻撃＋カウンター攻撃対応）
    public void HandleDamage()
    {
        // 保存したコライダーのタグが元に戻る可のチェック
        CleanupInvalidDamageColliders();

        foreach (var info in collidedInfos)
        {
            // すでにダメージ処理済み,タグコンポーネントがnullならスキップ
            if (info.multiTag == null || damagedColliders.Contains(info.collider)) { continue; }

            // プレイヤーの攻撃タグがあるかを調べる
            if (info.multiTag.HasTag(playerAttackTag))
            {
                // プレイヤーの基本ダメージを入れる変数
                float baseDamage = 0.0f;
                // プレイヤーの攻撃アップ倍率を入れる変数
                float multiplier = 1.0f;
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
                    baseDamage = playerState.GetPlayerCounterManager().GetCounterDamage();
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                    hitCounter = true;
                }
                // 投げる攻撃の場合の処理
                else if(isThrowAttack)
                {
                    var weaponManager = playerState.GetPlayerWeponManager();
                    var weaponData = weaponManager.GetWeaponData(playerState.GetPlayerWeponNumber());

                    baseDamage = weaponData.GetThrowDamage();
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                }
                // 通常こうげきの場合の処理
                else
                {
                    var weaponManager = playerState.GetPlayerWeponManager();
                    var weaponData = weaponManager.GetWeaponData(playerState.GetPlayerWeponNumber());

                    baseDamage = weaponData.GetDamage(playerState.GetPlayerConbo());
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                }

                // ダメージ計算
                finalDamage = baseDamage * multiplier;

#if UNITY_EDITOR
                Debug.Log($"Enemyのダメージ: {finalDamage}（{(isCounterAttack ? "カウンター" : "通常")}）");
                Debug.Log(Time.frameCount + ": Counter Hit!");
#endif

                // ダメージをあたえる
                hpManager.TakeDamage(finalDamage);



                break; // 一度ヒットで処理終了
            }
        }
    }




    // 攻撃タグまたはカウンタータグが外れたら削除
    public void CleanupInvalidDamageColliders()
    {
        // タグが攻撃タグ以外の物かを調べる
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || (!tag.HasTag(playerAttackTag));
        });

        // コライダーが非アクティブ化を調べる
        damagedColliders.RemoveWhere(collider =>
        collider == null || !collider.gameObject.activeInHierarchy || !collider.enabled);

        // 無効なコライダーや非アクティブ化されたものも除外
        collidedInfos.RemoveAll(info =>
            info.collider == null ||
            !info.collider.gameObject.activeInHierarchy ||
            !info.collider.enabled);
    }



    private void Die()
    {
        // EnemyManagerに自分が倒れたことを知らせる
        enemyManager.UnregisterEnemy(this);
        // 攻撃タグを元に戻す
        enemyWeponManager.DisableAllWeaponAttacks();

        if (playerState != null && dropWeapon != null && hitCounter)
        {
            // Playerに武器を渡す
            playerState.GetPlayerWeponManager().AddWeapon(dropWeapon);
        }

        // 自分を非アクティブにする
        gameObject.SetActive(false);

#if UNITY_EDITOR
        Debug.Log($"{gameObject.name} が死亡しました");
#endif

    }



    // セッター
    public void SetEnemyCombo(int value) { enemyConbo = value; }

    // ゲッター
    public  List<Collider>  GetEnemyCollidedObjects()  { return collidedObjects; }
    public  WeponManager    GetEnemyWeponManager()     { return enemyWeponManager; }
    public  int             GetEnemyConbo()            { return enemyConbo; }
    public  int             GetEnemyWeponNumber()      { return weponNumber; }
    public BaseAttackData GetDropWeapon() { return dropWeapon; }
    public string GetEnemyPlayerAttackTag() { return playerAttackTag; }
    public string GetEnemyPlayerCounterAttackTag() { return playerCounterTag; }
#if UNITY_EDITOR
    // エディタ実行時に実行される
    public  Renderer        GetEnemyRenderer()         { return enemyRenderer; }
#endif
}
