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
    [Header("怯みアニメーション")]
    [SerializeField] private AnimationClip enemyFlinchAnimation = null;
    [Header("走るアニメーション")]
    [SerializeField] private AnimationClip enemyDashAnimation = null;

    [Header("敵の走る移動速度")]
    [SerializeField] private float dashSpeed = 4.0f; //移動速度

    [Header("攻撃する距離")]
    [SerializeField] private float attackDistance = 1.0f; //移動速度

    [Header("視野角")]
    [SerializeField] private float fov;
    [Header("視野の長さ")]
    [SerializeField] private float visionLength;
    [Header("攻撃レンジ")]
    [SerializeField] private float attackRange;
    [Header("移動速度")]
    [SerializeField] private float moveSpeed;

    //プレイヤーを発見したかのフラグ
    private bool foundTargetFlg;


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

    private Transform playerTransform;


    // 現在のコンボ数
    private int enemyConbo = 0;
    // 現在使っている武器のナンバー
    private int weponNumber = 0;
    // カウンター攻撃で倒れたかのチェック
    private bool hitCounter = false;
    //
    private bool damagerFlag = false;
    //
    private int flinchCnt = 0;

    private bool attackFlag = false;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // Enemyのレンダラー
    [HideInInspector] public Renderer enemyRenderer;
#endif



    // 初期化処理
    void Start()
    {
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

        playerTransform = player.transform;

        // HPマネージャーにDie関数を渡す
        hpManager.SetOnDeathEvent(Die);
        
        // エネミーマネジャーにスローイベントをセット
        enemyManager.AddOnEnemySlow(SetEnemySpead);

        // 状態をセット
        currentState = new EnemyStandingState();
        // 状態の開始処理
        currentState.Enter(this);

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

        SetEnemySpead(0.8f);

        this.gameObject.SetActive(false);
    }


    // 更新処理
    void Update()
    {
        StateUpdate();
    }


    // ダメージ処理（通常攻撃＋カウンター攻撃対応）
    public void HandleDamage()
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
        
        ChangeState(new EnemyDeadState()); // Dead状態に変更
    }

    public void Target()
    {
        Vector3 direction = player.transform.position - transform.position;

        // Y方向の回転だけにしたい場合（上下は無視）
        direction.y = 0;

        // 向きたい方向が0ベクトルでないことを確認
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
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
        }
        else
        {
            Debug.Log("スピードがセットできませんでした。");
        }
    }


    // セッター
    public void SetEnemyCombo(int combo) { enemyConbo = combo; }
    public void SetEnemyFlinchCnt(int cnt) { flinchCnt = cnt; }
    public void SetEnemyAttackFlag(bool flag) { attackFlag = flag; }
    public void SetFoundTargetFlg(bool _foundTargetFlg) { foundTargetFlg = _foundTargetFlg; }

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
    public AnimationClip GetEnemyFlinchAnimation() { return enemyFlinchAnimation; }
    public bool GetEnemyDamageFlag() { return damagerFlag; }
    public bool GetEnemyHitCounterFlag() { return hitCounter; }
    public int GetEnemyFlinchCnt() { return flinchCnt; }
    public PlayerState GetPlayerState() { return playerState; }
    public float GetEnemyDashSpeed() { return dashSpeed; }
    public Rigidbody GetEnemyRigidbody() { return enemyRigidbody; }
    public bool GetEnemyAttackFlag() { return attackFlag; }
    public Transform GetPlayerTransform() { return playerTransform; }
    public AnimationClip GetEnemyDashAnimation() { return enemyDashAnimation; }
    public float GetDistanceAttack() { return attackDistance; }

    public float GetEnemyFov() { return fov; }
    public float GetEnemyVisionLength() { return visionLength; }
    public float GetEnemyAttackRange() { return attackRange; }
    public float GetEnemyMoveSpeed() { return moveSpeed; }
    public EnemyManager GetEnemyManager() { return enemyManager; }
    public bool GetFoundTargetFlg() { return foundTargetFlg; }
    public GameObject GetTargetObject() { return player; }

#if UNITY_EDITOR
    // エディタ実行時に実行される
    public  Renderer        GetEnemyRenderer()         { return enemyRenderer; }
#endif
}
