using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// =====================================
// エネミーの状態
// =====================================

public class EnemyState : BaseState<EnemyState>
{
    [Header("Playerの攻撃タグ名")]
    [SerializeField] private string playerAttackTag = "PlayerAttack";
    [Header("Playerのカウンタータグ")]
    [SerializeField] private string playerCounterTag = "CounterAttack";
    [Header("Playerに倒されたときに渡す武器")]
    [SerializeField] private BaseAttackData dropWeapon;
    [Header("倒した時に武器を渡すプレイヤー")]
    [SerializeField] private GameObject player;


    // 衝突したオブジェクトを保存するリスト
    [HideInInspector] private List<Collider> collidedObjects = new List<Collider>();
    // Enemyのウェポンマネージャー
    [HideInInspector] private WeponManager enemyWeponManager;
    // PlayerのState
    [HideInInspector] private PlayerState playerState;

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
        // hpManager
        hpManager = this.gameObject.GetComponent<HPManager>();
        // PlayerState
        playerState = player.GetComponent<PlayerState>();

        // HPマネージャーにDie関数を渡す
        hpManager.onDeath.AddListener(Die);

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



    public void HandleDamage(string getAttackTags, string counterAttackTags)
    {
        foreach (var info in collidedInfos)
        {
            // すでにダメージ処理済みの場合スキップ
            if (damagedColliders.Contains(info.collider))
                continue;
            // タグがない場合スキップ
            if (info.multiTag == null)
                continue;

            // PlayerAttackがあるかの判定
            if (info.multiTag.HasTag(getAttackTags))
            {
                // 基本ダメージを入れる
                float baseDamage = 0;
                // 攻撃力アップ倍率を取得する
                float multiplier = 0;
                // 最終ダメージを計算する
                float finalDamage = 0;

                // カウンタータグがあるか調べる
                bool isCounterAttack = info.multiTag.HasTag(counterAttackTags);
                
                // 一度ダメージ処理したコライダーを保存する
                damagedColliders.Add(info.collider);

                // カウンター処理
                if (isCounterAttack)
                {
                    // カウンターの攻撃力を取得する
                    baseDamage = playerState.GetPlayerCounterManager().GetCounterDamage();
                    // Playerの攻撃力アップ倍率を取得する
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                }
                // 通常攻撃処理
                else
                {
                    // Playerの攻撃力を取得する
                    baseDamage = playerState.GetPlayerWeponManager().GetWeaponData(playerState.GetPlayerWeponNumber())
                        .GetDamage(playerState.GetPlayerConbo());
                    // Playerの攻撃力アップ倍率を取得する
                    multiplier = playerState.GetPlayerCounterManager().GetDamageMultiplier();
                }

                // ダメージを計算する
                finalDamage = baseDamage * multiplier;
                // ダメージ処理
                hpManager.TakeDamage(finalDamage);
                // ログを出力する
                Debug.Log("HP " + hpManager.GetCurrentHP());

                break;
            }
        }

        CleanupInvalidDamageColliders(getAttackTags, counterAttackTags);
    }




    // 攻撃タグまたはカウンタータグが元に戻るまで
    public void CleanupInvalidDamageColliders(string getAttackTags, string counterAttackTags)
    {
        damagedColliders.RemoveWhere(collider =>
        {
            var tag = collidedInfos.FirstOrDefault(info => info.collider == collider).multiTag;
            return tag == null || (!tag.HasTag(getAttackTags) && !tag.HasTag(counterAttackTags));
        });
    }



    private void Die()
    {
        Debug.Log($"{gameObject.name} が死亡しました");

        if (player != null && dropWeapon != null)
        {
            playerState.GetPlayerWeponManager().AddWeapon(dropWeapon);
        }

        gameObject.SetActive(false);
    }



    // セッター
    public void SetEnemyCombo(int value) { enemyConbo = value; }

    // ゲッター
    public  List<Collider>  GetEnemyCollidedObjects()  { return collidedObjects; }
    public  WeponManager    GetEnemyWeponManager()     { return enemyWeponManager; }
    public  int             GetEnemyConbo()            { return enemyConbo; }
    public  int             GetEnemyWeponNumber()      { return weponNumber; }
    public BaseAttackData GetDropWeapon() { return dropWeapon; }
    public string GetEnemyPlayerAttackTag() { return playerAttackTag; }
    public string GetEnemyPlayerCounterAttackTag() { return playerCounterTag; }
#if UNITY_EDITOR
    // エディタ実行時に実行される
    public  Renderer        GetEnemyRenderer()         { return enemyRenderer; }
#endif
}
