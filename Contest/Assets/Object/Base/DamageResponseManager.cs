using UnityEngine;

public class DamageResponseManager : MonoBehaviour
{
    [Header("ひるみゲージ最大値")]
    [SerializeField] private float flinchGage = 100;
    [Header("ひるみゲージ回復開始フレーム")]
    [SerializeField] private int flinchGageFreams = 60;
    [Header("ひるみゲージ1フレームの回復量")]
    [SerializeField] private float flinchGageRecover = 1;

    [Header("プレイヤーがひるんだ時のフレーム数")]
    [SerializeField] private int flinchFreams = 30;
    [Header("怯み状態アニメーション")]
    [SerializeField] private AnimationClip playerFlinchAnimation = null;

    private float m_CurentGage = 0.0f;
    private bool m_RecoverFlag = false;
    private float m_freams = 0.0f;
    

    // Update is called once per frame
    public void FlinchUpdate(float _characterFleams)
    {
        if (flinchGage == m_CurentGage) { return; }

        if (m_RecoverFlag)
        {
            m_CurentGage += flinchGageRecover;

            if (flinchGage <= m_CurentGage) { m_CurentGage = flinchGage; }
            return;
        }

        m_freams += _characterFleams;
        if (m_freams >= flinchGageFreams)
        {
            m_RecoverFlag = true;
        }
    }


    public bool FlinchDamage(float _flinchDamage)
    {
        m_RecoverFlag = false; // フラグをfalse
        m_freams = 0.0f;
        m_CurentGage -= _flinchDamage;

        if (m_CurentGage <= 0)
        {
            m_CurentGage = 0;
            return true;
        }

        return false;
    }


    public void RecoverFlinch()
    {
        m_CurentGage = flinchGage;
    }


    // ゲッター
    public int GetFlinchFreams() { return flinchFreams; }
    public AnimationClip GetFlinchAnimation() { return playerFlinchAnimation; }
}
