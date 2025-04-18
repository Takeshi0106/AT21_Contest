using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AttackController : MonoBehaviour
{
    [Header("攻撃が有効なときに付けるタグ一覧")]
    [SerializeField] private List<string> attackTags = new List<string>();
    [Header("攻撃が無効なときに付けるタグ一覧")]
    [SerializeField] private List<string> noAttackTags = new List<string>();

    // 子オブジェクトを含む全ての MultiTag を保存
    private MultiTag[] multiTags;
    // コライダーを全て取得する
    // private Collider[] colliders;

    // 子オブジェクトのRendererと元の色を保存する
    private Renderer[] renderers;
    private Color[] originalColors;

#if UNITY_EDITOR

#endif



    void Start()
    {
        // 子オブジェクトも含めて全ての MultiTag を取得
        multiTags = this.GetComponentsInChildren<MultiTag>();

        // 攻撃が無効の時のタグを追加
        foreach (var mt in multiTags)
        {
            foreach (var tag in noAttackTags)
            {
                mt.AddTag(tag);
            }
        }

        // コライダーを所得する
        // colliders = this.GetComponentsInChildren<Collider>();

        // 子オブジェクトも含めたRendererを取得
        renderers = this.GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

#if UNITY_EDITOR
        if (multiTags == null || multiTags.Length == 0)
        {
            Debug.LogWarning("AttackController : MultiTag を持つ子オブジェクトが見つかりません");
        }

        if (this == null)
        {
            Debug.LogError("AttackController : 攻撃判定オブジェクトが見つかりません");
            return;
        }
        /*
        if (multiTag == null)
        {
            Debug.LogError("AttackController : タグクラスが見つかりません");
            return;
        }
        */
        
#endif
    }



    // 攻撃判定ON
    public void EnableAttack()
    {
        foreach (var mt in multiTags)
        {
            // 一旦すべてのタグを削除
            List<string> currentTags = mt.GetTags();
            foreach (var tag in currentTags)
            {
                mt.RemoveTag(tag);
            }


            // 攻撃タグを追加
            foreach (var tag in attackTags)
            {
                mt.AddTag(tag);
            }
        }

        /*
        // isTrigger を ON
        foreach (var col in colliders)
        {
            col.isTrigger = true;
        }
        */

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = Color.red;
        }

#if UNITY_EDITOR

#endif
    }



    // 攻撃判定OFF
    public void DisableAttack()
    {
        foreach (var mt in multiTags)
        {
            // 一旦すべてのタグを削除
            List<string> currentTags = mt.GetTags();
            foreach (var tag in currentTags)
            {
                mt.RemoveTag(tag);
            }

            // 攻撃が無効の時のタグを追加
            foreach (var tag in noAttackTags)
            {
                mt.AddTag(tag);
            }
        }

        /*
        // isTrigger を OFF に戻す
        foreach (var col in colliders)
        {
            col.isTrigger = false;
        }
        */

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }

#if UNITY_EDITOR

#endif
    }



}
