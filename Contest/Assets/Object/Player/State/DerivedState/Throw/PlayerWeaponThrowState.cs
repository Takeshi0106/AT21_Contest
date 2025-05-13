using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// ����𓊂�����
// ===============================

public class PlayerWeaponThrowState : StateClass<PlayerState>
{
    // �C���X�^���X������ϐ�
    private static PlayerWeaponThrowState instance;
    // ����̏��
    private static BaseAttackData weponData;
    // ����̃I�u�W�F�N�g������ϐ�
    private static GameObject prefab = null;

    // ����̓�����ʒu������ϐ�
    private Vector3 weaponPos;

    // �����镐��̓�����܂ł̃t���[��
    int throwStartUpFreams = 0;
    // ��������̍d���t���[��
    int throwStaggerFreams = 0;

    // ��ԕύX�܂ł̎���
    float freams = 0.0f;
    // ���������ǂ����̃t���O
    bool throwFlag = false;



    // �C���X�^���X���擾����
    public static PlayerWeaponThrowState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerWeaponThrowState();
            }
            return instance;
        }
    }



    // ��Ԃ�ύX����
    public override void Change(PlayerState playerState)
    {
        // ������Ԃɖ߂�
        if (freams > throwStartUpFreams + throwStaggerFreams)
        {
            playerState.ChangeState(PlayerCounterStaggerState.Instance);
            return;
        }
    }



    // ��Ԃ̊J�n����
    public override void Enter(PlayerState playerState)
    {
        // ����f�[�^���擾����
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        // �A�j���[�V�������擾���Ă���
        AnimationClip animClip = weponData.GetThrowAnimation();
        var childAnim = playerState.GetPlayerWeponManager().GetCurrentWeaponAnimator();

        // Wepon�̃t���[�������擾���Ă���
        throwStartUpFreams = weponData.GetThrowStartUp();
        throwStaggerFreams = weponData.GetThrowStagger();

        // �J�n�ʒu���擾���Ă���
        weaponPos = weponData.GetThrowStartPosition();

        // ������擾����
        prefab = Resources.Load<GameObject>($"Object/ThrowWeapon/{weponData.GetThrowWeaponName()}");

        // �A�j���[�V�����J�n����
        if (animClip != null && childAnim != null)
        {
            childAnim.CrossFade(animClip.name, 0.2f);
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerWeaponThrowState : �J�n");

        // �G�f�B�^���s���Ɏ擾���ĐF��ύX����
        playerState.GetPlayerRenderer().material.color = Color.green;

        if (prefab == null)
        {
            Debug.LogError("ThrowWeapon ��������܂���");
        }

#endif
    }



    // ��Ԓ��̏���
    public override void Excute(PlayerState playerState)
    {
        // �_���[�W������L���ɂ���
        playerState.HandleDamage();

        // ������
        if (freams > throwStartUpFreams && !throwFlag)
        {
            throwFlag = true;

            // �����镐��̃I�u�W�F�N�g���擾����
            if (prefab != null)
            {
                // ������ʒu�̃O���[�o���Ȉʒu���v�Z����
                Vector3 worldThrowPos = playerState.GetPlayerTransform().position
                      + (playerState.GetPlayerTransform().forward * weaponPos.z)
                      + (playerState.GetPlayerTransform().right * weaponPos.x)
                      + (playerState.GetPlayerTransform().up * weaponPos.y);

                // �I�u�W�F�N�g�𐶐�����
                GameObject thrownObj = GameObject.Instantiate(prefab, worldThrowPos, Quaternion.identity);

                // �I�u�W�F�N�g�ɉ�]��K�p
                thrownObj.transform.rotation = Quaternion.LookRotation(playerState.GetPlayerTransform().forward);

                // �v���C���[�̃g�����X�t�H�[�����Z�b�g����
                thrownObj.GetComponent<ThrowObjectState>().SetCameraTransfoem(playerState.GetCameraTransform());
            }

            // ��������폜
            playerState.GetPlayerWeponManager().RemoveWeapon(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
            playerState.GetPlayerRenderer().material.color = Color.blue;
#endif
        }

        freams += playerState.GetPlayerSpeed();
    }



    // ��Ԓ��̏I������
    public override void Exit(PlayerState playerState)
    {
        freams = 0.0f;
        throwFlag = false;

#if UNITY_EDITOR
        // �G�f�B�^���s���ɐF�����ɖ߂�
        playerState.GetPlayerRenderer().material.color = Color.white;
#endif
    }



}
