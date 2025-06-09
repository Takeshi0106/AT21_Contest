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

    [Header("HP��\������X���C�_�[")]
    [SerializeField] private Slider hpGaugeSlider;
    [Header("HP��x��Č��炷�X���C�_�[")]
    [SerializeField] private Slider m_DelayedDamageBar;

    // �\������I�u�W�F�N�g��HPManager
    [HideInInspector] private HPManager hpDisplayManager;
    private float delayedBarUpdateSpeed = 0.5f; // ���x
    private Coroutine delayedBarCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        hpDisplayManager = hpDisplayObject.GetComponent<HPManager>();
        // hpGaugeSlider = this.GetComponent<Slider>();

        // �_���[�W�C�x���g��ݒ肵�ČĂяo�����悤�ɂ���
        hpDisplayManager.SetOnDamagedEvent(HpUpdate);
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
        float currentHP = hpDisplayManager.GetCurrentHP();
        float maxHP = hpDisplayManager.GetMaxHP();

        hpGaugeSlider.maxValue = maxHP;
        hpGaugeSlider.value = currentHP;

        if (delayedBarCoroutine != null)
        {
            StopCoroutine(delayedBarCoroutine);
        }

        delayedBarCoroutine = StartCoroutine(UpdateDelayedBar(currentHP, maxHP));


        if (hpDisplayManager.GetCurrentHP() <= 0)
        {
            // HP���O�ɂȂ�����Q�[�W������
            this.gameObject.SetActive(false);
        }

    }



    private IEnumerator UpdateDelayedBar(float currentHP, float maxHP)
    {
        m_DelayedDamageBar.maxValue = maxHP;

        float startValue = m_DelayedDamageBar.value;
        float targetValue = currentHP;

        float elapsed = 0f;

        while (m_DelayedDamageBar.value > targetValue)
        {
            elapsed += Time.deltaTime * delayedBarUpdateSpeed;
            m_DelayedDamageBar.value = Mathf.Lerp(startValue, targetValue, elapsed);
            yield return null;
        }

        m_DelayedDamageBar.value = targetValue;
    }



}
