using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyBaseState<T> : BaseCharacterState<T> where T : EnemyBaseState<T>
{
    [Header("Playerの攻撃タグ名")]
    [SerializeField] protected string playerAttackTag = "PlayerAttack";
    [Header("Playerのカウンタータグ")]
    [SerializeField] protected string playerCounterTag = "CounterAttack";
    [Header("Playerの投げるタグ")]
    [SerializeField] protected string playerThrowTag = "ThrowAttack";

    [Header("Playerに倒されたときに渡す武器")]
    [SerializeField] protected BaseAttackData dropWeapon;
    [Header("倒した時に武器を渡すプレイヤーの名前")]
    [SerializeField] protected GameObject player;
    [Header("敵を管理するマネージャーの名前")]
    [SerializeField] protected GameObject enemyManagerObject;

    [Header("デバッグ用　敵の速度(0.01～1.00)")]
    [SerializeField] protected float enemySpeed = 1.0f;
    [Header("怯み時間")]
    [SerializeField] protected int flinchFreams = 10;

    [Header("立ち状態アニメーション")]
    [SerializeField] protected AnimationClip enemyStandingAnimation = null;
    [Header("死亡アニメーション")]
    [SerializeField] protected AnimationClip enemyDeadAnimation = null;
    [Header("怯みアニメーション")]
    [SerializeField] protected AnimationClip enemyFlinchAnimation = null;
    [Header("走るアニメーション")]
    [SerializeField] protected AnimationClip enemyDashAnimation = null;

    [Header("敵の走る移動速度")]
    [SerializeField] protected float dashSpeed = 4.0f;

    [Header("攻撃する距離")]
    [SerializeField] protected float attackDistance = 1.0f;

    [Header("視野角")]
    [SerializeField] protected float fov;
    [Header("視野の長さ")]
    [SerializeField] protected float visionLength;
    [Header("攻撃レンジ")]
    [SerializeField] protected float attackRange;
    [Header("移動速度")]
    [SerializeField] protected float moveSpeed;

    // プレイヤーを発見したかのフラグ
    protected bool foundTargetFlg;

    // 衝突したオブジェクトを保存するリスト
    [HideInInspector] protected List<Collider> collidedObjects = new List<Collider>();
    // Enemyのウェポンマネージャー
    [HideInInspector] protected WeponManager enemyWeponManager;
    // PlayerのState
    [HideInInspector] protected PlayerState playerState;
    // Enemyマネージャー
    [HideInInspector] protected EnemyManager enemyManager;
    // EnemyのHPマネージャー 
    protected HPManager hpManager;
    // Enemyのリジッドボディー
    protected Rigidbody enemyRigidbody;
    // Enemyのアニメーションを取得する
    protected Animator enemyAnimator;

    protected Transform playerTransform;

    // 現在のコンボ数
    protected int enemyConbo = 0;
    // 現在使っている武器のナンバー
    protected int weponNumber = 0;
    // カウンター攻撃で倒れたかのチェック
    protected bool hitCounter = false;
    protected bool damagerFlag = false;
    protected int flinchCnt = 0;
    protected bool attackFlag = false;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // Enemyのレンダラー
    [HideInInspector] public Renderer enemyRenderer;
#endif

    // Start is called before the first frame update
    public void EnemyStart()
    {
        // ウェポンマネージャー
        enemyWeponManager = this.gameObject.GetComponent<WeponManager>();
        // hpManager
        hpManager = this.gameObject.GetComponent<HPManager>();
        // PlayerState
        playerState = player.GetComponent<PlayerState>();
        // エネミーマネージャー
        enemyManager = enemyManagerObject.GetComponent<EnemyManager>();
        // リジッドボディーを取得
        enemyRigidbody = this.gameObject.GetComponent<Rigidbody>();
        // アニメーターを取得
        enemyAnimator = this.gameObject.GetComponent<Animator>();

        playerTransform = player.transform;



#if UNITY_EDITOR
        // エディタ実行時に取得して色を変更する
        enemyRenderer = this.gameObject.GetComponent<Renderer>();

        if (enemyWeponManager == null)
        {
            Debug.Log("EnemyState : WeponManagerが見つかりません");
            return;
        }
#endif
    }

    /*
    // 攻撃タグまたはカウンタータグが外れたら削除
    public void CleanupInvalidDamageColliders()
    {
        // タグが攻撃タグ以外の物かを調べる
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || (!tag.HasTag(playerAttackTag));
        });

        // コライダーが非アクティブ化を調べる
        damagedColliders.RemoveWhere(collider =>
        collider == null || !collider.gameObject.activeInHierarchy || !collider.enabled);

        // 無効なコライダーや非アクティブ化されたものも除外
        collidedInfos.RemoveAll(info =>
            info.collider == null ||
            !info.collider.gameObject.activeInHierarchy ||
            !info.collider.enabled);
    }
    */

    public void Target()
    {
        Vector3 direction = player.transform.position - transform.position;

        // Y方向の回転だけにしたい場合（上下は無視）
        direction.y = 0;

        // 向きたい方向が0ベクトルでないことを確認
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    

    // スピードをセットする処理
    public void SetEnemySpead(float speed)
    {
        if (speed >= 0.00f && speed <= 1.0f)
        {
            // フレームの進む処理を更新
            enemySpeed = speed;
            // アニメーションの速度を変更
            if (enemyAnimator != null)
            {
                enemyAnimator.speed = speed;
            }
        }
        else
        {
            Debug.Log("スピードがセットできませんでした。");
        }
    }


    // Enemy用のチェンジState　オーバーライド
    public override void ChangeState(StateClass<T> newState)
    {
        currentState.Exit((T)this);
        currentState = null;
        currentState = newState;
        currentState.Enter((T)this);
    }


    // セッター
    public void SetEnemyCombo(int combo) { enemyConbo = combo; }
    public void SetEnemyFlinchCnt(int cnt) { flinchCnt = cnt; }
    public void SetEnemyAttackFlag(bool flag) { attackFlag = flag; }
    public void SetFoundTargetFlg(bool _foundTargetFlg) { foundTargetFlg = _foundTargetFlg; }

    // ゲッター
    public List<Collider> GetEnemyCollidedObjects() { return collidedObjects; }
    public WeponManager GetEnemyWeponManager() { return enemyWeponManager; }
    public int GetEnemyConbo() { return enemyConbo; }
    public int GetEnemyWeponNumber() { return weponNumber; }
    public BaseAttackData GetDropWeapon() { return dropWeapon; }
    public string GetEnemyPlayerAttackTag() { return playerAttackTag; }
    public string GetEnemyPlayerCounterAttackTag() { return playerCounterTag; }
    public float GetEnemySpeed() { return enemySpeed; }
    public int GetEnemyFlinchFreams() { return flinchFreams; }
    public Animator GetEnemyAnimator() { return enemyAnimator; }
    public AnimationClip GetEnemyStandingAnimation() { return enemyStandingAnimation; }
    public AnimationClip GetEnemyDeadAnimation() { return enemyDeadAnimation; }
    public AnimationClip GetEnemyFlinchAnimation() { return enemyFlinchAnimation; }
    public bool GetEnemyDamageFlag() { return damagerFlag; }
    public bool GetEnemyHitCounterFlag() { return hitCounter; }
    public int GetEnemyFlinchCnt() { return flinchCnt; }
    public PlayerState GetPlayerState() { return playerState; }
    public float GetEnemyDashSpeed() { return dashSpeed; }
    public Rigidbody GetEnemyRigidbody() { return enemyRigidbody; }
    public bool GetEnemyAttackFlag() { return attackFlag; }
    public Transform GetPlayerTransform() { return playerTransform; }
    public AnimationClip GetEnemyDashAnimation() { return enemyDashAnimation; }
    public float GetDistanceAttack() { return attackDistance; }

    public float GetEnemyFov() { return fov; }
    public float GetEnemyVisionLength() { return visionLength; }
    public float GetEnemyAttackRange() { return attackRange; }
    public float GetEnemyMoveSpeed() { return moveSpeed; }
    public EnemyManager GetEnemyManager() { return enemyManager; }
    public bool GetFoundTargetFlg() { return foundTargetFlg; }
    public GameObject GetTargetObject() { return player; }

#if UNITY_EDITOR
    // エディタ実行時に実行される
    public Renderer GetEnemyRenderer() { return enemyRenderer; }
#endif
}
