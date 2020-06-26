using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private GameObject player;
    public float Tanli;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        player.transform.Translate(new Vector2(0,Tanli));
        
        Debug.Log("123");


    }
    
 
    // Update is called once per frame
    void Update()
    {

    }
}
