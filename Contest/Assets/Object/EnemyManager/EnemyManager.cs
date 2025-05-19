using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// ===============================
// EnemyManager
// ����������Enemy���Ăяo��
// ===============================

public class EnemyManager : MonoBehaviour
{
    private List<EnemyState> enemies = new List<EnemyState>();
    private UnityEvent<float> onEnemySlow = new UnityEvent<float>() { };
    EnemySystem enemyManager = null;
    int cnt = 0;

    [Header("�v���C���[�̃^�O��")]
    [SerializeField] private string playerTag = "Player";
    [Header("�G��system")]
    [SerializeField] private EnemySystem system = null;


    // Enemy�𐔂��擾
    public void RegisterEnemy(EnemyState enemy)
    {
        // Enemy��ǉ�����
        enemies.Add(enemy);

        if (cnt == 0)
        {
            // EnemySystem�Ɏ�����n��
            enemyManager = system.GetComponent<EnemySystem>();
            enemyManager.RegisterEnemyManager(this);

            cnt = 1;
        }
    }


    // �G��|�����Ƃ��̏���
    public void UnregisterEnemy(EnemyState enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            // �G���|���ꂽ��EnemySystem�ɒm�点��
            enemyManager.UnregisterEnemyManager(this);
        }
    }


    // �Z�b�g����Ă���G�l�~�[��x������
    public void EnemySlow(float slowSpead)
    {
        onEnemySlow.Invoke(slowSpead);
    }



    public void AddOnEnemySlow(UnityAction<float> action)
    {
        onEnemySlow.AddListener(action);
    }


    public void RemoveOnEnemySlow(UnityAction<float> action)
    {
        onEnemySlow.RemoveListener(action);
    }

    // �v���C���[���G�ɂԂ��������̏���
    void OnTriggerEnter(Collider other)
    {
        MultiTag tag = other.GetComponent<MultiTag>();

        // �z��̒��ɓ������̂����邩�̃`�F�b�N
        if (tag.HasTag(playerTag))
        {
            foreach (EnemyState enemy in enemies)
            {
                enemy.gameObject.SetActive(true);  // �o�^���ꂽ�G��L����
            }
        }
    }



    // �v���C���[�̈ʒu��n���āA�ł��߂� Enemy �̈ʒu��Ԃ�
    public Vector3 GetNearestEnemyPosition(Vector3 playerPosition)
    {
        EnemyState nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyState enemy in enemies)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        // ���������ꍇ�͂��̈ʒu��Ԃ��B�Ȃ���� Vector3.zero ��Ԃ��i�v�����\�j
        return nearest != null ? nearest.transform.position : Vector3.zero;
    }


    public void NearEnemyFlag(Vector3 playerPosition)
    {
        EnemyState nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyState enemy in enemies)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.SetEnemyAttackFlag(false);

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }
        // Null�`�F�b�N��ǉ�
        if (nearest != null)
        {
            nearest.SetEnemyAttackFlag(true);
        }

    }


    public void EnemyFlagFalse()
    {
        foreach (EnemyState enemy in enemies)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.SetEnemyAttackFlag(false);
        }
    }
}
