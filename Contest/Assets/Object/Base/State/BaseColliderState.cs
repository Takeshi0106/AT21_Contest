using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =============================================================
// 当たり判定があるオブジェクトの状態を管理するクラス
// =============================================================

public class BaseColliderState<T> : BaseState<T> where T : BaseColliderState<T>
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

    // 接触中の情報を入れる配列（Collider + MultiTag）
    protected List<CollidedInfo> collidedInfos = new List<CollidedInfo>();
    // すでにダメージを受けた攻撃オブジェクトを保持しておく
    protected HashSet<Collider> damagedColliders = new HashSet<Collider>();


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
        // Debug.Log("Triggerに当たった : " + other.gameObject.name);
#endif
    }



    // プレイヤーが敵と離れた時の処理
    void OnTriggerExit(Collider other)
    {
        // 配列から同じ物を探す
        collidedInfos.RemoveAll(info => info.collider == other);

#if UNITY_EDITOR
        // Debug.Log("Triggerから離れた : " + other.gameObject.name);
#endif
    }



}
