    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class CounterObjectManager : MonoBehaviour
{
    [Header("�J�E���^�[�I�u�W�F�N�g�̃v���n�u")]
    [SerializeField] private GameObject counterObjectPrefab;



    private GameObject counterObjectInstance;
    private AttackController attackController;



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
        counterObjectInstance.transform.localPosition = Vector3.zero; // �v���C���[���S
        counterObjectInstance.transform.localScale = Vector3.zero;
        counterObjectInstance.SetActive(false);

        // AttackController ���擾
        attackController = counterObjectInstance.GetComponent<AttackController>();
        if (attackController == null)
        {
            Debug.LogWarning("CounterObjectManager: AttackController ��������܂���");
        }
    }




    // �J�E���^�[�J�n�i�\���E���Z�b�g�j
    public void Activate()
    {
        attackController.EnableAttack();

        counterObjectInstance.transform.localScale = Vector3.zero;
        counterObjectInstance.SetActive(true);
    }



    // �J�E���^�[�I���i��\���j
    public void Deactivate()
    {
        // �U����������
        attackController.DisableAttack();

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
}
