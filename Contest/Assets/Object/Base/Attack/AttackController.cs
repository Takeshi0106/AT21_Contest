using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class AttackController : MonoBehaviour
{
    [Header("攻撃が有効なときに付けるタグ一覧")]
    [SerializeField] private List<string> attackTags = new List<string>();
    [Header("攻撃が無効なときに付けるタグ一覧")]
    [SerializeField] private List<string> noAttackTags = new List<string>();
    [Header("攻撃の軌道を設定")]
    [SerializeField] private TrailRenderer[] trailArray;
    [Header("攻撃のパーティクルを設定")]
    [SerializeField] private ParticleSystem[] particle;

    // 子オブジェクトを含む全ての MultiTag を保存
    private MultiTag[] multiTags;
    // コライダーを全て取得する
    // private Collider[] colliders;
    private bool m_Start = false;



#if UNITY_EDITOR
    // 子オブジェクトのRendererと元の色を保存する
    private Renderer[] renderers;
    private Color[] originalColors;
#endif



    void Start()
    {
        AttackControllerStart();
    }



    // 攻撃判定ON
    public void EnableAttack()
    {
        if (multiTags == null || multiTags.Length == 0)
        {
            if (multiTags == null)
            {
                Debug.Log("NULL");
            }
            Debug.LogWarning("[AttackController] MultiTag が初期化されていないか、子オブジェクトに存在しません");
            return;
        }

        foreach (var mt in multiTags)
        {
            // 一旦すべてのタグを削除
            List<string> currentTags = mt.GetTags();
            if (currentTags != null)
            {
                foreach (var tag in currentTags)
                {
                    mt.RemoveTag(tag);
                }
            }


            // 攻撃タグを追加
            foreach (var tag in attackTags)
            {
                mt.AddTag(tag);
            }
        }

        foreach (var trail in trailArray)
        {
            trail.emitting = true;
            // Debug.LogWarning("トレイルON");
        }
        foreach (var par in particle)
        {
            par.Play();
        }

#if UNITY_EDITOR
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].material != null)
                {
                   // renderers[i].material.color = Color.red;
                }
            }
        }
#endif
    }



    // 攻撃判定OFF
    public void DisableAttack()
    {
        foreach (var mt in multiTags)
        {
            // 一旦すべてのタグを削除
            List<string> currentTags = mt.GetTags();
            if (currentTags != null)
            {
                foreach (var tag in currentTags)
                {
                    mt.RemoveTag(tag);
                }
            }

            // 攻撃が無効の時のタグを追加
            foreach (var tag in noAttackTags)
            {
                mt.AddTag(tag);
            }
        }

        foreach (var trail in trailArray)
        {
            trail.emitting = false;
            // Debug.LogWarning("トレイルOFF");
        }
        foreach (var par in particle)
        {
            par.Stop();
        }

#if UNITY_EDITOR
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    // renderers[i].material.color = originalColors[i];
                }
            }
        }
#endif
    }



    public void AttackControllerStart()
    {
        if (m_Start == true) { return; }

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

        foreach (var trail in trailArray)
        {
            trail.emitting = false;
            // Debug.LogWarning("トレイルOFF");
        }
        foreach (var par in particle)
        {
            par.Stop();
        }

        // コライダーを所得する
        // colliders = this.GetComponentsInChildren<Collider>();

        m_Start = true;

#if UNITY_EDITOR
        Debug.Log("START");

        if (multiTags == null)
        {
            Debug.LogError("[AttackController] multiTags が null です");
        }
        else if (multiTags.Length == 0)
        {
            Debug.LogWarning("[AttackController] MultiTag を持つ子オブジェクトが 0 件です");
        }
        else
        {
            Debug.Log($"[AttackController] MultiTag を持つ子オブジェクト数: {multiTags.Length}");
        }
        if (multiTags == null || multiTags.Length == 0)
        {
            Debug.LogWarning("AttackController : MultiTag を持つ子オブジェクトが見つかりません");
        }

        // 子オブジェクトも含めたRendererを取得
        renderers = this.GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null && renderers[i].material != null)
            {
                // originalColors[i] = renderers[i].material.color; // 元の色を保存
            }
        }
#endif
    }



}
