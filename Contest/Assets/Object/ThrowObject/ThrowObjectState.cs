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
    [SerializeField] private string stageTag = "Enemy";
    [Header("�G�l�~�[�̃^�O")]
    [SerializeField] private string enemyTag = "Stage";

    // �v���C���[�̃g�����X�t�H�[��������ϐ�
    private Transform cameraTransform;
    // �����̃��W�b�h�{�f�B�[���擾���Ă���
    private Rigidbody throwObjectRigidbody;

    // ���������^�O���擾���Ă������X�g
    protected List<MultiTag> collidedTags = new List<MultiTag>();


    // Start is called before the first frame update
    void Start()
    {
        // ���W�b�h�{�f�B�[���擾
        throwObjectRigidbody = this.gameObject.GetComponent<Rigidbody>();


        // ��Ԃ��Z�b�g
        currentState = ThrowObjectThrowState.Instance;
        // ��Ԃ̊J�n����
        currentState.Enter(this);
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


    // �Z�b�^�[
    public void SetCameraTransfoem(Transform p_playerTransform) { cameraTransform = p_playerTransform; }

    // �Q�b�^�[
    public List<MultiTag> GetMultiTagList() { return collidedTags; }
    public Transform GetCameraTransform() { return cameraTransform; }
    public Rigidbody GetThrowObjectRigidbody() { return throwObjectRigidbody; }
    public float GetForcePower() { return forcePower; }
    public int GetNoGravityFreams() { return noGravityFreams; }
    public GameObject GetThrowObject() { return this.gameObject; }
    public string GetStageTag() { return stageTag; }
    public string GetEnemyTag() { return enemyTag; }
}
