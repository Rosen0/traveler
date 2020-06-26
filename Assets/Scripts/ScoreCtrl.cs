using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCtrl : MonoBehaviour
{
    public Text scoreText;
    public bool addScore;
    public int Score;//当前分数
    public int ScoreSum;


    //退出游戏程序
    private GameObject exitPanel ;
    private Button btn_false;
    private Button btn_true;

    private bool flag = false;

    private void F()
    {
        Debug.Log("Continue");
        exitPanel.SetActive(false);
        flag = false;
    }
    private void T()
    {
        //编辑模式下退出
        //UnityEditor.EditorApplication.isPlaying = false;
        //游戏退出
        Debug.Log("Exit");
        Application.Quit();
    }

    void Start()
    {
        exitPanel = GameObject.Find("exitPanel").gameObject;
        btn_true = GameObject.Find("Btn_true").GetComponent<Button>();
        btn_false = GameObject.Find("Btn_false").GetComponent<Button>();
        btn_false.onClick.AddListener(F);
        btn_true.onClick.AddListener(T);
        exitPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Score + "/" + ScoreSum;
        if (addScore) 
        {
            Score += 1;
            addScore = false;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!flag)
            {
                exitPanel.SetActive(true);
                flag = true;
            }
            else
            {
                exitPanel.SetActive(false);
                flag = false;
            }
        }
    }

}
