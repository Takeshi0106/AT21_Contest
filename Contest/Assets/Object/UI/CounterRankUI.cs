using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CounterRankUI : MonoBehaviour
{
    [Header("プレイヤーオブジェクトの名前")]
    [SerializeField] private string playerName = "Player";

    
    // テキスト
    private TextMeshProUGUI text;
    // Playerのマネージャーを入れる
    private CounterManager counterManager;



    // Start is called before the first frame update
    void Start()
    {
        // Playerを探す
        GameObject player = GameObject.Find(playerName);

        // テキストを取得
        text = this.GetComponent<TextMeshProUGUI>();
        // カウンター取得
        counterManager = player.GetComponent<CounterManager>();

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
        if (text == null)
        {
            Debug.LogError("CounterRankUI : TextMeshProが見つかりません");
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
        text.text = (counterManager.GetCurrentRank() + 1).ToString();
    }



    // ランクダウン時に実行する
    public void RankDown()
    {
        text.text = (counterManager.GetCurrentRank() + 1).ToString();
    }


    // ランクを初期化
    private void RankInitialization()
    {
        text.text = (counterManager.GetCurrentRank() + 1).ToString();
    }



}
