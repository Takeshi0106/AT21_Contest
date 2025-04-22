using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CounterRankUI : MonoBehaviour
{
    [Header("�v���C���[�I�u�W�F�N�g�̖��O")]
    [SerializeField] private string playerName = "Player";

    
    // �e�L�X�g
    private TextMeshProUGUI text;
    // Player�̃}�l�[�W���[������
    private CounterManager counterManager;



    // Start is called before the first frame update
    void Start()
    {
        // Player��T��
        GameObject player = GameObject.Find(playerName);

        // �e�L�X�g���擾
        text = this.GetComponent<TextMeshProUGUI>();
        // �J�E���^�[�擾
        counterManager = player.GetComponent<CounterManager>();

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
        if (text == null)
        {
            Debug.LogError("CounterRankUI : TextMeshPro��������܂���");
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
        text.text = (counterManager.GetCurrentRank() + 1).ToString();
    }



    // �����N�_�E�����Ɏ��s����
    public void RankDown()
    {
        text.text = (counterManager.GetCurrentRank() + 1).ToString();
    }


    // �����N��������
    private void RankInitialization()
    {
        text.text = (counterManager.GetCurrentRank() + 1).ToString();
    }



}
