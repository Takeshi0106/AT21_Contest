using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponManager : MonoBehaviour
{
    [Header("���Ă镐��̐�")]
    [SerializeField] private int weponNumber = 1;
    [Header("�������Ă��镐��f�[�^�i�v���C���[�p��3�܂Łj")]
    [SerializeField] private List<BaseAttackData> weaponDataList = new List<BaseAttackData>();

    private List<GameObject> instantiatedWeapons = new List<GameObject>();



    void Start()
    {
        AttachWeapons();
    }



    // �ŏ��̕���𐶐�
    public void AttachWeapons()
    {
        foreach (var weaponData in weaponDataList)
        {
            AttachWeapon(weaponData);
        }
    }


    // �����ǉ�
    public void AddWeapon(BaseAttackData newWeaponData)
    {
        if (weaponDataList.Count >= weponNumber)
        {
            Debug.LogWarning("����̏�����������ɒB���Ă��܂�");
            return;
        }

        weaponDataList.Add(newWeaponData);
        AttachWeapon(newWeaponData);
    }



    // ������폜
    public void RemoveWeapon(int index)
    {
        if (index < 0 || index >= instantiatedWeapons.Count)
        {
            Debug.LogWarning("�폜���悤�Ƃ�������C���f�b�N�X�������ł�");
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
        // �p�X���擾
        string path = $"Object/Weapon/{weaponName}";
        GameObject weaponPrefab = Resources.Load<GameObject>(path);

        if (weaponPrefab != null)
        {
            GameObject weaponInstance = Instantiate(weaponPrefab, transform);
            instantiatedWeapons.Add(weaponInstance);
        }
        else
        {
            Debug.LogWarning($"'{weaponName}' ��������܂���");
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
                // Renderer�����݂���Ȃ�ԐF�ɕύX
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
