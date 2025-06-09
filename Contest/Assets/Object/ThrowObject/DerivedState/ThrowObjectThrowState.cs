using UnityEngine;

// ==================================
// �������Ă�����
// ==================================

public class ThrowObjectThrowState : StateClass<ThrowObjectState>
{
    // �C���X�^���X������ϐ�
    private static ThrowObjectThrowState instance;

    // �t���[��
    int freams = 0;
    // �ۑ��p�t���[��
    int m_SaveFreams = 0;
    // ������Ƃ��ɓ�����Ȃ����Ƃ����邽�ߗ]������������
    bool m_DeleteFlag = false;

    // �C���X�^���X���擾����֐�
    public static ThrowObjectThrowState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThrowObjectThrowState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(ThrowObjectState state)
    {
        if (m_DeleteFlag && freams >= m_SaveFreams)
        {
            state.ChangeState(ThrowObjectDeleteState.Instance);
            Debug.Log("ThrowObject : �������Ԉڍs");
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(ThrowObjectState state)
    {
        // �x�̃x�N�g���ɔ�΂����̏���
        state.GetThrowObjectRigidbody().AddForce(state.GetPlayerTransform().forward * state.GetForcePower(),
    ForceMode.Impulse);

        state.GetThrowAttackController().EnableAttack();
    }



    // ��Ԓ��̏���
    public override void Excute(ThrowObjectState state)
    {
        // �d�͂𖳌��ɂ���
        if (state.GetNoGravityFreams() > freams)
        {
            state.GetThrowObjectRigidbody().useGravity = false;
        }
        // �d�͂�L���ɂ���
        else
        {
            state.GetThrowObjectRigidbody().useGravity = true;
        }

        freams++; // �t���[���X�V

        foreach (var tags in state.GetMultiTagList())
        {
            // �G�l�~�[���X�e�[�W�ɂԂ�������
            if (tags.HasTag(state.GetEnemyTag()) ||
                (tags.HasTag(state.GetStageTag()) && state.GetNoGravityFreams() < freams) ||
                state.GetThrowObjectTransform().position.y < -200.0f)
            {
                m_SaveFreams = freams;
                m_DeleteFlag = true;
            }
        }
    }



    // ��Ԓ��̏I������
    public override void Exit(ThrowObjectState state)
    {
        // �U���^�O�����ɖ߂�
        // state.GetThrowAttackController().DisableAttack();
    }



}
