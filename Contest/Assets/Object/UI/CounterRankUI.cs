using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CounterRankUI : MonoBehaviour
{
    [Header("プレイヤーオブジェクトの名前")]
    [SerializeField] private GameObject player = null;
    [Header("カウンターランクのUI")]
    [SerializeField] private Sprite[] m_sprite = { };

    // Playerのマネージャーを入れる
    private CounterManager counterManager;
    // イメージ
    private UnityEngine.UI.Image rankImage;
    int rank = 0;

    // Start is called before the first frame update
    void Start()
    {
        // カウンター取得
        counterManager = player.GetComponent<CounterManager>();
        // 自分のイメージを取得
        rankImage = this.GetComponent<Image>();

        // カウンターマネージャーにランクアップイベントを設定
        counterManager.SetCounterRankUpEvent(RankUp);
        // カウンターマネージャーにランクダウンイベントを設定
        counterManager.SetCounterRankDownEvent(RankDown);

        // ランクを初期化
        RankInitialization();

#if UNITY_EDITOR
        // バグチェック
        if (player == null)
        {
            Debug.LogError("CounterRankUI : PlayerObjectが見つかりません");
        }
        if (counterManager == null)
        {
            Debug.LogError("CounterRankUI : CounterManagerが見つかりません");
        }
#endif
    }



    // ランクアップ時に実行する
    public void RankUp()
    {
        rank++;
        if (rankImage != null && m_sprite[rank] != null)
        {
            rankImage.sprite = m_sprite[rank];
        }
    }



    // ランクダウン時に実行する
    public void RankDown()
    {
        rank--;
        if (rankImage != null && m_sprite[rank] != null)
        {
            rankImage.sprite = m_sprite[rank];
        }
    }


    // ランクを初期化
    private void RankInitialization()
    {
        rank = 0;
        if (rankImage != null && m_sprite[rank] != null)
        {
            rankImage.sprite = m_sprite[rank];
        }
    }



}
