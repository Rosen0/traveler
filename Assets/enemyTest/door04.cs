using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class door04 : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("123");
        if (col.tag == "Player")
        {
            SceneManager.LoadScene("end");
        }
        else
        {
            Debug.Log("NULL");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
