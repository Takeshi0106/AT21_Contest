using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================
// エネミーの状態
// =====================================

public class EnemyState : BaseState<EnemyState>
{
    // 衝突したオブジェクトを保存するリスト
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();
    // 子供オブジェクトを取得る
    [HideInInspector] private Transform childTransform;

    //子オブジェクトのコライダーを取得
    private Transform childCollider;

    // Start is called before the first frame update
    void Start()
    {
        // 状態をセット
        currentState = EnemyStandingState.Instance;

        // 状態の開始処理
        currentState.Enter(this);

        childTransform = transform.Find("Sword"); // 子オブジェクト"Sword"を探す

        //childCollider = transform.Find("SearchArea");

#if UNITY_EDITOR
        if (childTransform != null)
        {
            Debug.Log("子オブジェクトの名前: " + childTransform.gameObject.name);
            //Debug.Log("子オブジェクトのコライダー名: " + childCollider.gameObject.name);
        }
#endif
    }



    void Update()
    {
        StateUpdate();
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
    public List<Collider> GetEnemyCollidedObjects() => collidedObjects;
    public Transform GetChildTransform() => childTransform;

    //public Transform GetChildCollider() => childCollider;
}
