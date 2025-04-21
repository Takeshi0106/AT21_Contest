using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// ===================================
// ��Ԃ��Ǘ�����N���X
// ===================================

public class BaseState<T> : MonoBehaviour where T : BaseState<T>
{
    // �ڐG�����I�u�W�F�N�g�̏�������
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

    protected StateClass<T> currentState;
    protected HPManager hpManager;

    // �ڐG���̏�������z��iCollider + MultiTag�j
    protected List<CollidedInfo> collidedInfos = new List<CollidedInfo>();
    // ���łɃ_���[�W���󂯂��U���I�u�W�F�N�g��ێ����Ă���
    protected HashSet<Collider> damagedColliders = new HashSet<Collider>();


    protected void StateUpdate()
    {
        currentState.Change((T)this);
        currentState.Excute((T)this);
    }

    public void ChangeState(StateClass<T> newState)
    {
        currentState.Exit((T)this);
        currentState = newState;
        currentState.Enter((T)this);
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
            collidedInfos.Add(new CollidedInfo(other, multiTag));
        }

#if UNITY_EDITOR
        // Debug.Log("Trigger�ɓ������� : " + other.gameObject.name);
#endif
    }



    // �v���C���[���G�Ɨ��ꂽ���̏���
    void OnTriggerExit(Collider other)
    {
        // �z�񂩂瓯������T��
        collidedInfos.RemoveAll(info => info.collider == other);

#if UNITY_EDITOR
        // Debug.Log("Trigger���痣�ꂽ : " + other.gameObject.name);
#endif
    }
}
