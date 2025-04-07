using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "Attack System/Attack Data")]

public class BaseAttackData : ScriptableObject
{
    // インスペクタービューから変更できる
    [Header("武器の名前")]
    [SerializeField] private string weponName = "Sword";
    [Header("武器の最大コンボ数")]
    [SerializeField] private int maxCombo = 5;
    [Header("武器の攻撃フレーム")]
    [SerializeField] private int[] attackFruem = { 20, 20, 20, 20, 20 };
    [Header("武器の硬直フレーム")]
    [SerializeField] private int[] attackStaggerFruem = { 20, 20, 20, 20, 20 };
    [Header("攻撃の攻撃力")]
    [SerializeField] private float[] attackDamage = { 10.0f, 10.0f, 10.0f, 10.0f, 10.0f };


    // コンボ段数に応じたデータを取得する
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
