using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceManager : MonoBehaviour
{
    [Header("回避の開始フレーム")]
    [SerializeField] private int avoidanceStartUpFreams = 10;
    [Header("回避中のフレーム")]
    [SerializeField] private int avoidanceFreams = 30;
    [Header("回避後のフレーム")]
    [SerializeField] private int avoidanceAfterFreams = 20;
    [Header("回避成功時の無敵フレーム")]
    [SerializeField] private int avoidanceInvincibleFreams = 20;


    // ゲッター
    public int GetAvoidanceStartUpFreams() { return avoidanceStartUpFreams; }
    public int GetAvoidanceFreams() { return avoidanceFreams; }
    public int GetAvoidanceAfterFreams() { return avoidanceAfterFreams; }
    public int GetAvoidanceInvincibleFreams() { return avoidanceInvincibleFreams; }
}
