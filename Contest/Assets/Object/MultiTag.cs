using System.Collections.Generic;
using UnityEngine;

// ==============================
// タグを制作するクラス
// ==============================

public class MultiTag : MonoBehaviour
{
    // カスタムタグリスト
    [SerializeField] private List<string> tags = new List<string>();



    // タグを追加
    public void AddTag(string tag)
    {
        if (!tags.Contains(tag))
        {
            tags.Add(tag);
        }
    }



    // タグを削除
    public void RemoveTag(string tag)
    {
        if (tags.Contains(tag))
        {
            tags.Remove(tag);
        }
    }



    // タグを持っているかチェック
    public bool HasTag(string tag)
    {
        return tags.Contains(tag);
    }



}
