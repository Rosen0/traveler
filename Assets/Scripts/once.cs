using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class once : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("1");
            rb.AddForce(new Vector2(0, -Speed));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
