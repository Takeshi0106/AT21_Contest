using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// プレイヤーの状態
// =====================================

// Rigidbodyコンポーネントが必須
[RequireComponent(typeof(Rigidbody))]

#if UNITY_EDITOR
// エディタ実行時に実行される
[RequireComponent(typeof(Renderer))]
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

    // 衝突したオブジェクトを保存するリスト
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();

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

    // 現在のコンボ数
    private int playerConbo = 0;
    // 現在使っている武器のナンバー
    private int weponNumber = 0;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // Playerのレンダラー
    [HideInInspector] public Renderer playerRenderer;
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

#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        playerRenderer = this.gameObject.GetComponent<Renderer>();


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
    void OnCollisionEnter(Collision other)
    {
        Collider collider = other.collider;
        if (!collidedObjects.Contains(other.collider))
        {
            collidedObjects.Add(other.collider);
        }
#if UNITY_EDITOR
        // エディタ実行時に実行される
        Debug.Log("Objectに当たった : " + other.gameObject.name);
#endif
    }



    // プレイヤーが敵と離れた時の処理
    void OnCollisionExit(Collision other)
    {
        Collider collider = other.collider;
        if (collidedObjects.Contains(other.collider))
        {
            collidedObjects.Remove(other.collider);
        }
#if UNITY_EDITOR
        // エディタ実行時に実行される
        Debug.Log("Objectが離れた : " + other.gameObject.name);
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
    public List<Collider> GetPlayerCollidedObjects() { return collidedObjects; }
    public Transform GetCameraTransform()            { return cameraTransform; }
    public Rigidbody GetPlayerRigidbody()            { return playerRigidbody; }
    public Collider GetPlayerCollider()              { return playerCollider; }
    public Transform GetPlayerTransform()            { return playerTransform; }
    public CounterManager GetPlayerCounterManager()  { return playerCounterManager; }
    public WeponManager GetPlayerWeponManager()      { return playerWeponManager; }
    public int GetPlayerConbo()                      { return playerConbo; }
    public int GetPlayerWeponNumber()                { return weponNumber; }
#if UNITY_EDITOR
    // エディタ実行時に実行される
    public Renderer GetPlayerRenderer()              { return playerRenderer; }
#endif
}

