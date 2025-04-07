using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================
// �G�l�~�[�̏��
// =====================================

public class EnemyState : BaseState<EnemyState>
{
    // �Փ˂����I�u�W�F�N�g��ۑ����郊�X�g
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();
    // �q���I�u�W�F�N�g���擾��
    [HideInInspector] private Transform childTransform;


    // Start is called before the first frame update
    void Start()
    {
        // ��Ԃ��Z�b�g
        currentState = EnemyStandingState.Instance;

        // ��Ԃ̊J�n����
        currentState.Enter(this);

        childTransform = transform.Find("Sword"); // �q�I�u�W�F�N�g"Sword"��T��

#if UNITY_EDITOR
        if (childTransform != null)
        {
            Debug.Log("�q�I�u�W�F�N�g�̖��O: " + childTransform.gameObject.name);
        }
#endif
    }



    void Update()
    {
        StateUpdate();
    }



    // �v���C���[���G�ɂԂ��������̏���
    void OnCollisionEnter(Collision other)
    {
        Collider collider = other.collider;
        if (!collidedObjects.Contains(other.collider))
        {
            collidedObjects.Add(other.collider);
        }
#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ��s�����
        Debug.Log("Object�ɓ������� : " + other.gameObject.name);
#endif
    }



    // �v���C���[���G�Ɨ��ꂽ���̏���
    void OnCollisionExit(Collision other)
    {
        Collider collider = other.collider;
        if (collidedObjects.Contains(other.collider))
        {
            collidedObjects.Remove(other.collider);
        }
#if UNITY_EDITOR
        // �G�f�B�^���s���Ɏ��s�����
        Debug.Log("Object�����ꂽ : " + other.gameObject.name);
#endif
    }



    // �Q�b�^�[
    public List<Collider> GetEnemyCollidedObjects() => collidedObjects;
    public Transform GetChildTransform() => childTransform;
}
