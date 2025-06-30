using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationInstructions : MonoBehaviour
{
    [Header("コントローラーオブジェクト")]
    [SerializeField] private GameObject m_Con = null;
    [Header("キーボードオブジェクト")]
    [SerializeField] private GameObject m_Key = null;

    static bool m_Flag = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Con.SetActive(false);
        m_Key.SetActive(false);

        m_Flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        FlagChack();
        display();
    }

    void display()
    {
        if (m_Flag)
        {
            this.gameObject.SetActive(true);

            string[] joysticks = Input.GetJoystickNames();

            if (joysticks.Length > 0 && !string.IsNullOrEmpty(joysticks[0]))
            {
                m_Con.SetActive(true);
            }
            else
            {
                m_Key?.SetActive(true);
            }
        }
        else
        {
            m_Con.SetActive(false);
            m_Key.SetActive(false);

            this.gameObject.SetActive(false);
        }
    }

    static void SetOperationDisplay(bool _flag)
    { 
        m_Flag = _flag; 
    }

    void FlagChack()
    {
        if(Input.GetButtonDown("Kettei"))
        {
            Debug.Log("Kettei");
            m_Flag = false;
        }
    }
}
