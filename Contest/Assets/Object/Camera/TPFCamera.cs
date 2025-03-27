using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cameraコンポーネントが必須！！
[RequireComponent(typeof(Camera))]

public class TPFCamera : MonoBehaviour
{
    [Header("カメラを付けるオブジェクトの名前")]
    public string objectName = "Player"; //オブジェクト名

    [Header("オブジェクトとカメラの距離")]
    public float distance = 5.0f; //オブジェクトとカメラの距離

    [Header("マウス感度設定")]
    public float sensitivity = 3.0f; //マウス感度

    [Header("カメラの注視点設定(オブジェクトからの相対位置を入れる)")]
    public Vector2 lookPoint = new Vector2(0.0f, 1.0f); //オブジェクトの中心からのカメラの位置

    // カメラを入れるオブジェクト
    GameObject obj;
    // カメラのトランスフォーム
    Transform cameraTrans;
    // カメラのベクトル(向き)
    Vector3 targetPos;

    // マウスの入力
    float mouseInputX;
    float mouseInputY;

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトを代入
        obj = GameObject.Find(objectName);

        // 見つからなければエラー
        if (obj == null)
        {
            Debug.LogError("TPFCamera ： プレイヤーオブジェクトが見つかりません");
            return;
        }

        // カメラのtransform代入
        cameraTrans = this.transform;

        // オブジェクトの座標にlookPointを計算した値を代入する
        targetPos = ExportTargetPos(obj);

        // カメラの位置計算   ===================================
        // オブジェクトのベクトル(向き)を取得
        Vector3 objOppositeVector = obj.transform.forward;
        // 逆ベクトルを計算
        objOppositeVector = objOppositeVector * -1;
        //ベクトルの長さをdistanceにする
        objOppositeVector = objOppositeVector.normalized * distance;

        // カメラの位置を代入
        cameraTrans.position = targetPos + objOppositeVector;
        //カメラのベクトル(向き)を計算
        cameraTrans.LookAt(targetPos);

        //カーソル非表示
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // ESCキーが押されたらロックを解除する
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible == false)
            {
                // カーソルを表示
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                //カーソル非表示
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // オブジェクトの座標にlookPointを計算した値を代入する
        Vector3 tpos = ExportTargetPos(obj);
        // キャラクターが移動していたら
        if (tpos != targetPos)
        {
            // 移動量を計算   
            Vector3 sa = targetPos - tpos;
            // カメラの位置を移動
            cameraTrans.position -= sa;
            // カメラの注視点を更新
            targetPos = tpos;
        }

        // マウス入力を取得
        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");

        // X方向にカメラを移動させる
        cameraTrans.RotateAround(targetPos, Vector3.up, mouseInputX * sensitivity);

        // Y方向にカメラを移動させる
        Vector3 oldPos = cameraTrans.position;
        Quaternion oldRot = cameraTrans.rotation;

        // マウス入力を反転させる
        mouseInputY *= -1;
        // カメラの回転を計算
        cameraTrans.RotateAround(targetPos, cameraTrans.right, mouseInputY * sensitivity);
        // カメラの角度を計算
        float camAngle = Mathf.Abs(Vector3.Angle(Vector3.up, targetPos - cameraTrans.position));

        //カメラの角度が一定範囲外なら動かさない
        if (camAngle < 45 || camAngle > 135)
        {
            cameraTrans.position = oldPos;
            cameraTrans.rotation = oldRot;
        }
        // カメラのZ軸方向を固定する
        cameraTrans.eulerAngles = new Vector3(cameraTrans.eulerAngles.x, cameraTrans.eulerAngles.y, 0.0f);
    }


    // オブジェクトからの相対的位置計算する
    Vector3 ExportTargetPos(GameObject obj)
    {
        // オブジェクトの位置
        Vector3 res = obj.transform.position;

        // X方向の計算
        res += obj.transform.right * lookPoint.x;
        // Y方向の計算
        res += obj.transform.up * lookPoint.y;
        return res;
    }
}
