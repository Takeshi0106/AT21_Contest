using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    private List<EnemyState> enemies = new List<EnemyState>();

    public void RegisterEnemy(EnemyState enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyState enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            Debug.Log("‚·‚×‚Ä‚Ì“G‚ğ“|‚µ‚Ü‚µ‚½I");
            LoadResultScene();
        }
    }

    private void LoadResultScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("ResultScene");
    }
}
