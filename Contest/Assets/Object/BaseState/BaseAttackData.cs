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
    [Header("����̍U���t���[��")]
    [SerializeField] private int[] attackFruem = { 20, 20, 20, 20, 20 };
    [Header("����̍d���t���[��")]
    [SerializeField] private int[] attackStaggerFruem = { 20, 20, 20, 20, 20 };
    [Header("�U���̍U����")]
    [SerializeField] private float[] attackDamage = { 10.0f, 10.0f, 10.0f, 10.0f, 10.0f };


    // �R���{�i���ɉ������f�[�^���擾����
    public float GetMaxCombo() => maxCombo;

    public float GetDamage(int comboStep)
    {
        return (comboStep < maxCombo) ? attackDamage[comboStep] : attackDamage[maxCombo - 1];
    }

    public int GetAttackStaggerFrames(int comboStep)
    {
        return (comboStep < maxCombo) ? attackStaggerFruem[comboStep] : attackStaggerFruem[maxCombo - 1];
    }

    public int GetComboSuccessFrames(int comboStep)
    {
        return (comboStep < maxCombo) ? attackFruem[comboStep] : attackFruem[maxCombo - 1];
    }
}
