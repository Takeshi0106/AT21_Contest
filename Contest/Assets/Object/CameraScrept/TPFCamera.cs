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
    public float distance = 3.0f; //オブジェクトとカメラの距離

    [Header("マウス感度設定")]
    public float sensitivity = 3.0f; //マウス感度

    [Header("カメラの注視点設定(オブジェクトからの相対位置)")]
    public Vector2 lookPoint = new Vector2(0.7f, 1.0f); //オブジェクトの中心からのカメラの位置

    GameObject obj; //カメラを入れるオブジェクト
    Transform cameraTrans; //カメラのトランスフォーム
    Vector3 targetPos; //カメラのベクトルを入れる

    // マウスの入力を代入する
    float mouseInputX;
    float mouseInputY;

    // Start is called before the first frame update
    void Start()
    {
        //名前検索でオブジェクトを見つける
        obj = GameObject.Find(objectName);
        if (obj == null) //プレイヤーオブジェクトが見つからなければエラー
        {
            Debug.LogError("プレイヤーオブジェクトが見つかりません");
            return;
        }

        //自身のtransformを取得しておく
        cameraTrans = this.transform;

        //カメラの注視点を決める（キャラクターの少し右上）
        targetPos = ExportTargetPos(obj); //キャラクターの座標を取得

        //カメラの位置を決める（キャラクタの後ろ方向へkyoriだけ移動させた地点）
        Vector3 k = obj.transform.forward; //キャラクターの正面方向のベクトルを取得
        k = k * -1; //-1を掛けてキャラクターの真後ろ方向のベクトルにする
        k = k.normalized * distance;//ベクトルの長さをkyoriにする
        cameraTrans.position = targetPos + k; //カメラの位置を決定する

        //カメラを注視点へ向ける
        cameraTrans.LookAt(targetPos);

        //カーソル非表示
        Cursor.visible = false;
        //カーソルのロック
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //キャラクターが移動していたら
        Vector3 tpos = ExportTargetPos(obj);
        if (tpos != targetPos)
        {
            //移動差を取得
            Vector3 sa = targetPos - tpos;

            //カメラの位置も同じだけ動かす
            cameraTrans.position -= sa;

            //カメラの注視点を更新
            targetPos = tpos;
        }

        //マウス入力を取得
        mouseInputX = Input.GetAxis("Mouse X"); //X方向
        mouseInputY = Input.GetAxis("Mouse Y"); //Y方向

        //X方向にカメラを移動させる
        cameraTrans.RotateAround(targetPos, Vector3.up, mouseInputX * sensitivity);

        //Y方向にカメラを移動させる
        Vector3 oldPos = cameraTrans.position;
        Quaternion oldRot = cameraTrans.rotation;

        mouseInputY *= -1;

        cameraTrans.RotateAround(targetPos, cameraTrans.right, mouseInputY * sensitivity);
        float camAngle = Mathf.Abs(Vector3.Angle(Vector3.up, targetPos - cameraTrans.position)); //カメラの角度を求める
        if (camAngle < 45 || camAngle > 135) //カメラの角度が一定範囲外なら動かさない
        {
            cameraTrans.position = oldPos;
            cameraTrans.rotation = oldRot;
        }

        //カメラがZ軸方向に回転しないようにする
        cameraTrans.eulerAngles = new Vector3(cameraTrans.eulerAngles.x, cameraTrans.eulerAngles.y, 0.0f);
    }


    //カメラの注視点を取得する
    Vector3 ExportTargetPos(GameObject obj)
    {
        Vector3 res = obj.transform.position; //プレイヤーの位置

        res += obj.transform.right * lookPoint.x; //注視点調整（X方向）
        res += obj.transform.up * lookPoint.y; //注視点調整（Y方向）

        return res; //計算結果を返す
    }
}
