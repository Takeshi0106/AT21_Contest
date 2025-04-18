using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponManager : MonoBehaviour
{
    [Header("���Ă镐��̐�")]
    [SerializeField] private int weponNumber = 1;
    [Header("�������Ă��镐��f�[�^�i�v���C���[�p��3�܂Łj")]
    [SerializeField] private List<BaseAttackData> weaponDataList = new List<BaseAttackData>();

    // ����̃I�u�W�F�N�g������
    private List<GameObject> instantiatedWeapons = new List<GameObject>();
    // �������Ă��镐��̔ԍ�
    int weaponHaveNumber = 0;

    // ���݂̕���� Animator ��ێ�
    private Animator currentWeaponAnimator;

    void Start()
    {
        if (weaponDataList.Count == 0) return;

        // 1��������
        ChangeWeapon(weaponHaveNumber);
    }



    // �����ǉ�
    public void AddWeapon(BaseAttackData newWeaponData)
    {
        if (weaponDataList.Count >= weponNumber)
        {
#if UNITY_EDITOR
            Debug.LogWarning("����̏�����������ɒB���Ă��܂�");
#endif
            return;
        }

        weaponDataList.Add(newWeaponData);
    }



    // ������폜
    public void RemoveWeapon(int index)
    {
        if (index < 0 || index >= instantiatedWeapons.Count)
        {
#if UNITY_EDITOR
            Debug.LogWarning("�폜���悤�Ƃ�������C���f�b�N�X�������ł�");
#endif
            return;
        }

        Destroy(instantiatedWeapons[index]);
        instantiatedWeapons.RemoveAt(index);
        weaponDataList.RemoveAt(index);

        // ���̕�����擾����
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
            // ����̍폜  
            foreach (var oldWeapon in instantiatedWeapons)
            {
                if (oldWeapon != null)
                    Destroy(oldWeapon);
            }
            instantiatedWeapons.Clear();

            // �V��������̐����Ɛe�q�֌W�̐ݒ�
            GameObject weaponInstance = Instantiate(weaponPrefab, transform);
            weaponInstance.name = weaponPrefab.name;
            instantiatedWeapons.Clear(); // �O�̃��X�g���N���A
            instantiatedWeapons.Add(weaponInstance);

            // Animator ���擾�E�ۑ�
            currentWeaponAnimator = weaponInstance.GetComponentInChildren<Animator>();

#if UNITY_EDITOR
            if (currentWeaponAnimator == null)
            {
                Debug.LogWarning($"���� {weaponName} �� Animator �����݂��܂���");
            }
#endif
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning($"{path} �̃v���n�u�� Resources �ɑ��݂��܂���");
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


    public void ChangeWeapon(int index)
    {
        if (index < 0 || index >= weaponDataList.Count)
        {
            Debug.LogWarning("�؂�ւ��悤�Ƃ�������C���f�b�N�X�������ł�");
            return;
        }

        // �V��������𑕔�
        weaponHaveNumber = index;
        AttachWeapon(weaponDataList[weaponHaveNumber]);
    }


    // �Q�b�^�[
    public Animator GetCurrentWeaponAnimator() { return currentWeaponAnimator; }
}
