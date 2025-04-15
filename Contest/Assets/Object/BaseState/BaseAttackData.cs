using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "Attack System/Attack Data")]

public class BaseAttackData : ScriptableObject
{
    // �C���X�y�N�^�[�r���[����ύX�ł���
    [Header("����̖��O")]
    [SerializeField] private string weponName = "Sword";
    [Header("����̍ő�R���{��")]
    [SerializeField] private int maxCombo = 5;
    [Header("����̍U���O�t���[���i�U�����肪�Ȃ����ԁj")]
    [SerializeField] private int[] attackStartupFruem = { 20, 20, 20, 20, 20 };
    [Header("����̍U�����t���[���i�U�����肪���鎞�ԁj")]
    [SerializeField] private int[] attackFruem = { 20, 20, 20, 20, 20 };
    [Header("����̍U����t���[���i�U�����肪�Ȃ����ԁj")]
    [SerializeField] private int[] attackStaggerFruem = { 20, 20, 20, 20, 20 };
    [Header("�U���̍U����")]
    [SerializeField] private float[] attackDamage = { 10.0f, 10.0f, 10.0f, 10.0f, 10.0f };
    [Header("�e�R���{�ɑΉ�����U�����A�j���[�V����")]
    [SerializeField] private AnimationClip[] attackAnimations;
    [Header("�e�R���{�ɑΉ�����U����d���A�j���[�V����")]
    [SerializeField] private AnimationClip[] attackStaggerAnimations;


    // �Q�b�^�[
    public string GetWeaponName() => weponName;

    public float GetMaxCombo() => maxCombo;

    public float GetDamage(int comboStep)
    {
        return (comboStep < maxCombo) ? attackDamage[comboStep] : attackDamage[maxCombo - 1];
    }

    public int GetAttackStaggerFrames(int comboStep)
    {
        return (comboStep < maxCombo) ? attackStaggerFruem[comboStep] : attackStaggerFruem[maxCombo - 1];
    }

    public int GetAttackSuccessFrames(int comboStep)
    {
        return (comboStep < maxCombo) ? attackFruem[comboStep] : attackFruem[maxCombo - 1];
    }

    public int GetAttackStartupFrames(int comboStep)
    {
        return (comboStep < maxCombo) ? attackStartupFruem[comboStep] : attackStartupFruem[maxCombo - 1];
    }
    
    public AnimationClip GetAttackAnimation(int comboStep)
    {
        return (comboStep < attackAnimations.Length) ? attackAnimations[comboStep] : null;
    }

    public AnimationClip GetAttackStaggerAnimation(int comboStep)
    {
        return (comboStep < attackStaggerAnimations.Length) ? attackStaggerAnimations[comboStep] : null;
    }
}
