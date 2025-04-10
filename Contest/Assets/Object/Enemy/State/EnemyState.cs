using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================
// エネミーの状態
// =====================================

public class EnemyState : BaseState<EnemyState>
{
    // 衝突したオブジェクトを保存するリスト
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();
    // Enemyのウェポンマネージャー
    [HideInInspector] private WeponManager enemyWeponManager;

    // 現在のコンボ数
    private int enemyConbo = 0;
    // 現在使っている武器のナンバー
    private int weponNumber = 0;

#if UNITY_EDITOR
    // エディタ実行時に実行される
    // Enemyのレンダラー
    [HideInInspector] public Renderer enemyRenderer;
#endif

    // Start is called before the first frame update
    void Start()
    {
        // 状態をセット
        currentState = EnemyStandingState.Instance;

        // 状態の開始処理
        currentState.Enter(this);

        // ウェポンマネージャー
        enemyWeponManager = this.gameObject.GetComponent<WeponManager>();

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



    void Update()
    {
        StateUpdate();
    }



    // プレイヤーが敵にぶつかった時の処理
    void OnCollisionEnter(Collision other)
    {
        Collider collider = other.collider;
        if (!collidedObjects.Contains(other.collider))
        {
            collidedObjects.Add(other.collider);
        }
#if UNITY_EDITOR
        // エディタ実行時に実行される
        Debug.Log("Objectに当たった : " + other.gameObject.name);
#endif
    }



    // プレイヤーが敵と離れた時の処理
    void OnCollisionExit(Collision other)
    {
        Collider collider = other.collider;
        if (collidedObjects.Contains(other.collider))
        {
            collidedObjects.Remove(other.collider);
        }
#if UNITY_EDITOR
        // エディタ実行時に実行される
        Debug.Log("Objectが離れた : " + other.gameObject.name);
#endif
    }



    // セッター
    public void SetEnemyCombo(int value)
    {
        enemyConbo = value;
    }

    // ゲッター
    public  List<Collider>  GetEnemyCollidedObjects()  { return collidedObjects; }
    public  WeponManager    GetEnemyWeponManager()     { return enemyWeponManager; }
    public  int             GetEnemyConbo()            { return enemyConbo; }
    public  int             GetEnemyWeponNumber()      { return weponNumber; }
#if UNITY_EDITOR
    // エディタ実行時に実行される
    public  Renderer        GetEnemyRenderer()         { return enemyRenderer; }
#endif
}
