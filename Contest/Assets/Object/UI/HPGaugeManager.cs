using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Slider�ɂ����R���|�[�l���g�o���Ȃ�
[RequireComponent(typeof(Slider))]

public class HPGaugeManager : MonoBehaviour
{
    [Header("HP��\������I�u�W�F�N�g")]
    [SerializeField] private GameObject hpDisplayObject;

    // �\������I�u�W�F�N�g��HPManager
    [HideInInspector] private HPManager hpDisplayManager;

    // ����������
    private Slider hpGaugeSlider;

    // Start is called before the first frame update
    void Start()
    {
        hpDisplayManager = hpDisplayObject.GetComponent<HPManager>();
        hpGaugeSlider = this.GetComponent<Slider>();

        // �_���[�W�C�x���g��ݒ肵�ČĂяo�����悤�ɂ���
        hpDisplayManager.onDamaged.AddListener(HpUpdate);
        // HP�Q�[�W�X�V
        HpUpdate();

#if UNITY_EDITOR
        if (hpDisplayObject == null)
        {
            Debug.LogError("HpGaugemanager : hpDisplayObject���ݒ肳��Ă��܂���");
        }
        if (hpDisplayManager == null)
        {
            Debug.LogError("HPGaugeManager : hpDisplayManager��������܂���");
        }
        if (hpGaugeSlider == null)
        {
            Debug.LogError("HPGaugeManager : hpGaugeSlider�ɃR���|�[�l���g����Ă��܂���");
        }
#endif
    }

    

    public void HpUpdate()
    {
        hpGaugeSlider.maxValue = hpDisplayManager.GetMaxHP();
        hpGaugeSlider.value = hpDisplayManager.GetCurrentHP();
    }



}
