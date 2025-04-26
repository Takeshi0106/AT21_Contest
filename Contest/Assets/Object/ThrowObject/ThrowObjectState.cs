using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowObjectState : BaseState<ThrowObjectState>
{
    // インスペクタービューから変更できる
    [Header("投げる力を代入する")]
    [SerializeField] private float forcePower;
    [Header("投げた時の重力無効フレーム")]
    [SerializeField] private int noGravityFreams = 0;
    [Header("プレイヤーのタグ")]
    [SerializeField] private string playerTag = "Player";

    // プレイヤーのトランスフォームを入れる変数
    private Transform cameraTransform;
    // 自分のリジッドボディーを取得しておく
    private Rigidbody throwObjectRigidbody;

    // 当たったタグを取得しておくリスト
    protected List<MultiTag> collidedTags = new List<MultiTag>();


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
    void LateUpdate()
    {
        // 状態更新
        StateUpdate();
    }



    // 自分を削除する
    public void ThrowObjectDelete()
    {
        Destroy(this.gameObject);
    }


    // プレイヤーが敵にぶつかった時の処理
    void OnTriggerEnter(Collider other)
    {
        // 最上位の親（Rootオブジェクト）を取得
        Transform rootTransform = other.transform.root;
        // RootのMultiTagを取得
        MultiTag multiTag = rootTransform.GetComponent<MultiTag>();

        // タグリストに追加
        collidedTags.Add(multiTag);
    }


    // セッター
    public void SetCameraTransfoem(Transform p_playerTransform) { cameraTransform = p_playerTransform; }

    // ゲッター
    public List<MultiTag> GetMultiTagList() { return collidedTags; }
    public Transform GetCameraTransform() { return cameraTransform; }
    public Rigidbody GetThrowObjectRigidbody() { return throwObjectRigidbody; }
    public float GetForcePower() { return forcePower; }
    public int GetNoGravityFreams() { return noGravityFreams; }
    public GameObject GetThrowObject() { return this.gameObject; }
    public string GetPlayerTag() { return playerTag; }
}
