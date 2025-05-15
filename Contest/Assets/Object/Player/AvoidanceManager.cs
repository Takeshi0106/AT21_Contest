using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AvoidanceManager : MonoBehaviour
{
    [Header("回避の開始フレーム")]
    [SerializeField] private int avoidanceStartUpFreams = 10;
    [Header("回避中のフレーム")]
    [SerializeField] private int avoidanceFreams = 30;
    [Header("回避後のフレーム")]
    [SerializeField] private int avoidanceAfterFreams = 20;
    [Header("回避成功時の無敵フレーム")]
    [SerializeField] private int avoidanceInvincibleFreams = 20;
    [Header("回避成功時の敵のスピード低下(全体のスピード低下と重複)")]
    [SerializeField] private float avoidanceSlowEnemy = 0.8f;
    [Header("回避が成功して遅くなるフレーム")]
    [SerializeField] private int slowFreams = 20;
    [Header("敵のsystemを入れる")]
    [SerializeField] private GameObject enemySystemObj = null;

    [Header("回避成功時の全体のスピード低下")]
    [SerializeField] private float avoidanceSlow = 0.8f;

    [Header("Color Gradingのフィルターの色")]
    [SerializeField] private Color avoidanceColorFilter = Color.red;

    private EnemySystem enemySystem;
    private PlayerState playerState;
    private PostProcessVolume playerVolume;
    private ColorGrading colorGrading;
    private bool slowFlag = false;
    private int freams = 0;


    void Start()
    {
        // 敵のマネジャーを取得する
        enemySystem = enemySystemObj.GetComponent<EnemySystem>();
        // プレイヤーの状態を取得する
        playerState = this.gameObject.GetComponent<PlayerState>();
        // エフェクトを追加
        playerVolume = this.gameObject.GetComponent<PostProcessVolume>();

        // Color Grading エフェクトを取得
        if (playerVolume.profile.TryGetSettings(out colorGrading))
        {
            // 初期色を設定
            colorGrading.colorFilter.value = Color.white;
        }   

#if UNITY_EDITOR
        Debug.Log("AvoidanceManager の初期化が実行されました。");

        if (enemySystem == null)
        {
            Debug.LogError("AvoidanceManager : enemySystemが null です");
        }
        if (playerState == null)
        {
            Debug.LogError("AvoidanceManager : playerStateが null です");
        }
        if (playerVolume == null)
        {
            Debug.LogError("AvoidanceManager : playerVolumeが null です");
        }
        if(colorGrading == null)
        {
            Debug.LogError("AvoidanceManager : colorGradingが null です");
        }
#endif
    }



    public void AvoidUpdate()
    {
        // 回避が成功していなければ実行しない
        if(!slowFlag) { return; }

        if (slowFreams < freams)
        {
            // 回避の終わり処理
            AvoidanceEnd();
        }

        // フレーム更新
        freams++;
    }



    public void AvoidanceStart()
    {
#if UNITY_EDITOR
        Debug.Log("回避成功 開始処理");
#endif
        // エフェクトを実行
        colorGrading.colorFilter.value = avoidanceColorFilter;
        // 全ての速度を遅くする
        Time.timeScale = avoidanceSlow;
        // プレイヤーのフレーム処理を遅くする
        playerState.SetPlayerSpeed(avoidanceSlow);
        // 敵を遅くする
        enemySystem.EnemySlow(avoidanceSlowEnemy);
        // フラグをONにする
        slowFlag = true;
    }



    private void AvoidanceEnd()
    {
#if UNITY_EDITOR
        Debug.Log("回避終了 終了処理");
#endif
        // エフェクトを元に戻す
        colorGrading.colorFilter.value = Color.white;
        // 全ての速度を元に戻す
        Time.timeScale = 1.0f;
        // プレイヤーのフレーム処理を元に戻す
        playerState.SetPlayerSpeed(1.0f);
        // 敵を元に戻す
        enemySystem.EnemySlow(0.8f);
        // フラグをOFFにする
        slowFlag= false;
        // フレームを初期化する
        freams = 0;
    }



    // ゲッター
    public int GetAvoidanceStartUpFreams() { return avoidanceStartUpFreams; }
    public int GetAvoidanceFreams() { return avoidanceFreams; }
    public int GetAvoidanceAfterFreams() { return avoidanceAfterFreams; }
    public int GetAvoidanceInvincibleFreams() { return avoidanceInvincibleFreams; }
    public EnemySystem GetEnemyManager() { return enemySystem;}
}
