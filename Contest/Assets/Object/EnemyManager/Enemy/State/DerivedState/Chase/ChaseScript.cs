using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseScript : MonoBehaviour
{
    [Header("ターゲット")]
    [SerializeField] private Transform target;

    private NavMeshAgent agent;

    private void Start()
    {
        //ターゲットが取得されていたら
        if (target != null)
        {
           // Debug.LogError("ターゲット取得:" + target.name);
           //コンポーネント取得
            agent = GetComponent<NavMeshAgent>();
            //Debug.Log("停止距離：" + agent.stoppingDistance);

        }
    }

    public void SetStoppingDistance(float _stoppingDistance)
    {
        agent.stoppingDistance = _stoppingDistance;
        //Debug.Log("停止距離：" + agent.stoppingDistance);
    }


    public void TargetChase()
    {
        if(agent != null)
        {
            //Debug.LogError(name+":が処理している");
            agent.destination = target.position;
        }

    }




}
