using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseScript : MonoBehaviour
{
    //[Header("ターゲット")]
    //[SerializeField] 
    private Transform target;

    private NavMeshAgent agent;

    public void SetTarget(GameObject _gameObject)
    {
        //ターゲットとagentが定義されていないなら定義する
        if (agent == null && target == null)
        {
            target = _gameObject.transform;
            Debug.Log("ターゲット取得:" + target.name);
            agent = GetComponent<NavMeshAgent>();
        }
    }

    
    public void SetStoppingDistance(float _stoppingDistance)
    {
        //停止距離を外から持ってきた値に設定する
        agent.stoppingDistance = _stoppingDistance;
        //Debug.Log("停止距離：" + agent.stoppingDistance);
    }


    public void TargetChase()
    {
        if(agent != null)
        {
            //Debug.LogError(name+":が処理している");
            agent.destination = target.position;
            //Debug.Log("変更後：" + agent.destination);
        }

    }

}
