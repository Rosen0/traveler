using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    private ScoreCtrl Ctrl;

     void Start()
     {
         Ctrl=GameObject.Find("RewardCtrl").GetComponent<ScoreCtrl>();
     }

     void OnTriggerEnter2D(Collider2D col)
     {
         if (col.tag == "Player")
         {
            // IncreaseScore(ScoreIncrement);
             Ctrl.addScore = true;
             Destroy(this.gameObject);
         }
     }
    
}
