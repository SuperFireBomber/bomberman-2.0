
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DamgePlayer : MonoBehaviour
{
    void Start()
    {
        
    }
 
    void Update()
    {
        
    }
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            PlayerHealthController.instance.DealDamage();
        }
        
    }
}
