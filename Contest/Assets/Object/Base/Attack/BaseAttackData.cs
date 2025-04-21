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
    [Header("武器の攻撃前フレーム（攻撃判定がない時間）")]
    [SerializeField] private int[] attackStartupFruem = { 20, 20, 20, 20, 20 };
    [Header("武器の攻撃中フレーム（攻撃判定がある時間）")]
    [SerializeField] private int[] attackFruem = { 20, 20, 20, 20, 20 };
    [Header("武器の攻撃後フレーム（攻撃判定がない時間）")]
    [SerializeField] private int[] attackStaggerFruem = { 20, 20, 20, 20, 20 };
    [Header("攻撃の攻撃力")]
    [SerializeField] private float[] attackDamage = { 10.0f, 10.0f, 10.0f, 10.0f, 10.0f };
    [Header("各コンボに対応する攻撃中アニメーション")]
    [SerializeField] private AnimationClip[] attackAnimations;
    [Header("各コンボに対応する攻撃後硬直アニメーション")]
    [SerializeField] private AnimationClip[] attackStaggerAnimations;


    // ゲッター
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
