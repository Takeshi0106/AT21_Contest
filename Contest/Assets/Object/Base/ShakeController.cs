using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    [SerializeField, Tooltip("�h�ꎞ�ԁi�b�P�ʁj")]
    private float duration = 0.2f;
    [SerializeField, Tooltip("�h��̋���")]
    private float magnitude = 0.1f;

    private bool isShaking = false;
    private float shakeElapsed = 0f;
    private Vector3 originalPos;

    void Update()
    {
        if (isShaking)
        {
            if (shakeElapsed < duration)
            {
                float offsetX = Random.Range(-1f, 1f) * magnitude;
                float offsetY = Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

                shakeElapsed += Time.deltaTime;
            }
            else
            {
                isShaking = false;
                transform.localPosition = originalPos;
            }
        }
    }

    // �C���X�y�N�^�[�̒l���g��StartShake
    public void StartShake()
    {
        if (!isShaking)
        {
            originalPos = transform.localPosition;
        }

        shakeElapsed = 0f;
        isShaking = true;
    }

    // �����t����StartShake���c���i�K�v�Ȃ�j
    public void StartShake(float customDuration, float customMagnitude)
    {
        duration = customDuration;
        magnitude = customMagnitude;

        if (!isShaking)
        {
            originalPos = transform.localPosition;
        }
        shakeElapsed = 0f;
        isShaking = true;
    }
}
