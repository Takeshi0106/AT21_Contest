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

    // 表示するオブジェクトのHPManager
    [HideInInspector] private HPManager hpDisplayManager;

    // 自分を入れる
    private Slider hpGaugeSlider;

    // Start is called before the first frame update
    void Start()
    {
        hpDisplayManager = hpDisplayObject.GetComponent<HPManager>();
        hpGaugeSlider = this.GetComponent<Slider>();

        // ダメージイベントを設定して呼び出されるようにする
        hpDisplayManager.onDamaged.AddListener(HpUpdate);
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
        hpGaugeSlider.maxValue = hpDisplayManager.GetMaxHP();
        hpGaugeSlider.value = hpDisplayManager.GetCurrentHP();
    }



}
