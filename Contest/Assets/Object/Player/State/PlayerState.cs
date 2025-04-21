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

public class PlayerState : BaseState<PlayerState>
{
    // インスペクタービューから変更できる
    [Header("カメラオブジェクト名")]
    [SerializeField] private string cameraName = "Main Camera"; //カメラオブジェクト名
    [Header("プレイヤーの歩く移動速度")]
    [SerializeField] private float walkSpeed = 2.0f; //移動速度
    [Header("プレイヤーの歩く移動速度")]
    [SerializeField] private float dashSpeed = 4.0f; //移動速度
    [Header("プレイヤーのカウンター攻撃範囲オブジェクト")]
    [SerializeField] private GameObject playerCounterObject;
    [Header("カウンター可能な攻撃のタグ")]
    [SerializeField] private string counterPossibleAttack = "EnemyAttack";
    [Header("カウンター成功時の攻撃の大きさ")]
    [SerializeField] private float counterRange = 5.0f;
    [Header("敵の攻撃タグ名")]
    [SerializeField] private string enemyAttackTag = "EnemyAttack";
    [Header("プレイヤーのカウンター成功後の無敵時間（カウンター成功中の無敵時間とは別）")]
    [SerializeField] private int invincibleTime = 0;


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


    // 現在のコンボ数
    private int playerConbo = 0;
    // 現在使っている武器のナンバー
    private int weponNumber = 0;

    // 入力をスタックする
    RESEVEDSTATE nextReserved = RESEVEDSTATE.NOTHING;

    // エディタ実行時に実行される
    // Playerのレンダラー
    [HideInInspector] public Renderer playerRenderer;

#if UNITY_EDITOR


#endif

    // Start is called before the first frame update
    void Start()
    {
        // 状態をセット
        currentState = PlayerStandingState.Instance;

        // 状態の開始処理
        currentState.Enter(this);

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

        playerCounterObject.SetActive(false);

        // エディタ実行時に取得して色を変更する
        playerRenderer = this.gameObject.GetComponent<Renderer>();

        // HPマネージャーにDie関数を渡す
        hpManager.onDeath.AddListener(Die);

#if UNITY_EDITOR


        // 所得出来ていないときエラーを出す
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerState : カメラオブジェクトが見つかりません");
            return;
        }
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Rigidbodyが見つかりません");
            return;
        }
        if (playerCollider == null)
        {
            Debug.LogError("PlayerState : Colliderが見つかりません");
            return;
        }
        if (playerTransform == null)
        {
            Debug.LogError("PlayerState : Transformが見つかりません");
            return;
        }
        if (playerCounterManager == null)
        {
            Debug.LogError("PlayerState : CounterManagerが見つかりません");
            return;
        }
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Rendererが見つかりません");
            return;
        }
        if(playerWeponManager==null)
        {
            Debug.Log("PlayerState : WeponManagerが見つかりません");
            return;
        }
        if (playerAnimator == null)
        {
            Debug.Log("PlayerState : PlayerAnimatorが見つかりません");
            return;
        }
        if (playerCounterAttackController == null)
        {
            Debug.Log("PlayerState : PlayerCounterAttackControllerが見つかりません");
            return;
        }
        if (hpManager == null)
        {
            Debug.Log("PlayerState : HPManagerが見つかりません");
            return;
        }
        if (playerStatusEffectManager == null)
        {
            Debug.Log("PlayerState : playerStatusEffectManagerが見つかりません");
            return;
        }
#endif
    }



    void Update()
    {
        // 状態を更新する
        StateUpdate();
        // カウンターランクが落ちる処理
        playerCounterManager.GaugeDecay();
    }



    // ダメージ処理
    public void HandleDamage(string getAttackTags)
    {
        if (!playerStatusEffectManager.Invincible(invincibleTime))
        {
            playerRenderer.material.color = Color.white;

            foreach (var info in collidedInfos)
            {
                // すでにダメージ処理済み、またはタグがないならスキップ
                if (damagedColliders.Contains(info.collider))
                    continue;

                if (info.multiTag != null && info.multiTag.HasTag(getAttackTags))
                {
                    // ダメージ処理などをここに追加
                    Debug.Log("ダメージ対象ヒット: " + info.collider.gameObject.name);

                    // 一度ダメージを与えたら、このコライダーは記録
                    damagedColliders.Add(info.collider);

                    // 親オブジェクトから EnemyState を取得
                    var enemyState = info.collider.GetComponentInParent<EnemyState>();

                    if (enemyState != null)
                    {
                        hpManager.TakeDamage(enemyState.GetEnemyWeponManager().GetWeaponData(0).GetDamage(enemyState.GetEnemyConbo()));
                    }

                    // ダメージ処理などをここに追加
                    Debug.Log("HP " + hpManager.GetCurrentHP());

                    // 一つ当たったら抜けるなら break（複数なら continue）
                    break;
                }
            }
        }
#if UNITY_EDITOR
        else
        {
            playerRenderer.material.color = Color.yellow;

        }
#endif

        CleanupInvalidDamageColliders(getAttackTags);
    }



    // 攻撃タグが元に戻るまで
    public void CleanupInvalidDamageColliders(string getAttackTags)
    {
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || !tag.HasTag(getAttackTags);
        });
    }



    private void Die()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameObject.SetActive(false);
        SceneManager.LoadScene("ResultScene");
    }



    // セッター
    public void SetPlayerCombo(int value) { playerConbo = value; }
    public void SetPlayerNextReseved(RESEVEDSTATE next) { nextReserved = next; }



    // ゲッター
    public float GetWalkSpeed()                      { return walkSpeed; }
    public float GetDashSpeed()                      { return dashSpeed; }
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
    // エディタ実行時に実行される
    public Renderer GetPlayerRenderer() { return playerRenderer; }

#if UNITY_EDITOR

#endif
}

