using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHandler : MonoBehaviour
{
    public AnimalData animalData;

    public event EventHandler<OnAttackEventArgs> OnAttack;
    [SerializeField]
    private AnimalHandler targetEnemy;

    private Text nameText;
    private Text healthText;

    private int attack;
    private int health;
    private int maxHealth;
    private int armor;
    private int speed;

    private float timerInterval = 50f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        attack = animalData.attack;
        health = animalData.health;
        maxHealth = animalData.health;
        armor = animalData.armor;
        speed = animalData.speed;
        targetEnemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEnemy != null)
        {
            //Debug.Log("timer:  " + timer);
            if (timer > timerInterval)
            {
                Debug.Log("timer triggered");
                timer -= timerInterval;
                OnAttack?.Invoke(this, new OnAttackEventArgs { damageDealt = this.attack });
            }
            timer += (speed + 10) * Time.deltaTime;
        }
        else
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I " + this.animalData.name + " have ecountered: " + other.name);
        if (targetEnemy == null)
        {
            var otherAnimal = other.gameObject.GetComponent<AnimalHandler>();
            if (otherAnimal != null)
            {
                Debug.Log("I " + this.animalData.name + " see other creature: " + other.name + " is animal");
                targetEnemy = otherAnimal;
                OnAttack += otherAnimal.AnimalHandler_OnAttacked;
            }
        }
    }

    private void AnimalHandler_OnAttacked(object sender, OnAttackEventArgs e)
    {
        TakeDamage(e.damageDealt);
        if (health <= 0)
        {
            var aHandler = (AnimalHandler)sender;
            OnAttack -= aHandler.AnimalHandler_OnAttacked;
            Deadded();
        }
    }

    private void TakeDamage(int damage)
    {
        health -= (damage - armor);
        Debug.Log("I " + this.animalData.name + " have taken damage. Current health: " + health);
        //trigger damage animation
    }

    private void Deadded()
    {
        Debug.Log("I " + this.animalData.name + " have died: " + health);
        targetEnemy = null;
        //do death animation
        Destroy(this.transform.root.gameObject);
    }

    public class OnAttackEventArgs : EventArgs
    {
        public int damageDealt;
    }
}
