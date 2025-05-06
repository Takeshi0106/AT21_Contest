using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    private List<EnemyState> enemies = new List<EnemyState>();
    public UnityEvent<float> onEnemySlow;


    public void RegisterEnemy(EnemyState enemy)
    {
        enemies.Add(enemy);
    }



    public void UnregisterEnemy(EnemyState enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            Debug.Log("すべての敵を倒しました！");
            LoadResultScene();
        }
    }



    private void LoadResultScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("ResultScene");
    }


    // セットされているエネミーを
    public void EnemySlow(float slowSpead)
    {
        onEnemySlow.Invoke(slowSpead);
    }



    public void AddOnEnemySlow(UnityAction<float> action)
    {
        onEnemySlow.AddListener(action);
    }



}
