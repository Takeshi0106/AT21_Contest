using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowObjectState : BaseState<ThrowObjectState>
{
    // インスペクタービューから変更できる
    [Header("投げる力を代入する")]
    [SerializeField] private float forcePower;
    [Header("投げた時の重力無効フレーム")]
    [SerializeField] private int noGravityFreams = 0;
    [Header("ステージのタグ")]
    [SerializeField] private string stageTag = "Stage";
    [Header("エネミーのタグ")]
    [SerializeField] private string enemyTag = "Enemy";

    // プレイヤーのトランスフォームを入れる変数
    private Transform playerTransform;
    // 自分のリジッドボディーを取得しておく
    private Rigidbody throwObjectRigidbody;
    // アタックコントローラーを取得
    private AttackController throwAttackController;
    private AttackInterface attackInface = null;

    float damage = 0;
    float stanDamage = 0;

    // 当たったタグを取得しておくリスト
    protected List<MultiTag> collidedTags = new List<MultiTag>();


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("開始");

        // リジッドボディーを取得
        throwObjectRigidbody = this.gameObject.GetComponent<Rigidbody>();
        // 攻撃コントローラーを取得
        throwAttackController = this.gameObject.GetComponent<AttackController>();

        attackInface = this.gameObject.GetComponent<AttackInterface>();

        // 状態をセット
        currentState = new ThrowObjectThrowState();
        // 状態の開始処理
        currentState.Enter(this);

        if(attackInface == null)
        {
            Debug.LogError("AttackInforse NULL");
        }
    }



    // Update is called once per frame
    void LateUpdate()
    {
        // 状態更新
        StateUpdate();
    }



    // 自分を削除する
    public void ThrowObjectDelete()
    {
        Destroy(this.gameObject);
    }


    // プレイヤーが敵にぶつかった時の処理
    void OnTriggerEnter(Collider other)
    {
        // MultiTagを取得
        MultiTag multiTag = other.transform.GetComponent<MultiTag>();

        if (multiTag != null)
        {
            // タグリストに追加
            collidedTags.Add(multiTag);

#if UNITY_EDITOR
            Debug.Log("ThrowObject_Triggerに当たった : " + other.gameObject.name);
#endif
        }
    }

    public override void ChangeState(StateClass<ThrowObjectState> newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // セッター
    public void SetPlayerTransfoem(Transform p_playerTransform) { playerTransform = p_playerTransform; }
    public void SetDamage(float _damage) { damage = _damage; }
    public void SetStanDamage(float _stanDamage) { stanDamage = _stanDamage; }

    // ゲッター
    public List<MultiTag> GetMultiTagList() { return collidedTags; }
    public Transform GetPlayerTransform() { return playerTransform; }
    public Rigidbody GetThrowObjectRigidbody() { return throwObjectRigidbody; }
    public float GetForcePower() { return forcePower; }
    public int GetNoGravityFreams() { return noGravityFreams; }
    public GameObject GetThrowObject() { return this.gameObject; }
    public string GetStageTag() { return stageTag; }
    public string GetEnemyTag() { return enemyTag; }
    public Transform GetThrowObjectTransform() { return this.transform; }
    public AttackController GetThrowAttackController() { return throwAttackController; }
    public float GetThrowDamage() { return damage; }
    public float GetThrowStanDamage() { return stanDamage; }
    public AttackInterface GetThrowAttackInterface() { return attackInface; }
}
