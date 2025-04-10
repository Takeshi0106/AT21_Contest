using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponManager : MonoBehaviour
{
    [Header("持てる武器の数")]
    [SerializeField] private int weponNumber = 1;
    [Header("所持している武器データ（プレイヤー用は3つまで）")]
    [SerializeField] private List<BaseAttackData> weaponDataList = new List<BaseAttackData>();

    private List<GameObject> instantiatedWeapons = new List<GameObject>();



    void Start()
    {
        AttachWeapons();
    }



    // 最初の武器を生成
    public void AttachWeapons()
    {
        foreach (var weaponData in weaponDataList)
        {
            AttachWeapon(weaponData);
        }
    }


    // 武器を追加
    public void AddWeapon(BaseAttackData newWeaponData)
    {
        if (weaponDataList.Count >= weponNumber)
        {
            Debug.LogWarning("武器の所持数が上限に達しています");
            return;
        }

        weaponDataList.Add(newWeaponData);
        AttachWeapon(newWeaponData);
    }



    // 武器を削除
    public void RemoveWeapon(int index)
    {
        if (index < 0 || index >= instantiatedWeapons.Count)
        {
            Debug.LogWarning("削除しようとした武器インデックスが無効です");
            return;
        }

        Destroy(instantiatedWeapons[index]);
        instantiatedWeapons.RemoveAt(index);
        weaponDataList.RemoveAt(index);
    }



    private void AttachWeapon(BaseAttackData weaponData)
    {
        if (weaponData == null) return;

        string weaponName = weaponData.GetWeaponName();
        // パスを取得
        string path = $"Object/Weapon/{weaponName}";
        GameObject weaponPrefab = Resources.Load<GameObject>(path);

        if (weaponPrefab != null)
        {
            GameObject weaponInstance = Instantiate(weaponPrefab, transform);
            instantiatedWeapons.Add(weaponInstance);
        }
        else
        {
            Debug.LogWarning($"'{weaponName}' が見つかりません");
        }
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



}
