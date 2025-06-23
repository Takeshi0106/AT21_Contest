using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ==============================================================
// ButtonをWASDで選択できる　
// ==============================================================

public class ButtonSelect : MonoBehaviour
{
    [Header("ボタンのリスト")]
    [SerializeField] private Button[] m_buttonList;
    [Header("ボタンのクルーダウン (秒単位)")]
    [SerializeField] private float m_InputInterval = 2.0f;

    private Vector2 m_lastMousePosition;
    private bool m_isUsingMouse = true;

    private int m_SelectButton = 0;
    private float m_LastInputTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_SelectButton = 0;
        EventSystem.current.SetSelectedGameObject(m_buttonList[m_SelectButton].gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // マウスの位置取得
        Vector2 currentMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (currentMousePos != m_lastMousePosition)
        {
            m_isUsingMouse = true;
            m_lastMousePosition = currentMousePos;
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (!m_isUsingMouse)
        {
            //入力を取得
            float inputX = Input.GetAxisRaw("Horizontal"); //横方向
            float inputY = Input.GetAxisRaw("Vertical"); //縦方向

            if (Time.time - m_LastInputTime > m_InputInterval)
            {
                if (inputX > 0.5f)
                {
                    m_SelectButton = (m_SelectButton + 1) % m_buttonList.Length;
                    m_LastInputTime = Time.time;
                    EventSystem.current.SetSelectedGameObject(m_buttonList[m_SelectButton].gameObject);

                }
                else if (inputX < -0.5f)
                {
                    m_SelectButton = (m_SelectButton - 1 + m_buttonList.Length) % m_buttonList.Length;
                    m_LastInputTime = Time.time;
                    EventSystem.current.SetSelectedGameObject(m_buttonList[m_SelectButton].gameObject);
                }
            }
        }
        else
        {
            // キー操作を検出したらマウス操作をやめる
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f)
            {
                m_isUsingMouse = false;
                // 最初に選択ボタンをセット
                EventSystem.current.SetSelectedGameObject(m_buttonList[m_SelectButton].gameObject);
                m_LastInputTime = Time.time; // インターバルリセット
            }
        }
    }
}

