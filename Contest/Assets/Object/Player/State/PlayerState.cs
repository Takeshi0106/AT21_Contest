using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("カメラオブジェクト名")]
    public string cameraName = "Main Camera"; //カメラオブジェクト名

    [Header("移動速度")]
    public float speed = 2.0f; //移動速度

    // プレイヤーのStateを入れる変数    
    StateClass playerState;
    // カメラのトランスフォーム このスクリプト以外で変更できないように設定
    [HideInInspector] public Transform cameraTransform { get; private set; }
    // Playerのリジッドボディ
    [HideInInspector] public Rigidbody playerRigidbody { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // 状態をセット
        playerState = StandingState.Instance;
        // 状態の開始処理
        playerState.Enter(this.gameObject);

        //　カメラオブジェクトを代入
        cameraTransform = GameObject.Find(cameraName).transform;
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerState : カメラオブジェクトが見つかりません");
            return;
        }

        // Playerリジッドボディー
        // Rigidbodyを探す
        playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerState : Rigidbodyが見つかりません");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 状態の変更
        playerState.Change(this.gameObject);
        // 状態の更新
        playerState.Excute(this.gameObject);
    }

    // StateをChangする処理
    public void ChangPlayerState(StateClass stateClass)
    {
        // 終了処理
        playerState.Exit(this.gameObject);
        // State変更
        playerState = stateClass;
        // 開始処理
        playerState.Enter(this.gameObject);
    }
}
