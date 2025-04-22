    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class CounterObjectManager : MonoBehaviour
{
    [Header("カウンターオブジェクトのプレハブ")]
    [SerializeField] private GameObject counterObjectPrefab;



    private GameObject counterObjectInstance;
    private AttackController attackController;



    // プレイヤーの初期化時などに呼び出す
    public void setting()
    {
        Debug.LogError("CounterObjectManager: Initalizeを開始");

#if UNITY_EDITOR
        if (counterObjectPrefab == null)
        {
            Debug.LogError("CounterObjectManager: プレハブが設定されていません！");
            return;
        }
#endif

        // すでに生成済みならスキップ
        if (counterObjectInstance != null) return;

        // インスタンスを生成してプレイヤーの子に設定
        counterObjectInstance = Instantiate(counterObjectPrefab, transform);
        counterObjectInstance.transform.localPosition = Vector3.zero; // プレイヤー中心
        counterObjectInstance.transform.localScale = Vector3.zero;
        counterObjectInstance.SetActive(false);

        // AttackController を取得
        attackController = counterObjectInstance.GetComponent<AttackController>();
        if (attackController == null)
        {
            Debug.LogWarning("CounterObjectManager: AttackController が見つかりません");
        }
    }




    // カウンター開始（表示・リセット）
    public void Activate()
    {
        attackController.EnableAttack();

        counterObjectInstance.transform.localScale = Vector3.zero;
        counterObjectInstance.SetActive(true);
    }



    // カウンター終了（非表示）
    public void Deactivate()
    {
        // 攻撃も無効化
        attackController.DisableAttack();

        counterObjectInstance.SetActive(false);
    }



    // 外からスケール変化（カウンター範囲拡大中）
    public void UpdateScale(float rate, float maxSize)
    {
        float scale = Mathf.Lerp(0f, maxSize, rate);
        counterObjectInstance.transform.localScale = Vector3.one * scale;
    }



    // ゲッター
    public GameObject GetCounterObject()
    {
        return counterObjectInstance;
    }
}
