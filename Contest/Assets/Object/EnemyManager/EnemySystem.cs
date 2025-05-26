using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// =====================================
// EnemyManager���Ǘ����āAresultscene���Ă�
// ======================================

public class EnemySystem : MonoBehaviour
{
    [Header("Player�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject player = null;

    private List<EnemyManager> enemyMan = new List<EnemyManager>();


    void Update()
    {
        EnemyManager nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyManager enemy in enemyMan)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            enemy.EnemyFlagFalse();

            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        nearest.NearEnemyFlag(player.transform.position);
    }



    // EnemyManager�̐����擾
    public void RegisterEnemyManager(EnemyManager enemy)
    {
        enemyMan.Add(enemy);
    }


    // resultscene�Ɉڍs
    private void LoadResultScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("PlayerWinScene");
    }


    // EnemyMangaer�̑S�Ă̓G��|�����Ƃ��̏���
    public void UnregisterEnemyManager(EnemyManager enemy)
    {
        enemyMan.Remove(enemy);

        if (enemyMan.Count == 0)
        {
            Debug.Log("���ׂĂ̓G��|���܂����I");
            LoadResultScene();
        }
    }


    // �Z�b�g����Ă���G�l�~�[��x������
    public void EnemySlow(float slowSpead)
    {
        foreach (EnemyManager enemy in enemyMan)
        {
            enemy.EnemySlow(slowSpead);
        }
    }


    // �v���C���[�̈ʒu��n���āA�ł��߂� EnemyState �̈ʒu��Ԃ�
    public Vector3 GetNearestEnemyPositionFromAll(Vector3 playerPosition)
    {
        EnemyManager nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (EnemyManager enemy in enemyMan)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        Vector3 nearestPosition = nearest.GetNearestEnemyPosition(playerPosition);

        return nearestPosition;
    }



}
