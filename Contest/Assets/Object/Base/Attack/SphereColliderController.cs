using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===================================
// �U�����̓����蔻��𐧌䂷��
// ===================================

public class SphereColliderController : MonoBehaviour
{
    [Header("�U�����̓����蔻��T�C�Y�i���a�j")]
    [SerializeField] private float activeRadius = 2.0f;

    // �����̃R���C�_�[������
    [HideInInspector] private SphereCollider sphereCollider;

    // ���̃R���C�_�[�̑傫�������Ă���
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
