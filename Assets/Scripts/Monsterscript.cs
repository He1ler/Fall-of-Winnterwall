﻿// Script for enemies units
using UnityEngine;
using System.Collections;
public class Monsterscript : MonoBehaviour
{
    private Animator m_animator;
    private float m_delayToIdle = 0.0f;
    UI ui;
    public string EnemyName;
    public int health;
    public int AttackDamage;
    public int MaxHealth;
    public bool IsDead = false;
    public AudioClip Attack;
    public AudioClip Death;
    private EnemyData ed;
    public AudioSource AS;
    void Start()// loading data from files in the objects
    {
        Time.timeScale = 1;
        ed = DataTransition.EnemyFromFile(EnemyName);
        MaxHealth = ed.health;
        health = MaxHealth;
        AttackDamage = ed.AttackDamage;
        ui = GameObject.Find("UI").GetComponent<UI>();
        m_animator = GameObject.Find(EnemyName + "(Clone)").GetComponent<Animator>();
        AS = GameObject.Find(EnemyName + "(Clone)").GetComponent<AudioSource>();
    }
    public void Set_Death()// Script for death of enemy unit
    {
        if (!Pausemenu.GameisPaused)
        {
            m_animator.SetTrigger("Death");
            AS.clip = Death;
            AS.Play();
            IsDead = true;
            if (EnemyName == "burning-ghoul")//special unit for which death using function
            {
                burningghoul();
                Destroy(this.gameObject, (GameObject.Find(EnemyName + "(Clone)").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.5f));
            }
            else
            {
                Destroy(this.gameObject, (GameObject.Find(EnemyName + "(Clone)").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 1f));
            }         
        }
    }
    public void Set_Hurt(int hp)// taking damage by enemies
    {
        StartCoroutine(Set_HurtI(hp, GameObject.Find(ui.hero.HeroName + "(Clone)").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * 0.7f));
    }

    public IEnumerator Set_HurtI(int hp, float f)
    {
        yield return new WaitForSeconds(f);
        if (!Pausemenu.GameisPaused && !IsDead)
        {
            m_animator.SetTrigger("Hurt");
            if (EnemyName == "burning-ghoul")//special unit for which death using function
            {
                health = 0;
                Set_Death();
            }
            health -= hp;
            //DataTransition.EnemyToFile(name, hp);
            if(health<=0)
            {
                health = 0;
                Set_Death();
            }
        }
    }
    public void Set_Idle()
    {
        if (!Pausemenu.GameisPaused && !IsDead)
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
            { }
        }
    }
    public void Set_Attack()// Enemy Attack
    {
        if (!Pausemenu.GameisPaused && !ui.hero.IsDead && !IsDead)
        {
            m_animator.SetTrigger("Attack");
            AS.clip = Attack;
            AS.Play();
            if (EnemyName == "burning-ghoul") //special unit for which attack using function
            {
                Set_Death();
            }
            else
            {
                ui.hero.Set_Hurt(AttackDamage);
            }
        }
    }
    void burningghoul ()//special unit that in death can damage heroes
    {
            if (!ui.hero.IsDead)
            {
                ui.hero.Set_Hurt(AttackDamage);
            }
            if (!ui.CheckIsEnemiesDead[0] && ui.enemy1.EnemyName != "burning-ghoul")
            {
                ui.enemy1.Set_Hurt(AttackDamage);
            }
            if (!ui.CheckIsEnemiesDead[1] && ui.enemy2.EnemyName != "burning-ghoul")
            {
                ui.enemy2.Set_Hurt(AttackDamage);
            }
            if(!ui.CheckIsEnemiesDead[2] && ui.enemy1.EnemyName != "burning-ghoul")
            {
                    ui.enemy3.Set_Hurt(AttackDamage);
            }
            if (!ui.CheckIsEnemiesDead[3] && ui.enemy1.EnemyName != "burning-ghoul")
            {
                    ui.enemy4.Set_Hurt(AttackDamage);
            }
    }
}
