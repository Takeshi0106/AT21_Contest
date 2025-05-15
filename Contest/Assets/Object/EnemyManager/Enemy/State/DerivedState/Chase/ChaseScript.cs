using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseScript : MonoBehaviour
{
    [Header("�^�[�Q�b�g")]
    [SerializeField] private Transform target;

    private NavMeshAgent agent;

    private void Start()
    {
        //�^�[�Q�b�g���擾����Ă�����
        if (target != null)
        {
           // Debug.LogError("�^�[�Q�b�g�擾:" + target.name);
           //�R���|�[�l���g�擾
            agent = GetComponent<NavMeshAgent>();
            //Debug.Log("��~�����F" + agent.stoppingDistance);

        }
    }

    public void SetStoppingDistance(float _stoppingDistance)
    {
        agent.stoppingDistance = _stoppingDistance;
        //Debug.Log("��~�����F" + agent.stoppingDistance);
    }


    public void TargetChase()
    {
        if(agent != null)
        {
            //Debug.LogError(name+":���������Ă���");
            agent.destination = target.position;
        }

    }




}
