using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    [SerializeField, Tooltip("揺れ時間（秒単位）")]
    private float duration = 0.2f;
    [SerializeField, Tooltip("揺れの強さ")]
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

    // インスペクターの値を使うStartShake
    public void StartShake()
    {
        if (!isShaking)
        {
            originalPos = transform.localPosition;
        }

        shakeElapsed = 0f;
        isShaking = true;
    }

    // 引数付きのStartShakeも残す（必要なら）
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
