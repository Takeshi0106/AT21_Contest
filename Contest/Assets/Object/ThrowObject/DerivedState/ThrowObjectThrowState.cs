using UnityEngine;

// ==================================
// 投げられている状態
// ==================================

public class ThrowObjectThrowState : StateClass<ThrowObjectState>
{
    // インスタンスを入れる変数
    private static ThrowObjectThrowState instance;

    // フレーム
    int freams = 0;
    // 保存用フレーム
    int m_SaveFreams = 0;
    // 消えるときに当たらないことがあるため余分を持たせる
    bool m_DeleteFlag = false;

    // インスタンスを取得する関数
    public static ThrowObjectThrowState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThrowObjectThrowState();
            }
            return instance;
        }
    }



    // 状態を変更する
    public override void Change(ThrowObjectState state)
    {
        if (m_DeleteFlag && freams >= m_SaveFreams)
        {
            state.ChangeState(ThrowObjectDeleteState.Instance);
            Debug.Log("ThrowObject : 消える状態移行");
        }
    }



    // 状態の開始処理
    public override void Enter(ThrowObjectState state)
    {
        // 度のベクトルに飛ばすかの処理
        state.GetThrowObjectRigidbody().AddForce(state.GetPlayerTransform().forward * state.GetForcePower(),
    ForceMode.Impulse);

        state.GetThrowAttackController().EnableAttack();
    }



    // 状態中の処理
    public override void Excute(ThrowObjectState state)
    {
        // 重力を無効にする
        if (state.GetNoGravityFreams() > freams)
        {
            state.GetThrowObjectRigidbody().useGravity = false;
        }
        // 重力を有効にする
        else
        {
            state.GetThrowObjectRigidbody().useGravity = true;
        }

        freams++; // フレーム更新

        foreach (var tags in state.GetMultiTagList())
        {
            // エネミーかステージにぶつかった時
            if (tags.HasTag(state.GetEnemyTag()) ||
                (tags.HasTag(state.GetStageTag()) && state.GetNoGravityFreams() < freams) ||
                state.GetThrowObjectTransform().position.y < -200.0f)
            {
                m_SaveFreams = freams;
                m_DeleteFlag = true;
            }
        }
    }



    // 状態中の終了処理
    public override void Exit(ThrowObjectState state)
    {
        // 攻撃タグを元に戻す
        // state.GetThrowAttackController().DisableAttack();
    }



}
