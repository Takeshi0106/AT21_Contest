using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    // 接触したオブジェクトの情報を入れる
    public struct CollidedInfo
    {
        public Collider collider;
        public MultiTag multiTag;

        public CollidedInfo(Collider collider, MultiTag multiTag)
        {
            this.collider = collider;
            this.multiTag = multiTag;
        }
    }

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

    // 接触中の情報を入れる配列（Collider + MultiTag）
    private List<CollidedInfo> collidedInfos = new List<CollidedInfo>();

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
    

    // 現在のコンボ数
    private int playerConbo = 0;
    // 現在使っている武器のナンバー
    private int weponNumber = 0;
    // 入力をスタックする
    bool nextAttackReserved = false;

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

        // エディタ実行時に取得して色を変更する
        playerRenderer = this.gameObject.GetComponent<Renderer>();


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
#endif
    }



    void Update()
    {
        // 状態を更新する
        StateUpdate();
        // カウンターランクが落ちる処理
        playerCounterManager.GaugeDecay();
    }



    // プレイヤーが敵にぶつかった時の処理
    void OnTriggerEnter(Collider other)
    {
        // 配列の中に同じものがあるかのチェック
        if (!collidedInfos.Any(info => info.collider == other))
        {
            // MulltiTagを取得する
            MultiTag multiTag = other.GetComponent<MultiTag>();
            // 配列に追加
            collidedInfos.Add(new CollidedInfo(other, multiTag));
        }

#if UNITY_EDITOR
        Debug.Log("Triggerに当たった : " + other.gameObject.name);
#endif
    }



    // プレイヤーが敵と離れた時の処理
    void OnTriggerExit(Collider other)
    {
        // 配列から同じ物を探す
        collidedInfos.RemoveAll(info => info.collider == other);

#if UNITY_EDITOR
        Debug.Log("Triggerから離れた : " + other.gameObject.name);
#endif
    }



    // セッター
    public void SetPlayerCombo(int value)
    {
        playerConbo = value;
    }

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
    public bool GetPlayerNextAttackReseved() { return nextAttackReserved; }
    public void SetPlayerNextAttackReseved(bool next) { nextAttackReserved = next; }
#if UNITY_EDITOR
    // エディタ実行時に実行される
    public Renderer GetPlayerRenderer()              { return playerRenderer; }
#endif
}

