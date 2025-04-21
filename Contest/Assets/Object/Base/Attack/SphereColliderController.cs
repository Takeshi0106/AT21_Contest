using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===================================
// 攻撃中の当たり判定を制御する
// ===================================

public class SphereColliderController : MonoBehaviour
{
    [Header("攻撃時の当たり判定サイズ（半径）")]
    [SerializeField] private float activeRadius = 2.0f;

    // 自分のコライダーを入れる
    [HideInInspector] private SphereCollider sphereCollider;

    // 元のコライダーの大きさを入れておく
    private float originalRadius;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            originalRadius = sphereCollider.radius;
        }
    }

    public void EnableCollider()
    {
        if (sphereCollider != null)
        {
            sphereCollider.enabled = true;
            sphereCollider.radius = originalRadius;
        }
    }

    public void DisableCollider()
    {
        if (sphereCollider != null)
        {
            sphereCollider.enabled = false;
            sphereCollider.radius = originalRadius;
        }
    }

    public void SetScale(float radius)
    {
        activeRadius = radius;
    }
}
