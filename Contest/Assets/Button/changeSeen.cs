using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeSeen : MonoBehaviour
{
    //シーンをロードする
    public void LoadScene(string str)//()引数にして受け取る
    {
        SceneManager.LoadScene(str);//シーンを読み込む
    }

    //ゲーム終了する
    public void GameEnd()
    {
#if UNITY_EDITOR //UnityEditorでの実行なら再生モードを解除する
        UnityEditor.EditorApplication.isPlaying = false;
#else//unityEditor.での実行では無ければ（→ビルド後）ならアプリケーションを終了する
            Application.Quit();//誤字るとビルドするときに動かない
#endif
    }
}
