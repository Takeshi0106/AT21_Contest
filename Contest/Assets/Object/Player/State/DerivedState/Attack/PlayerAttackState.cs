using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerAttackState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerAttackState instance;
    // weponData
    private static BaseAttackData weponData;
    // �t���[�����v��
    int freams = 0;

    // �C���X�^���X���擾����֐�
    public static PlayerAttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerAttackState();
            }
            return instance;
        }
    }



    // ��Ԃ̕ύX����
    public override void Change(PlayerState playerState)
    {
        // �U���̃t���[�����߂�����
        if (freams == weponData.GetAttackStartupFrames(playerState.GetPlayerConbo()) +
            weponData.GetAttackSuccessFrames(playerState.GetPlayerConbo()))
        {
            // �ォ��d����ԂɈڍs����
            playerState.ChangeState(PlayerAttackRecoveryState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
        Debug.LogError($"PlayerAttackState : �J�n�iCombo���F{playerState.GetPlayerConbo() + 1}�j");

        if (weponData == null)
        {
            Debug.LogError("PlayerAttackState : WeponData��������܂���");
            return;
        }
#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        freams++;

        // �U�������ON�ɂ���
        if (freams >= weponData.GetAttackStartupFrames(playerState.GetPlayerConbo()))
        {
            playerState.GetPlayerWeponManager().EnableAllWeaponAttacks();
        }
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        // �U�������OFF
        playerState.GetPlayerWeponManager().DisableAllWeaponAttacks();
        // ������
        freams = 0;
    }



}
