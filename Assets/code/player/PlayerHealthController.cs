
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;   // 声明静态变量
 
    public int currentHealth, maxHealth;             // 声明当前生命和最大生命
 
 
    private void Awake()
    {
        instance = this;
    }
 
    void Start()
    {
        currentHealth = maxHealth;
 
    }
 
    void Update()
    {
 
    }
 
    public void DealDamage()
    {
 
            currentHealth --;
            
            // 当生命值为0时，玩家消失
            if(currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
 
    }

}