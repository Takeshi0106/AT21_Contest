using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AttackController : MonoBehaviour
{
    [Header("�U�����L���ȂƂ��ɕt����^�O�ꗗ")]
    [SerializeField] private List<string> attackTags = new List<string>();
    [Header("�U���������ȂƂ��ɕt����^�O�ꗗ")]
    [SerializeField] private List<string> noAttackTags = new List<string>();

    // �q�I�u�W�F�N�g���܂ޑS�Ă� MultiTag ��ۑ�
    private MultiTag[] multiTags;
    // �R���C�_�[��S�Ď擾����
    // private Collider[] colliders;

#if UNITY_EDITOR
    // �q�I�u�W�F�N�g��Renderer�ƌ��̐F��ۑ�����
    private Renderer[] renderers;
    private Color[] originalColors;
#endif



    void Start()
    {
        // �q�I�u�W�F�N�g���܂߂đS�Ă� MultiTag ���擾
        multiTags = this.GetComponentsInChildren<MultiTag>();

        // �U���������̎��̃^�O��ǉ�
        foreach (var mt in multiTags)
        {
            foreach (var tag in noAttackTags)
            {
                mt.AddTag(tag);
            }
        }

        // �R���C�_�[����������
        // colliders = this.GetComponentsInChildren<Collider>();

#if UNITY_EDITOR
        if (multiTags == null || multiTags.Length == 0)
        {
            Debug.LogWarning("AttackController : MultiTag �����q�I�u�W�F�N�g��������܂���");
        }

        if (this == null)
        {
            Debug.LogError("AttackController : �U������I�u�W�F�N�g��������܂���");
            return;
        }
        /*
        if (multiTag == null)
        {
            Debug.LogError("AttackController : �^�O�N���X��������܂���");
            return;
        }
        */
        // �q�I�u�W�F�N�g���܂߂�Renderer���擾
        renderers = this.GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
#endif
    }



    // �U������ON
    public void EnableAttack()
    {
        foreach (var mt in multiTags)
        {
            // ��U���ׂẴ^�O���폜
            List<string> currentTags = mt.GetTags();
            foreach (var tag in currentTags)
            {
                mt.RemoveTag(tag);
            }

            // �U���^�O��ǉ�
            foreach (var tag in attackTags)
            {
                mt.AddTag(tag);
            }
        }

        /*
        // isTrigger �� ON
        foreach (var col in colliders)
        {
            col.isTrigger = true;
        }
        */

#if UNITY_EDITOR
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = Color.red;
        }
#endif
    }



    // �U������OFF
    public void DisableAttack()
    {
        foreach (var mt in multiTags)
        {
            // ��U���ׂẴ^�O���폜
            List<string> currentTags = mt.GetTags();
            foreach (var tag in currentTags)
            {
                mt.RemoveTag(tag);
            }

            // �U���������̎��̃^�O��ǉ�
            foreach (var tag in noAttackTags)
            {
                mt.AddTag(tag);
            }
        }

        /*
        // isTrigger �� OFF �ɖ߂�
        foreach (var col in colliders)
        {
            col.isTrigger = false;
        }
        */

#if UNITY_EDITOR
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
#endif
    }



}
