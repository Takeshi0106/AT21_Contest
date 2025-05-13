using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


// =====================================
// プレイヤーの状態を管理する
// =====================================

// Rigidbodyコンポーネントが必須
[RequireComponent(typeof(Rigidbody))]

// エディタ実行時に実行される
[RequireComponent(typeof(Renderer))]

#if UNITY_EDITOR

#endif

public class PlayerState : BaseCharacterState<PlayerState>
{
    // インスペクタービューから変更できる
    [Header("カメラオブジェクト名")]
    [SerializeField] private string cameraName = "Main Camera"; //カメラオブジェクト名
    [Header("プレイヤーの歩く移動速度")]
    [SerializeField] private float walkSpeed = 2.0f; //移動速度
    [Header("プレイヤーの歩く移動速度")]
    [SerializeField] private float dashSpeed = 4.0f; //移動速度
    [Header("プレイヤーがジャンプするときの力")]
    [SerializeField] private float jumpPower = 2.0f;
    [Header("プレイヤーのカウンター攻撃範囲オブジェクト")]
    [SerializeField] private GameObject playerCounterObject;
    [Header("カウンター可能な攻撃のタグ")]
    [SerializeField] private string counterPossibleAttack = "EnemyAttack";
    [Header("カウンター成功時の攻撃の大きさ")]
    [SerializeField] private float counterRange = 5.0f;
    [Header("敵の攻撃タグ名")]
    [SerializeField] private string enemyAttackTag = "EnemyAttack";
    [Header("プレイヤーがひるんだ時のフレーム数")]
    [SerializeField] private int flinchFreams = 0;
    [Header("プレイヤーが投げるのを失敗したときのフレーム数")]
    [SerializeField] private int ThrowFailedFreams = 0;
    [Header("プレイヤーが投げるのを失敗したときのアニメーション")]
    [SerializeField] private AnimationClip throwFailedAnimations = null;
    [Header("プレイヤーのスピード(デバッグ用)")]
    [SerializeField] private float playerSpeed = 1.0f;


    // カメラのトランスフォーム このスクリプト以外で変更できないように設定
    [HideInInspector] private Transform cameraTransform;
    // Playerのリジッドボディ
    [HideInInspector] private Rigidbody playerRigidbody;
    // Playerのコライダー
    [HideInInspector] private Collider playerCollider;
    // Playerのトランスフォーム
    [HideInInspector] private Transform playerTransform;
    // Playerのカウンターマネージャー
    [HideInInspector] private CounterManager playerCounterManager;
    // Playerのウェポンマネージャー
    [HideInInspector] private WeponManager playerWeponManager;
    // Playerのアニメーター
    [HideInInspector] private Animator playerAnimator;
    // カウンターのAttackController
    [HideInInspector] private AttackController playerCounterAttackController;
    // Playerの状態マネージャー
    [HideInInspector] private StatusEffectManager playerStatusEffectManager;
    // PlayerのHPマネージャー
    private HPManager hpManager;
    // PlayerのHPマネージャー
    private AvoidanceManager playerAvoidanceManager;


    // 現在のコンボ数
    private int playerConbo = 0;
    // 現在使っている武器のナンバー
    private int weponNumber = 0;
    // 空中に浮いているかのフラグ
    private bool isInAir = false;

    // 入力をスタックする
    RESEVEDSTATE nextReserved = RESEVEDSTATE.NOTHING;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // Playerのレンダラー
    [HideInInspector] public Renderer playerRenderer;
#endif



    // Start is called before the first frame update
    void Start()
    {
        //　カメラオブジェクトを代入
        cameraTransform = GameObject.Find(cameraName).transform;
        // Playerリジッドボディー
        playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        // Playerコライダー
        playerCollider = this.gameObject.GetComponent<Collider>();
        // Playerトランスフォーム
        playerTransform = this.gameObject.GetComponent<Transform>();
        // カウンターマネージャー
        playerCounterManager = this.gameObject.GetComponent<CounterManager>();
        // ウェポンマネージャー
        playerWeponManager = this.gameObject.GetComponent<WeponManager>();
        // アニメーター
        playerAnimator = this.gameObject.GetComponent<Animator>();
        // カウンターの攻撃コントローラー
        playerCounterAttackController = playerCounterObject.GetComponent<AttackController>();
        // HpManager
        hpManager = this.gameObject.GetComponent<HPManager>(); 
        // 状態管理
        playerStatusEffectManager = this.gameObject.GetComponent<StatusEffectManager>();
        // 回避管理
        playerAvoidanceManager = this.gameObject.GetComponent<AvoidanceManager>();

        // playerCounterObject.SetActive(false);
        // HPマネージャーにDie関数を渡す
        hpManager.SetOnDeathEvent(Die);


        // 状態をセット
        currentState = PlayerStandingState.Instance;
        // 状態の開始処理
        currentState.Enter(this);

#if UNITY_EDITOR

        // エディタ実行時に取得して色を変更する
        playerRenderer = this.gameObject.GetComponent<Renderer>();

        // 所得出来ていないときログを出す（エラーではなく情報）
        if (cameraTransform == null)
        {
            Debug.Log("PlayerState : カメラオブジェクトが見つかりません");
        }
        if (playerRigidbody == null)
        {
            Debug.Log("PlayerState : Rigidbodyが見つかりません");
        }
        if (playerCollider == null)
        {
            Debug.Log("PlayerState : Colliderが見つかりません");
        }
        if (playerTransform == null)
        {
            Debug.Log("PlayerState : Transformが見つかりません");
        }
        if (playerCounterManager == null)
        {
            Debug.Log("PlayerState : CounterManagerが見つかりません");
        }
        if (playerRigidbody == null)
        {
            Debug.Log("PlayerState : Rendererが見つかりません");
        }
        if (playerWeponManager == null)
        {
            Debug.Log("PlayerState : WeponManagerが見つかりません");
        }
        if (playerAnimator == null)
        {
            Debug.Log("PlayerState : PlayerAnimatorが見つかりません");
        }
        if (playerCounterAttackController == null)
        {
            Debug.Log("PlayerState : PlayerCounterAttackControllerが見つかりません");
        }
        if (hpManager == null)
        {
            Debug.Log("PlayerState : HPManagerが見つかりません");
        }
        if (playerStatusEffectManager == null)
        {
            Debug.Log("PlayerState : playerStatusEffectManagerが見つかりません");
        }
        if(playerAvoidanceManager == null)
        {
            Debug.Log("PlayerState : playerAvoidanceManagerが見つかりません");
        }
#endif
    }



    void Update()
    {
        // 空中に浮いているかの判定
        AirDetermine();
        // 状態を更新する
        StateUpdate();
        // 回避状態更新
        playerAvoidanceManager.AvoidUpdate();
        // カウンターランクが落ちる処理
        playerCounterManager.GaugeDecay();
    }



    // 空中に浮いているかの判定
    public void AirDetermine()
    {

        Ray ray = new Ray(this.transform.position + new Vector3(0.0f, 0.05f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f));
        Debug.DrawRay(this.transform.position + new Vector3(0.0f, 0.05f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), Color.red);
        RaycastHit hit;

        //レイ処理
        if (Physics.Raycast(ray, out hit, 5.0f))//10.0fと指定して最長を決めている
        {

            if (hit.distance < 0.6f)//誤差があるので0.05ではなく0.1にする
            {
                isInAir = false;
#if UNITY_EDITOR
                //Debug.Log("空中に浮いていない");
#endif
            }
            else
            {
                isInAir = true;
#if UNITY_EDITOR
                //Debug.Log("空中に浮ている");
#endif
            }
#if UNITY_EDITOR
            //Debug.Log($"Hit Distance: {hit.distance}");
#endif
        }
    }


    // ダメージ処理
    public void HandleDamage()
    {
        // 保存したコライダーのタグが元に戻る可のチェック
        CleanupInvalidDamageColliders();

        // プレイヤーが無敵状態か調べる
        if (playerStatusEffectManager.Invincible())
        {
#if UNITY_EDITOR
            playerRenderer.material.color = Color.yellow;
#endif
            // 無敵状態なら関数を終了する
            return;
        }
#if UNITY_EDITOR
        else
        {
            // 無敵終了後は色を元に戻す
            if (playerRenderer.material.color == Color.yellow)
            {
                playerRenderer.material.color = Color.white;
            }
        }
#endif


        // 当たっているオブジェクトのタグを調べる
        foreach (var info in collidedInfos)
        {
            // すでにダメージ処理済み,タグコンポーネントがnullならスキップ
            if (info.multiTag == null || damagedColliders.Contains(info.collider)) { continue; }

            // 敵の攻撃タグがあるかの判定
            if (info.multiTag.HasTag(enemyAttackTag))
            {
#if UNITY_EDITOR
                Debug.Log("ダメージ対象ヒット: " + info.collider.gameObject.name);
#endif

                // コライダーは記録
                damagedColliders.Add(info.collider);

                // 親オブジェクトから EnemyState を取得
                var enemyState = info.collider.GetComponentInParent<EnemyState>();

                if (enemyState != null)
                {
                    // ダメージ処理
                    hpManager.TakeDamage(enemyState.GetEnemyWeponManager().GetWeaponData(0).GetDamage(enemyState.GetEnemyConbo()));
                }

#if UNITY_EDITOR
                // ダメージ処理などをここに追加
                Debug.Log("HP " + hpManager.GetCurrentHP());
#endif
            }
        }
    }



    // 攻撃タグが元に戻るまで
    public void CleanupInvalidDamageColliders()
    {
        // タグが攻撃タグ以外の物かを調べる
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || !tag.HasTag(enemyAttackTag);
        });

        // コライダーが非アクティブ化を調べる
        damagedColliders.RemoveWhere(collider =>
        collider == null || !collider.gameObject.activeInHierarchy || !collider.enabled);

        // 当たっているオブジェクトが非アクティブかを調べる
        collidedInfos.RemoveAll(info =>
        info.collider == null ||
        !info.collider.gameObject.activeInHierarchy ||
        !info.collider.enabled);
    }



    private void Die()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameObject.SetActive(false);
        SceneManager.LoadScene("PlayerLoseScene");      
    }



    public void AddDamagedCollider(Collider target)
    {
        if (target != null && !damagedColliders.Contains(target))
        {
            damagedColliders.Add(target);
        }
    }



    // セッター
    public void SetPlayerCombo(int value) { playerConbo = value; }
    public void SetPlayerNextReseved(RESEVEDSTATE next) { nextReserved = next; }
    public void SetPlayerSpeed(float speed) { playerSpeed = speed; }



    // ゲッター
    public float GetWalkSpeed()                      { return walkSpeed; }
    public float GetDashSpeed()                      { return dashSpeed; }
    public float GetPlayerJumpPower() { return jumpPower; }
    public List<CollidedInfo> GetPlayerCollidedInfos() { return collidedInfos; }
    public Transform GetCameraTransform()            { return cameraTransform; }
    public Rigidbody GetPlayerRigidbody()            { return playerRigidbody; }
    public Collider GetPlayerCollider()              { return playerCollider; }
    public Transform GetPlayerTransform()            { return playerTransform; }
    public CounterManager GetPlayerCounterManager()  { return playerCounterManager; }
    public WeponManager GetPlayerWeponManager()      { return playerWeponManager; }
    public int GetPlayerConbo()                      { return playerConbo; }
    public int GetPlayerWeponNumber()                { return weponNumber; }
    public Animator GetPlayerAnimator() { return playerAnimator; }
    public GameObject GetPlayerCounterObject() { return playerCounterObject; }
    public string GetPlayerCounterPossibleAttack() { return counterPossibleAttack; }
    public RESEVEDSTATE GetPlayerNextReseved() { return nextReserved; }
    public AttackController GetPlayerCounterAttackController() { return playerCounterAttackController; }
    public float GetPlayerCounterRange() { return counterRange; }
    public HPManager GetPlayerHPManager() { return hpManager; }
    public string GetPlayerEnemyAttackTag() { return enemyAttackTag; }
    public StatusEffectManager GetPlayerStatusEffectManager() {  return playerStatusEffectManager; }
    public HashSet<Collider> GetPlayerDamagedColliders() { return damagedColliders; }
    public int GetPlayerFlinchFreams() { return flinchFreams; }
    public int GetThrowFailedFreams() { return ThrowFailedFreams; }
    public AnimationClip GetThrowFailedAnimation() { return throwFailedAnimations; }
    public bool GetPlayerAirFlag() { return isInAir; }
    public AvoidanceManager GetPlayerAvoidanceManager() { return playerAvoidanceManager; }
    public float GetPlayerSpeed() { return playerSpeed; }

#if UNITY_EDITOR
    // エディタ実行時に実行される
    public Renderer GetPlayerRenderer() { return playerRenderer; }
#endif
}

