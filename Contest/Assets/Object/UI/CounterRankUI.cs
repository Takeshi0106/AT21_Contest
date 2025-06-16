using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CounterRankUI : MonoBehaviour
{
    [Header("�v���C���[�I�u�W�F�N�g�̖��O")]
    [SerializeField] private GameObject player = null;
    [Header("�J�E���^�[�����N��UI")]
    [SerializeField] private Sprite[] m_sprite = { };

    // Player�̃}�l�[�W���[������
    private CounterManager counterManager;
    // �C���[�W
    private UnityEngine.UI.Image rankImage;
    int rank = 0;

    // Start is called before the first frame update
    void Start()
    {
        // �J�E���^�[�擾
        counterManager = player.GetComponent<CounterManager>();
        // �����̃C���[�W���擾
        rankImage = this.GetComponent<Image>();

        // �J�E���^�[�}�l�[�W���[�Ƀ����N�A�b�v�C�x���g��ݒ�
        counterManager.SetCounterRankUpEvent(RankUp);
        // �J�E���^�[�}�l�[�W���[�Ƀ����N�_�E���C�x���g��ݒ�
        counterManager.SetCounterRankDownEvent(RankDown);

        // �����N��������
        RankInitialization();

#if UNITY_EDITOR
        // �o�O�`�F�b�N
        if (player == null)
        {
            Debug.LogError("CounterRankUI : PlayerObject��������܂���");
        }
        if (counterManager == null)
        {
            Debug.LogError("CounterRankUI : CounterManager��������܂���");
        }
#endif
    }



    // �����N�A�b�v���Ɏ��s����
    public void RankUp()
    {
        rank++;
        if (rankImage != null && m_sprite[rank] != null)
        {
            rankImage.sprite = m_sprite[rank];
        }
    }



    // �����N�_�E�����Ɏ��s����
    public void RankDown()
    {
        rank--;
        if (rankImage != null && m_sprite[rank] != null)
        {
            rankImage.sprite = m_sprite[rank];
        }
    }


    // �����N��������
    private void RankInitialization()
    {
        rank = 0;
        if (rankImage != null && m_sprite[rank] != null)
        {
            rankImage.sprite = m_sprite[rank];
        }
    }



}
