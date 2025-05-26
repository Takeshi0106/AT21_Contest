using UnityEngine;
using UnityEngine.AI;

public class ChaseScript : MonoBehaviour
{
    //[Header("�^�[�Q�b�g")]
    //[SerializeField] 
    private Transform target;

    private NavMeshAgent agent;

    public void SetTarget(GameObject _gameObject)
    {
        //�^�[�Q�b�g��agent����`����Ă��Ȃ��Ȃ��`����
        if (agent == null && target == null)
        {
            target = _gameObject.transform;
            Debug.Log("�^�[�Q�b�g�擾:" + target.name);
            agent = GetComponent<NavMeshAgent>();
        }
    }

    
    public void SetStoppingDistance(float _stoppingDistance)
    {
        //agent����`����Ă�����
        if (agent != null)
        {
            //��~�������O���玝���Ă����l�ɐݒ肷��
            agent.stoppingDistance = _stoppingDistance;
            //Debug.Log("��~�����F" + agent.stoppingDistance);
        }

    }

    public void SetMoveSpeed(float _moveSpeed)
    {
        //agent����`����Ă�����
        if (agent != null)
        {
            agent.speed = _moveSpeed;
        }
    }

    public void TargetChase()
    {
        //agent����`����Ă�����
        if (agent != null)
        {
            //Debug.LogError(name+":���������Ă���");
            agent.destination = target.position;
            //Debug.Log("�ύX��F" + agent.destination);
        }

    }

}
