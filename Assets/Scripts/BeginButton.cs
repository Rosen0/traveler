using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BeginButton : MonoBehaviour
{
    private Button btn_Begin;

    private void changeScene()
    {
        Debug.Log("转场");
        SceneManager.LoadScene("Level01");
    }
    // Start is called before the first frame update
    void Start()
    {
        btn_Begin = this.transform.Find("Btn_Begin").GetComponent<Button>();
        btn_Begin.onClick.AddListener(changeScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
