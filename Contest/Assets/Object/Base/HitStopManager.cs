using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    [Header("プレイヤー")]
    [SerializeField] private GameObject playerObj = null;
    [Header("敵のシステム")]
    [SerializeField] private GameObject enemySystemObj = null;

    private PlayerState m_PlayerState;
    private EnemySystem m_EnemySystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
