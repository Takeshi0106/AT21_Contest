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

    [Header("デバッグ用　敵の速度(0.01～1.00)")]
    [SerializeField] private float enemySpeed = 1.0f;
    [Header("怯み時間")]
    [SerializeField] private int flinchFreams = 10;

    [Header("立ち状態アニメーション")]
    [SerializeField] private AnimationClip enemyStandingAnimation = null;
    [Header("死亡アニメーション")]
    [SerializeField] private AnimationClip enemyDeadAnimation = null;


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
    // Enemyのリジッドボディー
    private Rigidbody enemyRigidbody;
    // Enemyのアニメーションを取得する
    private Animator enemyAnimator;


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



    // 初期化処理
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
        // リジッドボディーを取得
        enemyRigidbody = this.gameObject.GetComponent<Rigidbody>();
        // アニメーターを取得
        enemyAnimator = this.gameObject.GetComponent<Animator>();


        // HPマネージャーにDie関数を渡す
        hpManager.SetOnDeathEvent(Die);
        
        // エネミーマネジャーにスローイベントをセット
        enemyManager.AddOnEnemySlow(SetEnemySpead);



#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        enemyRenderer = this.gameObject.GetComponent<Renderer>();

        if (enemyWeponManager == null)
        {
            Debug.Log("EnemyState : WeponManagerが見つかりません");
            return;
        }
#endif

        // エネミーオブジェクトに自分を渡す
        enemyManager.RegisterEnemy(this);

        this.gameObject.SetActive(false);

        SetEnemySpead(0.8f);
    }


    // 更新処理
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
        
        ChangeState(EnemyDeadState.Instance); // Dead状態に変更
    }



    // スピードをセットする処理
    public void SetEnemySpead(float speed)
    {
        if (speed >= 0.01f && speed <= 1.0f)
        {
            // フレームの進む処理を更新
            enemySpeed = speed;
            // アニメーションの速度を変更
            if (enemyAnimator != null)
            {
                enemyAnimator.speed = speed;
            }
            else
            {
                enemyWeponManager.GetCurrentWeaponAnimator().speed = speed;
            }
        }
        else
        {
            Debug.Log("スピードがセットできませんでした。");
        }
    }



    // セッター
    public void SetEnemyCombo(int combo) { enemyConbo = combo; }

    // ゲッター
    public  List<Collider>  GetEnemyCollidedObjects()  { return collidedObjects; }
    public  WeponManager    GetEnemyWeponManager()     { return enemyWeponManager; }
    public  int             GetEnemyConbo()            { return enemyConbo; }
    public  int             GetEnemyWeponNumber()      { return weponNumber; }
    public BaseAttackData GetDropWeapon() { return dropWeapon; }
    public string GetEnemyPlayerAttackTag() { return playerAttackTag; }
    public string GetEnemyPlayerCounterAttackTag() { return playerCounterTag; }
    public float GetEnemySpeed() { return enemySpeed; }
    public int GetEnemyFlinchFreams() { return flinchFreams; }
    public Animator GetEnemyAnimator() { return enemyAnimator; }
    public AnimationClip GetEnemyStandingAnimation() { return enemyStandingAnimation; }
    public AnimationClip GetEnemyDeadAnimation() { return enemyDeadAnimation; }

#if UNITY_EDITOR
    // エディタ実行時に実行される
    public  Renderer        GetEnemyRenderer()         { return enemyRenderer; }
#endif
}
