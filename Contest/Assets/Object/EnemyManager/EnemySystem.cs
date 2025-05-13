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
    private List<EnemyManager> enemyMan = new List<EnemyManager>();


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

        SceneManager.LoadScene("ResultScene");
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
}
