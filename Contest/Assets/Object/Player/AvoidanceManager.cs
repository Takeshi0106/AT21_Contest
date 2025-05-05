using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("回避成功時の敵のスピード低下")]
    [SerializeField] private float avoidanceSlow = 0.8f;
    [Header("回避が成功して遅くなるフレーム")]
    [SerializeField] private int EnemySlowFreams = 20;
    [Header("敵のマネージャーを入れる")]
    [SerializeField] private GameObject enemyManagerObj = null;

    [Header("回避成功時の自分のスピード低下")]
    [SerializeField] private float avoidanceSlowPlayer = 0.8f;

    private EnemyManager enemyManager;
    private bool slowFlag = false;
    private int freams = 0;


    void Start()
    {
        // 敵のマネジャーを取得する
        enemyManager= enemyManagerObj.GetComponent<EnemyManager>();
    }



    public void AvoidUpdate()
    {
        // 回避が成功していなければ実行しない
        if(!slowFlag) { return; }

        if (EnemySlowFreams < freams)
        {
            // 回避の終わり処理
            AvoidanceEnd();
        }

        // フレーム更新
        freams++;
    }



    public void AvoidanceStart()
    {
        // 敵を遅くする
        enemyManager.EnemySlow(avoidanceSlow);
        // フラグをONにする
        slowFlag = true;
    }



    private void AvoidanceEnd()
    {
        // 敵を元に戻す
        enemyManager.EnemySlow(1.0f);
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
    public EnemyManager GetEnemyManager() { return enemyManager;}
    public float GetAvoidanceSlowPlayer() { return avoidanceSlowPlayer; }
}
