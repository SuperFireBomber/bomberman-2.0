
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
        AudioManager.instance.PlaySFX("hurt");
        // 当生命值为0时，玩家消失
        if (currentHealth <= 0)
        {
            // 禁用所有爆炸的碰撞体，防止后续爆炸伤害其他对象
            DisableAllExplosionColliders();
            AudioManager.instance.PlaySFX("steel-pipe");
            gameObject.SetActive(false);
            GameOverManager.instance.ShowGameOver();
        }

    }
    private void DisableAllExplosionColliders()
    {
        // 找到所有标签为 "Explosion" 的游戏对象
        GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        foreach (GameObject exp in explosions)
        {
            Collider2D col = exp.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

}