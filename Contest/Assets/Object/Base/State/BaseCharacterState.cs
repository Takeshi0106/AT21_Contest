using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =============================================================
// 当たり判定があるオブジェクトの状態を管理するクラス
// =============================================================

public class BaseCharacterState<T> : BaseState<T> where T : BaseCharacterState<T>
{
    [Header("ヒットエフェクトパーティクル")]
    [SerializeField] private GameObject[] m_HitParticle;
    [Header("ヒットエフェクトパーティクルの位置")]
    [SerializeField] private Vector3 m_HitParticlePos;

    // 接触したオブジェクトの情報を入れる
    public class CollidedInfo
    {
        public Collider collider;
        public MultiTag multiTag;
        public bool hitFlag;
        public int hitID;

        public CollidedInfo(Collider collider, MultiTag multiTag,bool hitFlag,int hitID)
        {
            this.collider = collider;
            this.multiTag = multiTag;
            this.hitFlag = hitFlag;
            this.hitID = hitID;
        }
    }

    // 接触中の情報を入れる配列（Collider + MultiTag）
    protected List<CollidedInfo> collidedInfos = new List<CollidedInfo>();
    // すでにダメージを受けた攻撃オブジェクトを保持しておく
    // protected HashSet<Collider> damagedColliders = new HashSet<Collider>();
    // 自分の攻撃情報を取得する
    protected AttackInterface m_SelfAttackInterface;

    protected void CharacterStart()
    {
        m_SelfAttackInterface = this.GetComponent<AttackInterface>();

#if UNITY_EDITOR
        if (m_SelfAttackInterface == null)
        {
            Debug.Log("AttackInterfaceが見つかりませんでした。");
        }
#endif
    }

    // プレイヤーが敵にぶつかった時の処理
    void OnTriggerEnter(Collider other)
    {
        // MulltiTagを取得する
        MultiTag multiTag = other.GetComponent<MultiTag>();
        // AttackInterface を取得
        AttackInterface attackInterface = other.GetComponentInParent<AttackInterface>();

        int currentAttackId = -1;

        if (attackInterface != null)
        {
            currentAttackId = attackInterface.GetOtherAttackID();

            Debug.Log($"Name : {other.name} : AttackID{currentAttackId}");
        }

        var existingInfo = collidedInfos.FirstOrDefault(info => info.collider == other);

        if (existingInfo == null)
        {
            // 新規追加
            collidedInfos.Add(new CollidedInfo(other, multiTag, false, currentAttackId));
        }
        else
        {
            // 攻撃IDが変わっていたらフラグリセット
            if (existingInfo.hitID != currentAttackId)
            {
#if UNITY_EDITOR
                Transform root = other.transform.root;
                Debug.Log($"{this.gameObject.name} {root.name} : 一度ヒットした者");
#endif

                existingInfo.hitFlag = false;
                existingInfo.hitID = currentAttackId;
            }
        }

#if UNITY_EDITOR
        // Debug.Log("Triggerに当たった : " + other.gameObject.name);
#endif
    }



    // プレイヤーが敵と離れた時の処理
    void OnTriggerExit(Collider other)
    {
        for (int i = collidedInfos.Count - 1; i >= 0; i--)
        {
            if (collidedInfos[i].collider == other)
            {
#if UNITY_EDITOR
                // Debug.Log($"[TriggerExit] {other.gameObject.name} から離れました。hitFlag: {collidedInfos[i].hitFlag}");
#endif
                collidedInfos.RemoveAt(i);
                break; // 一致は1つだけの想定なら break でOK
            }
        }

        // 配列から同じ物を探す
        //collidedInfos.RemoveAll(info => info.collider == other);

#if UNITY_EDITOR
        // Debug.Log("Triggerから離れた : " + other.gameObject.name);
#endif
    }

    
    protected void DamageParticle(Collider other)
    {
        // プレイヤーの中心で生成
        Vector3 hitPos = transform.position;
        // 回転は適当に正面向き、または Quaternion.identity
        Quaternion hitRot = Quaternion.identity;

        foreach (var particle in m_HitParticle)
        {
            Instantiate(particle, hitPos + m_HitParticlePos, hitRot);
        }
    }
    

    // ゲッター
    public AttackInterface GetAttackInterface() { return m_SelfAttackInterface; }

}