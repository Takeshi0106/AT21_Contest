using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ==============================================================
// Button��WASD�őI���ł���@
// ==============================================================

public class ButtonSelect : MonoBehaviour
{
    [Header("�{�^���̃��X�g")]
    [SerializeField] private Button[] m_buttonList;
    [Header("�{�^���̃N���[�_�E�� (�b�P��)")]
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
        // �}�E�X�̈ʒu�擾
        Vector2 currentMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (currentMousePos != m_lastMousePosition)
        {
            m_isUsingMouse = true;
            m_lastMousePosition = currentMousePos;
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (!m_isUsingMouse)
        {
            //���͂��擾
            float inputX = Input.GetAxisRaw("Horizontal"); //������
            float inputY = Input.GetAxisRaw("Vertical"); //�c����

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
            // �L�[��������o������}�E�X�������߂�
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f)
            {
                m_isUsingMouse = false;
                // �ŏ��ɑI���{�^�����Z�b�g
                EventSystem.current.SetSelectedGameObject(m_buttonList[m_SelectButton].gameObject);
                m_LastInputTime = Time.time; // �C���^�[�o�����Z�b�g
            }
        }
    }
}

