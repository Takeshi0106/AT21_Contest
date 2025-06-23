using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ===============================
// EnemyManager
// ����������Enemy���Ăяo��
// ===============================

public class EnemyManager : MonoBehaviour
{
    private List<EnemyBaseState<EnemyState>> enemyList = new List<EnemyBaseState<EnemyState>>();
    private List<EnemyBaseState<BossState>> bossList = new List<EnemyBaseState<BossState>>();
    private UnityEvent<float> onEnemySlow = new UnityEvent<float>() { };
    EnemySystem enemyManager = null;

    [Header("�v���C���[�̃^�O��")]
    [SerializeField] private string playerTag = "Player";
    [Header("�G��system")]
    [SerializeField] private EnemySystem system = null;


    // �J�n����
    public void Start()
    {
        // EnemySystem�Ɏ�����n��
        enemyManager = system.GetComponent<EnemySystem>();
        enemyManager.RegisterEnemyManager(this);
    }


    // Enemy���擾
    public void RegisterEnemy(EnemyBaseState<EnemyState> enemy)
    {
        // Enemy��ǉ�����
        enemyList.Add(enemy);
    }
    // Boss���擾
    public void RegisterEnemy(EnemyBaseState<BossState> boss)
    {
        // Enemy��ǉ�����
        bossList.Add(boss);
    }


    // Enemy��|�����Ƃ��̏���
    public void UnregisterEnemy(EnemyBaseState<EnemyState> enemy)
    {
        enemyList.Remove(enemy);

        if (enemyList.Count + bossList.Count == 0)
        {
            // �G���|���ꂽ��EnemySystem�ɒm�点��
            enemyManager.UnregisterEnemyManager(this);
        }
    }
    // Boss��|�����Ƃ��̏���
    public void UnregisterEnemy(EnemyBaseState<BossState> boss)
    {
        bossList.Remove(boss);

        if (enemyList.Count + bossList.Count == 0)
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


    // Enemy�̃X�s�[�h�ቺ�֐��ǉ�
    public void AddOnEnemySlow(UnityAction<float> action)
    {
        onEnemySlow.AddListener(action);
    }
    // Enemy�̃X�s�[�h�ቺ�֐��폜
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
            // Enemy��L����
            foreach (EnemyBaseState<EnemyState> enemy in enemyList)
            {
                enemy.gameObject.SetActive(true);  // �o�^���ꂽ�G��L����
            }
            // Boss��L����
            foreach (EnemyBaseState<BossState> boss in bossList)
            {
                boss.gameObject.SetActive(true);  // �o�^���ꂽ�G��L����
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        MultiTag tag = other.GetComponent<MultiTag>();

        if (tag != null && tag.HasTag(playerTag))
        {
            // Enemy�𖳌���
            foreach (EnemyBaseState<EnemyState> enemy in enemyList)
            {
                if (enemy != null)
                {
                    enemy.ResetEnemy();
                    enemy.gameObject.SetActive(false);
                }
            }

            Debug.Log("�v���C���[���G���A�O�ɏo�����߁A�G���A�N�e�B�u�ɂ��܂����B");
        }
    }


    // �v���C���[�̈ʒu��n���āA�ł��߂� Enemy �̈ʒu��Ԃ�
    public Vector3 GetNearestEnemyPosition(Vector3 playerPosition)
    {
        // Boss��Enemy�̈�ԋ߂���������
        EnemyBaseState<EnemyState> enemyNearest = null;
        EnemyBaseState<BossState> bossNearest = null;
        float shortestDistance = float.MaxValue;

        // ��ԋ߂�Enemy��T��
        foreach (EnemyBaseState<EnemyState> enemy in enemyList)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                enemyNearest = enemy;
            }
        }
        // ��ԋ߂�Boss��T��
        foreach (EnemyBaseState<BossState> boss in bossList)
        {
            if (boss == null || !boss.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(playerPosition, boss.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                enemyNearest = null;
                bossNearest = boss;
            }
        }

        // ���������ꍇ�͂��̈ʒu��Ԃ��B
        if (enemyNearest != null)
        {
            return enemyNearest.transform.position;
        }
        else if(bossNearest != null)
        {
            return bossNearest.gameObject.transform.position;
        }

        return Vector3.zero;
    }


    // ��ԋ߂�Enemy�EBoss�Ƀt���O��n��
    public void NearEnemyFlag(Vector3 playerPosition)
    {
        EnemyBaseState<EnemyState> enemyNearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyBaseState<EnemyState> enemy in enemyList)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.SetEnemyAttackFlag(false);

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                enemyNearest = enemy;
            }
        }


        // Null�`�F�b�N��ǉ�
        if (enemyNearest != null)
        {
            enemyNearest.SetEnemyAttackFlag(true);
        }
    }


    public void EnemyFlagFalse()
    {
        foreach (EnemyBaseState<EnemyState> enemy in enemyList)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.SetEnemyAttackFlag(false);
        }
    }
}
