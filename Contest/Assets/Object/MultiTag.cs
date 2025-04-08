using System.Collections.Generic;
using UnityEngine;

// ==============================
// �^�O�𐧍삷��N���X
// ==============================

public class MultiTag : MonoBehaviour
{
    // �J�X�^���^�O���X�g
    [SerializeField] private List<string> tags = new List<string>();



    // �^�O��ǉ�
    public void AddTag(string tag)
    {
        if (!tags.Contains(tag))
        {
            tags.Add(tag);
        }
    }



    // �^�O���폜
    public void RemoveTag(string tag)
    {
        if (tags.Contains(tag))
        {
            tags.Remove(tag);
        }
    }



    // �^�O�������Ă��邩�`�F�b�N
    public bool HasTag(string tag)
    {
        return tags.Contains(tag);
    }



}
