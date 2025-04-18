using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponManager : MonoBehaviour
{
    [Header("持てる武器の数")]
    [SerializeField] private int weponNumber = 1;
    [Header("所持している武器データ（プレイヤー用は3つまで）")]
    [SerializeField] private List<BaseAttackData> weaponDataList = new List<BaseAttackData>();

    // 武器のオブジェクトを入れる
    private List<GameObject> instantiatedWeapons = new List<GameObject>();
    // 今持っている武器の番号
    int weaponHaveNumber = 0;

    // 現在の武器の Animator を保持
    private Animator currentWeaponAnimator;

    void Start()
    {
        if (weaponDataList.Count == 0) return;

        // 1つだけ装備
        ChangeWeapon(weaponHaveNumber);
    }



    // 武器を追加
    public void AddWeapon(BaseAttackData newWeaponData)
    {
        if (weaponDataList.Count >= weponNumber)
        {
#if UNITY_EDITOR
            Debug.LogWarning("武器の所持数が上限に達しています");
#endif
            return;
        }

        weaponDataList.Add(newWeaponData);
    }



    // 武器を削除
    public void RemoveWeapon(int index)
    {
        if (index < 0 || index >= instantiatedWeapons.Count)
        {
#if UNITY_EDITOR
            Debug.LogWarning("削除しようとした武器インデックスが無効です");
#endif
            return;
        }

        Destroy(instantiatedWeapons[index]);
        instantiatedWeapons.RemoveAt(index);
        weaponDataList.RemoveAt(index);

        // 次の武器を取得する
        ChangeWeapon(weaponHaveNumber);
    }



    private void AttachWeapon(BaseAttackData weaponData)
    {
        if (weaponData == null) return;

        string weaponName = weaponData.GetWeaponName();
        string path = $"Object/Weapon/{weaponName}";
        GameObject weaponPrefab = Resources.Load<GameObject>(path);

        if (weaponPrefab != null)
        {
            // 武器の削除  
            foreach (var oldWeapon in instantiatedWeapons)
            {
                if (oldWeapon != null)
                    Destroy(oldWeapon);
            }
            instantiatedWeapons.Clear();

            // 新しい武器の生成と親子関係の設定
            GameObject weaponInstance = Instantiate(weaponPrefab, transform);
            weaponInstance.name = weaponPrefab.name;
            instantiatedWeapons.Clear(); // 前のリストもクリア
            instantiatedWeapons.Add(weaponInstance);

            // Animator を取得・保存
            currentWeaponAnimator = weaponInstance.GetComponentInChildren<Animator>();

#if UNITY_EDITOR
            if (currentWeaponAnimator == null)
            {
                Debug.LogWarning($"武器 {weaponName} に Animator が存在しません");
            }
#endif
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{path} のプレハブが Resources に存在しません");
        }
#endif
    }



    public BaseAttackData GetWeaponData(int i)
    {
        return weaponDataList[i];
    }



    public void EnableAllWeaponAttacks()
    {
        foreach (var weapon in instantiatedWeapons)
        {
            var controllers = weapon.GetComponentsInChildren<AttackController>();
            foreach (var controller in controllers)
            {
                controller.EnableAttack();

#if UNITY_EDITOR
                // Rendererが存在するなら赤色に変更
                var renderer = controller.GetComponentInChildren<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.color = Color.red;
                }
#endif
            }
        }
    }



    public void DisableAllWeaponAttacks()
    {
        foreach (var weapon in instantiatedWeapons)
        {
            var controllers = weapon.GetComponentsInChildren<AttackController>();
            foreach (var controller in controllers)
            {
                controller.DisableAttack();
            }
        }
    }


    public void ChangeWeapon(int index)
    {
        if (index < 0 || index >= weaponDataList.Count)
        {
            Debug.LogWarning("切り替えようとした武器インデックスが無効です");
            return;
        }

        // 新しい武器を装備
        weaponHaveNumber = index;
        AttachWeapon(weaponDataList[weaponHaveNumber]);
    }


    // ゲッター
    public Animator GetCurrentWeaponAnimator() { return currentWeaponAnimator; }
}
