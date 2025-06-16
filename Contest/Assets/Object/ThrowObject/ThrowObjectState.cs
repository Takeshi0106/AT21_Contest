using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowObjectState : BaseState<ThrowObjectState>
{
    // �C���X�y�N�^�[�r���[����ύX�ł���
    [Header("������͂�������")]
    [SerializeField] private float forcePower;
    [Header("���������̏d�͖����t���[��")]
    [SerializeField] private int noGravityFreams = 0;
    [Header("�X�e�[�W�̃^�O")]
    [SerializeField] private string stageTag = "Stage";
    [Header("�G�l�~�[�̃^�O")]
    [SerializeField] private string enemyTag = "Enemy";

    // �v���C���[�̃g�����X�t�H�[��������ϐ�
    private Transform playerTransform;
    // �����̃��W�b�h�{�f�B�[���擾���Ă���
    private Rigidbody throwObjectRigidbody;
    // �A�^�b�N�R���g���[���[���擾
    private AttackController throwAttackController;
    private AttackInterface attackInface = null;

    float damage = 0;
    float stanDamage = 0;

    // ���������^�O���擾���Ă������X�g
    protected List<MultiTag> collidedTags = new List<MultiTag>();


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("�J�n");

        // ���W�b�h�{�f�B�[���擾
        throwObjectRigidbody = this.gameObject.GetComponent<Rigidbody>();
        // �U���R���g���[���[���擾
        throwAttackController = this.gameObject.GetComponent<AttackController>();

        attackInface = this.gameObject.GetComponent<AttackInterface>();

        // ��Ԃ��Z�b�g
        currentState = new ThrowObjectThrowState();
        // ��Ԃ̊J�n����
        currentState.Enter(this);

        if(attackInface == null)
        {
            Debug.LogError("AttackInforse NULL");
        }
    }



    // Update is called once per frame
    void LateUpdate()
    {
        // ��ԍX�V
        StateUpdate();
    }



    // �������폜����
    public void ThrowObjectDelete()
    {
        Destroy(this.gameObject);
    }


    // �v���C���[���G�ɂԂ��������̏���
    void OnTriggerEnter(Collider other)
    {
        // MultiTag���擾
        MultiTag multiTag = other.transform.GetComponent<MultiTag>();

        if (multiTag != null)
        {
            // �^�O���X�g�ɒǉ�
            collidedTags.Add(multiTag);

#if UNITY_EDITOR
            Debug.Log("ThrowObject_Trigger�ɓ������� : " + other.gameObject.name);
#endif
        }
    }

    public override void ChangeState(StateClass<ThrowObjectState> newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // �Z�b�^�[
    public void SetPlayerTransfoem(Transform p_playerTransform) { playerTransform = p_playerTransform; }
    public void SetDamage(float _damage) { damage = _damage; }
    public void SetStanDamage(float _stanDamage) { stanDamage = _stanDamage; }

    // �Q�b�^�[
    public List<MultiTag> GetMultiTagList() { return collidedTags; }
    public Transform GetPlayerTransform() { return playerTransform; }
    public Rigidbody GetThrowObjectRigidbody() { return throwObjectRigidbody; }
    public float GetForcePower() { return forcePower; }
    public int GetNoGravityFreams() { return noGravityFreams; }
    public GameObject GetThrowObject() { return this.gameObject; }
    public string GetStageTag() { return stageTag; }
    public string GetEnemyTag() { return enemyTag; }
    public Transform GetThrowObjectTransform() { return this.transform; }
    public AttackController GetThrowAttackController() { return throwAttackController; }
    public float GetThrowDamage() { return damage; }
    public float GetThrowStanDamage() { return stanDamage; }
    public AttackInterface GetThrowAttackInterface() { return attackInface; }
}
