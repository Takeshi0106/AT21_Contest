using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceManager : MonoBehaviour
{
    [Header("����̊J�n�t���[��")]
    [SerializeField] private int avoidanceStartUpFreams = 10;
    [Header("��𒆂̃t���[��")]
    [SerializeField] private int avoidanceFreams = 30;
    [Header("�����̃t���[��")]
    [SerializeField] private int avoidanceAfterFreams = 20;
    [Header("��𐬌����̖��G�t���[��")]
    [SerializeField] private int avoidanceInvincibleFreams = 20;
    [Header("��𐬌����̓G�̃X�s�[�h�ቺ")]
    [SerializeField] private float avoidanceSlow = 0.8f;



    // �Q�b�^�[
    public int GetAvoidanceStartUpFreams() { return avoidanceStartUpFreams; }
    public int GetAvoidanceFreams() { return avoidanceFreams; }
    public int GetAvoidanceAfterFreams() { return avoidanceAfterFreams; }
    public int GetAvoidanceInvincibleFreams() { return avoidanceInvincibleFreams; }
    public float GetAvoidanceSlow() {  return avoidanceSlow; }
}
