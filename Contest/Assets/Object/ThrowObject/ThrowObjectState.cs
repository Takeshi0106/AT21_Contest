using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectState : BaseColliderState<ThrowObjectState>
{
    // インスペクタービューから変更できる
    [Header("投げる力を代入する")]
    [SerializeField] private float forcePower;
    [Header("投げた時の重力無効フレーム")]
    [SerializeField] private int noGravityFreams = 0;


    // プレイヤーのトランスフォームを入れる変数
    private Transform cameraTransform;
    // 自分のリジッドボディーを取得しておく
    private Rigidbody throwObjectRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        // リジッドボディーを取得
        throwObjectRigidbody = this.gameObject.GetComponent<Rigidbody>();


        // 状態をセット
        currentState = ThrowObjectThrowState.Instance;
        // 状態の開始処理
        currentState.Enter(this);
    }

    // Update is called once per frame
    void Update()
    {
        // 状態更新
        StateUpdate();
    }


    // セッター
    public void SetCameraTransfoem(Transform p_playerTransform) { cameraTransform = p_playerTransform; }

    // ゲッター
    public Transform GetCameraTransform() { return cameraTransform; }
    public Rigidbody GetThrowObjectRigidbody() { return throwObjectRigidbody; }
    public float GetForcePower() { return forcePower; }
    public int GetNoGravityFreams() { return noGravityFreams; }
}
