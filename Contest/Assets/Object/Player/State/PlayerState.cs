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

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // Playerのレンダラー
    [HideInInspector] public Renderer playerRenderer;
#endif

    // Start is called before the first frame update
    void Start()
    {
        // 状態をセット
        currentState = StandingState.Instance;

        // 状態の開始処理
        currentState.Enter(this);

        //　カメラオブジェクトを代入
        cameraTransform = GameObject.Find(cameraName).transform;
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerState : カメラオブジェクトが見つかりません");
            return;
        }

        // Playerリジッドボディー
        playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Rigidbodyが見つかりません");
            return;
        }

        // Playerコライダー
        playerCollider = this.gameObject.GetComponent<Collider>();
        if (playerCollider == null)
        {
            Debug.LogError("PlayerState : Colliderが見つかりません");
            return;
        }

        // Playerトランスフォーム
        playerTransform = this.gameObject.GetComponent<Transform>();
        if (playerTransform == null)
        {
            Debug.LogError("PlayerState : Transformが見つかりません");
            return;
        }

        // カウンターマネージャー
        playerCounterManager = this.gameObject.GetComponent<CounterManager>();
        if(playerCounterManager == null)
        {
            Debug.LogError("PlayerState : CounterManagerが見つかりません");
            return;
        }

#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        playerRenderer = this.gameObject.GetComponent<Renderer>();
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Rendererが見つかりません");
            return;
        }
#endif
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



    // ゲッター
    public float GetWalkSpeed() => walkSpeed;
    public float GetDashSpeed() => dashSpeed;
    public List<Collider> GetCollidedObjects() => collidedObjects;
    public Transform GetCameraTransform() => cameraTransform;
    public Rigidbody GetPlayerRigidbody() => playerRigidbody;
    public Collider GetPlayerCollider() => playerCollider;
    public Transform GetPlayerTransform() => playerTransform;
    public CounterManager GetPlayerCounterManager() => playerCounterManager;
#if UNITY_EDITOR
    // エディタ実行時に実行される
    public Renderer GetPlayerRenderer() => playerRenderer;
#endif
}

