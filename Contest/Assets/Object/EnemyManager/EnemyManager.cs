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
        // ��A�N�e�B�u�ɂ���
        enemy.gameObject.SetActive(false);

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

}
