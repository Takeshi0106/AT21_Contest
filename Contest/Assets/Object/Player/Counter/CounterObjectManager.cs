    using System.Collections;
    using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CounterObjectManager : MonoBehaviour
{
    [Header("�J�E���^�[�I�u�W�F�N�g�̃v���n�u")]
    [SerializeField] private GameObject counterObjectPrefab;
    [Header("�J�E���^�[�I�u�W�F�N�g�̍ő�T�C�Y")]
    [SerializeField] private float m_MaxSize;



    private GameObject counterObjectInstance;
    private AttackController attackController;
    private Renderer m_CounterRend;



    // �v���C���[�̏��������ȂǂɌĂяo��
    public void setting()
    {
        Debug.LogError("CounterObjectManager: Initalize���J�n");

#if UNITY_EDITOR
        if (counterObjectPrefab == null)
        {
            Debug.LogError("CounterObjectManager: �v���n�u���ݒ肳��Ă��܂���I");
            return;
        }
#endif

        // ���łɐ����ς݂Ȃ�X�L�b�v
        if (counterObjectInstance != null) return;

        // �C���X�^���X�𐶐����ăv���C���[�̎q�ɐݒ�
        counterObjectInstance = Instantiate(counterObjectPrefab, transform);

        Vector3 counterPos = Vector3.zero;
        counterPos.y = 1.0f;

        counterObjectInstance.transform.localPosition = counterPos; // �v���C���[���S
        counterObjectInstance.transform.localScale = Vector3.zero;

        // AttackController ���擾
        attackController = counterObjectInstance.GetComponent<AttackController>();
        if (attackController == null)
        {
            Debug.LogWarning("CounterObjectManager: AttackController ��������܂���");
        }

        // ������
        attackController.AttackControllerStart();

        m_CounterRend = counterObjectInstance.GetComponentInChildren<Renderer>();
        if (m_CounterRend != null)
        {
            m_CounterRend.enabled = false;
        }
        else
        {
            Debug.LogWarning("CounterObjectManager: �q�I�u�W�F�N�g�� Renderer ��������܂���ł���");
        }

        counterObjectInstance.SetActive(false);
    }




    // �J�E���^�[�J�n�i�\���E���Z�b�g�j
    public void Activate()
    {
        attackController.EnableAttack();

        counterObjectInstance.transform.localScale = Vector3.zero;
        counterObjectInstance.SetActive(true);

//#if UNITY_EDITOR
        m_CounterRend.enabled = true;
//#endif 
    }



    // �J�E���^�[�I���i��\���j
    public void Deactivate()
    {
        // �U����������
        attackController.DisableAttack();
        counterObjectInstance.transform.localScale = Vector3.zero;
        counterObjectInstance.SetActive(false);
    }



    // �O����X�P�[���ω��i�J�E���^�[�͈͊g�咆�j
    public void UpdateScale(float rate, float maxSize)
    {
        float scale = Mathf.Lerp(0f, maxSize, rate);
        counterObjectInstance.transform.localScale = Vector3.one * scale;
    }



    // �Q�b�^�[
    public GameObject GetCounterObject()
    {
        return counterObjectInstance;
    }
    public float GetCounterMaxSize() { return m_MaxSize; }
}
