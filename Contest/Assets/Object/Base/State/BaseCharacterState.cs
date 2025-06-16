using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =============================================================
// �����蔻�肪����I�u�W�F�N�g�̏�Ԃ��Ǘ�����N���X
// =============================================================

public class BaseCharacterState<T> : BaseState<T> where T : BaseCharacterState<T>
{
    [Header("�q�b�g�G�t�F�N�g�p�[�e�B�N��")]
    [SerializeField] private GameObject[] m_HitParticle;
    [Header("�q�b�g�G�t�F�N�g�p�[�e�B�N���̈ʒu")]
    [SerializeField] private Vector3 m_HitParticlePos;

    // �ڐG�����I�u�W�F�N�g�̏�������
    public class CollidedInfo
    {
        public Collider collider;
        public MultiTag multiTag;
        public bool hitFlag;

        public CollidedInfo(Collider collider, MultiTag multiTag,bool hitFlag)
        {
            this.collider = collider;
            this.multiTag = multiTag;
            this.hitFlag = hitFlag;
        }
    }

    // �ڐG���̏�������z��iCollider + MultiTag�j
    protected List<CollidedInfo> collidedInfos = new List<CollidedInfo>();
    // ���łɃ_���[�W���󂯂��U���I�u�W�F�N�g��ێ����Ă���
    // protected HashSet<Collider> damagedColliders = new HashSet<Collider>();
    // �����̍U�������擾����
    protected AttackInterface m_SelfAttackInterface;

    protected void CharacterStart()
    {
        m_SelfAttackInterface = this.GetComponent<AttackInterface>();

#if UNITY_EDITOR
        if (m_SelfAttackInterface == null)
        {
            Debug.Log("AttackInterface��������܂���ł����B");
        }
#endif
    }

    // �v���C���[���G�ɂԂ��������̏���
    void OnTriggerEnter(Collider other)
    {
        // �z��̒��ɓ������̂����邩�̃`�F�b�N
        if (!collidedInfos.Any(info => info.collider == other))
        {
            // MulltiTag���擾����
            MultiTag multiTag = other.GetComponent<MultiTag>();
            // �z��ɒǉ�
            collidedInfos.Add(new CollidedInfo(other, multiTag, false));
        }

#if UNITY_EDITOR
        // Debug.Log("Trigger�ɓ������� : " + other.gameObject.name);
#endif
    }



    // �v���C���[���G�Ɨ��ꂽ���̏���
    void OnTriggerExit(Collider other)
    {
        for (int i = collidedInfos.Count - 1; i >= 0; i--)
        {
            if (collidedInfos[i].collider == other)
            {
#if UNITY_EDITOR
                // Debug.Log($"[TriggerExit] {other.gameObject.name} ���痣��܂����BhitFlag: {collidedInfos[i].hitFlag}");
#endif
                collidedInfos.RemoveAt(i);
                break; // ��v��1�����̑z��Ȃ� break ��OK
            }
        }

        // �z�񂩂瓯������T��
        //collidedInfos.RemoveAll(info => info.collider == other);

#if UNITY_EDITOR
        // Debug.Log("Trigger���痣�ꂽ : " + other.gameObject.name);
#endif
    }

    
    protected void DamageParticle(Collider other)
    {
        // �v���C���[�̒��S�Ő���
        Vector3 hitPos = transform.position;
        // ��]�͓K���ɐ��ʌ����A�܂��� Quaternion.identity
        Quaternion hitRot = Quaternion.identity;

        foreach (var particle in m_HitParticle)
        {
            Instantiate(particle, hitPos + m_HitParticlePos, hitRot);
        }
    }
    

    // �Q�b�^�[
    public AttackInterface GetAttackInterface() { return m_SelfAttackInterface; }

}