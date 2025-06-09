using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sliderにしかコンポーネント出来ない
[RequireComponent(typeof(Slider))]

public class HPGaugeManager : MonoBehaviour
{
    [Header("HPを表示するオブジェクト")]
    [SerializeField] private GameObject hpDisplayObject;

    [Header("HPを表示するスライダー")]
    [SerializeField] private Slider hpGaugeSlider;
    [Header("HPを遅れて減らすスライダー")]
    [SerializeField] private Slider m_DelayedDamageBar;

    // 表示するオブジェクトのHPManager
    [HideInInspector] private HPManager hpDisplayManager;
    private float delayedBarUpdateSpeed = 0.5f; // 速度
    private Coroutine delayedBarCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        hpDisplayManager = hpDisplayObject.GetComponent<HPManager>();
        // hpGaugeSlider = this.GetComponent<Slider>();

        // ダメージイベントを設定して呼び出されるようにする
        hpDisplayManager.SetOnDamagedEvent(HpUpdate);
        // HPゲージ更新
        HpUpdate();

#if UNITY_EDITOR
        if (hpDisplayObject == null)
        {
            Debug.LogError("HpGaugemanager : hpDisplayObjectが設定されていません");
        }
        if (hpDisplayManager == null)
        {
            Debug.LogError("HPGaugeManager : hpDisplayManagerが見つかりません");
        }
        if (hpGaugeSlider == null)
        {
            Debug.LogError("HPGaugeManager : hpGaugeSliderにコンポーネントされていません");
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
            // HPが０になったらゲージを消す
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
