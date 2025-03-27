using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rigidbodyコンポーネントが必須
[RequireComponent(typeof(Rigidbody))]

public class MovePlayerScript : MonoBehaviour
{
    // Playerのリジッドボディ
    Rigidbody rb;
    // 移動度
    Vector3 moveForward;
    // カメラのトランスフォーム
    Transform cameraTransform;

    [Header("カメラオブジェクト名")]
    public string cameraName = "Main Camera"; //カメラオブジェクト名

    [Header("移動速度")]
    public float speed = 2.0f; //移動速度

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbodyを代入
        rb = this.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("MovePlayerScript : Rigidbodyが見つかりません");
            return;
        }

        //　カメラオブジェクトを代入
        cameraTransform = GameObject.Find(cameraName).transform;
        if (cameraTransform == null)
        {
            Debug.LogError("MovePlayerScript : カメラオブジェクトが見つかりません");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //移動度をリセットする
        moveForward = Vector3.zero;

        //入力を取得
        float inputX = Input.GetAxisRaw("Horizontal"); //横方向
        float inputY = Input.GetAxisRaw("Vertical"); //縦方向

        // カメラのベクトルから移動方向を決める
        moveForward = cameraTransform.forward * inputY + cameraTransform.right * inputX;
        moveForward = Vector3.Scale(moveForward, new Vector3(1, 0, 1)).normalized * speed;

        // 移動度に移動速度を掛けて力を加える
        rb.velocity = new Vector3(moveForward.x, rb.velocity.y, moveForward.z);

        //キャラクターを回転させる
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, cameraTransform.eulerAngles.y, this.transform.eulerAngles.z);

    }
}
