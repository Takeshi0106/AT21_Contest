using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class AttackController : MonoBehaviour
{
    [Header("�U�����L���ȂƂ��ɕt����^�O�ꗗ")]
    [SerializeField] private List<string> attackTags = new List<string>();
    [Header("�U���������ȂƂ��ɕt����^�O�ꗗ")]
    [SerializeField] private List<string> noAttackTags = new List<string>();
    [Header("�U���̋O����ݒ�")]
    [SerializeField] private TrailRenderer[] trailArray;
    [Header("�U���̃p�[�e�B�N����ݒ�")]
    [SerializeField] private ParticleSystem[] particle;

    // �q�I�u�W�F�N�g���܂ޑS�Ă� MultiTag ��ۑ�
    private MultiTag[] multiTags;
    // �R���C�_�[��S�Ď擾����
    // private Collider[] colliders;
    private bool m_Start = false;



#if UNITY_EDITOR
    // �q�I�u�W�F�N�g��Renderer�ƌ��̐F��ۑ�����
    private Renderer[] renderers;
    private Color[] originalColors;
#endif



    void Start()
    {
        AttackControllerStart();
    }



    // �U������ON
    public void EnableAttack()
    {
        if (multiTags == null || multiTags.Length == 0)
        {
            if (multiTags == null)
            {
                Debug.Log("NULL");
            }
            Debug.LogWarning("[AttackController] MultiTag ������������Ă��Ȃ����A�q�I�u�W�F�N�g�ɑ��݂��܂���");
            return;
        }

        foreach (var mt in multiTags)
        {
            // ��U���ׂẴ^�O���폜
            List<string> currentTags = mt.GetTags();
            if (currentTags != null)
            {
                foreach (var tag in currentTags)
                {
                    mt.RemoveTag(tag);
                }
            }


            // �U���^�O��ǉ�
            foreach (var tag in attackTags)
            {
                mt.AddTag(tag);
            }
        }

        foreach (var trail in trailArray)
        {
            trail.emitting = true;
            // Debug.LogWarning("�g���C��ON");
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



    // �U������OFF
    public void DisableAttack()
    {
        foreach (var mt in multiTags)
        {
            // ��U���ׂẴ^�O���폜
            List<string> currentTags = mt.GetTags();
            if (currentTags != null)
            {
                foreach (var tag in currentTags)
                {
                    mt.RemoveTag(tag);
                }
            }

            // �U���������̎��̃^�O��ǉ�
            foreach (var tag in noAttackTags)
            {
                mt.AddTag(tag);
            }
        }

        foreach (var trail in trailArray)
        {
            trail.emitting = false;
            // Debug.LogWarning("�g���C��OFF");
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

        foreach (var trail in trailArray)
        {
            trail.emitting = false;
            // Debug.LogWarning("�g���C��OFF");
        }
        foreach (var par in particle)
        {
            par.Stop();
        }

        // �R���C�_�[����������
        // colliders = this.GetComponentsInChildren<Collider>();

        m_Start = true;

#if UNITY_EDITOR
        Debug.Log("START");

        if (multiTags == null)
        {
            Debug.LogError("[AttackController] multiTags �� null �ł�");
        }
        else if (multiTags.Length == 0)
        {
            Debug.LogWarning("[AttackController] MultiTag �����q�I�u�W�F�N�g�� 0 ���ł�");
        }
        else
        {
            Debug.Log($"[AttackController] MultiTag �����q�I�u�W�F�N�g��: {multiTags.Length}");
        }
        if (multiTags == null || multiTags.Length == 0)
        {
            Debug.LogWarning("AttackController : MultiTag �����q�I�u�W�F�N�g��������܂���");
        }

        // �q�I�u�W�F�N�g���܂߂�Renderer���擾
        renderers = this.GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null && renderers[i].material != null)
            {
                // originalColors[i] = renderers[i].material.color; // ���̐F��ۑ�
            }
        }
#endif
    }



}
