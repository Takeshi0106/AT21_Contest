using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeSeen : MonoBehaviour
{
    //�V�[�������[�h����
    public void LoadScene(string str)//()�����ɂ��Ď󂯎��
    {
        SceneManager.LoadScene(str);//�V�[����ǂݍ���
    }

    //�Q�[���I������
    public void GameEnd()
    {
#if UNITY_EDITOR //UnityEditor�ł̎��s�Ȃ�Đ����[�h����������
        UnityEditor.EditorApplication.isPlaying = false;
#else//unityEditor.�ł̎��s�ł͖�����΁i���r���h��j�Ȃ�A�v���P�[�V�������I������
            Application.Quit();//�뎚��ƃr���h����Ƃ��ɓ����Ȃ�
#endif
    }
}
