using UnityEngine;

// ===============================
// 武器を投げる状態
// ===============================

public class PlayerWeaponThrowState : StateClass<PlayerState>
{
    // インスタンスを入れる変数
    private static PlayerWeaponThrowState instance;
    // 武器の情報
    private static BaseAttackData weponData;
    // 武器のオブジェクトを入れる変数
    private static GameObject prefab = null;

    // 武器の投げる位置を入れる変数
    private Vector3 weaponPos;

    // 投げる武器の投げるまでのフレーム
    int throwStartUpFreams = 0;
    // 投げた後の硬直フレーム
    int throwStaggerFreams = 0;

    // 状態変更までの時間
    float freams = 0.0f;
    // 投げたかどうかのフラグ
    bool throwFlag = false;



    // インスタンスを取得する
    public static PlayerWeaponThrowState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerWeaponThrowState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(PlayerState playerState)
    {
        // 立ち状態に戻す
        if (freams > throwStartUpFreams + throwStaggerFreams)
        {
            playerState.ChangeState(PlayerStandingState.Instance);
            return;
        }
    }



    // 状態の開始処理
    public override void Enter(PlayerState playerState)
    {
        // 武器データを取得する
        weponData = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber());

        // アニメーションを取得しておく
        AnimationClip animClip = weponData.GetThrowAnimation();
        var anim = playerState.GetPlayerAnimator();

        // Weponのフレーム数を取得しておく
        throwStartUpFreams = weponData.GetThrowStartUp();
        throwStaggerFreams = weponData.GetThrowStagger();

        // 開始位置を取得しておく
        weaponPos = weponData.GetThrowStartPosition();

        // 武器を取得する
        prefab = Resources.Load<GameObject>($"Object/ThrowWeapon/{weponData.GetThrowWeaponName()}");

        // アニメーション開始処理
        if (animClip != null && anim != null)
        {
            anim.CrossFade(animClip.name, 0.2f);
        }

#if UNITY_EDITOR
        Debug.LogError("PlayerWeaponThrowState : 開始");

        // エディタ実行時に取得して色を変更する
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.green;
        }

        if (prefab == null)
        {
            Debug.LogError("ThrowWeapon が見つかりません");
        }

#endif
    }



    // 状態中の処理
    public override void Excute(PlayerState playerState)
    {
        // ダメージ処理を有効にする
        playerState.HandleDamage();

        // 投げる
        if (freams > throwStartUpFreams && !throwFlag)
        {

            throwFlag = true;

            // 投げる武器のオブジェクトを取得する
            if (prefab != null)
            {
                // 投げる位置のグローバルな位置を計算する
                Vector3 worldThrowPos = playerState.GetPlayerTransform().position
                      + (playerState.GetPlayerTransform().forward * weaponPos.z)
                      + (playerState.GetPlayerTransform().right * weaponPos.x)
                      + (playerState.GetPlayerTransform().up * weaponPos.y);

                // オブジェクトを生成する
                GameObject thrownObj = GameObject.Instantiate(prefab, worldThrowPos, Quaternion.identity);

                // プレハブの元の回転を取得
                Quaternion originalRotation = prefab.transform.rotation;

                // プレイヤーの forward を向く回転
                Quaternion lookRotation = Quaternion.LookRotation(playerState.GetPlayerTransform().forward);

                // 回転を合成する（lookRotation を基準にして originalRotation を追加）
                thrownObj.transform.rotation = lookRotation * originalRotation;

                // プレイヤーのトランスフォームをセットする
                thrownObj.GetComponent<ThrowObjectState>().SetPlayerTransfoem(playerState.GetPlayerTransform());
            }

            // 装備から削除
            playerState.GetPlayerWeponManager().RemoveWeapon(playerState.GetPlayerWeponNumber());

#if UNITY_EDITOR
            if (playerState.GetPlayerRenderer() != null)
            {
                playerState.GetPlayerRenderer().material.color = Color.blue;
            }
#endif
        }

        freams += playerState.GetPlayerSpeed();
    }



    // 状態中の終了処理
    public override void Exit(PlayerState playerState)
    {
        freams = 0.0f;
        throwFlag = false;

#if UNITY_EDITOR
        // エディタ実行時に色を元に戻す
        if (playerState.GetPlayerRenderer() != null)
        {
            playerState.GetPlayerRenderer().material.color = Color.white;
        }
#endif
    }



}
