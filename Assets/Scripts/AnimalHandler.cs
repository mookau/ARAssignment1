using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHandler : MonoBehaviour
{
    public AnimalData animalData;    

    //public event EventHandler<OnAttackEventArgs> OnAttack;
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

        EventManager.current.OnAttack += Current_OnAttack;
    }

    private void Current_OnAttack(object sender, EventManager.OnAttackEventArgs e)
    {
        //check if we are currently in an encounter, and that our enemy is the sender
        if (targetEnemy != null && e.attackerName == targetEnemy.name)
        {
            //double check that we are the correct target
            if (e.attackedName == name)
            {
                TakeDamage(e.damageDealt);
            }
        }
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
                //invoke attack event
                EventManager.current.InvokeAttack(this, new EventManager.OnAttackEventArgs { attackedName = targetEnemy.name, attackerName = this.name, damageDealt = this.attack });
            }
            timer += (speed + 10) * Time.deltaTime;
        }
        else
        {
            timer = 0f;
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
                //add other animals attacked function to event (this is bad behaviour)
                //OnAttack += otherAnimal.AnimalHandler_OnAttacked;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (timer == 0f)
        {
            Debug.Log("I " + this.animalData.name + " have changed target to: " + other.name);
            if (targetEnemy == null)
            {
                var otherAnimal = other.gameObject.GetComponent<AnimalHandler>();
                if (otherAnimal != null)
                {
                    Debug.Log("I " + this.animalData.name + " see other creature: " + other.name + " is animal");
                    targetEnemy = otherAnimal;
                    //add other animals attacked function to event (this is bad behaviour)
                    //OnAttack += otherAnimal.AnimalHandler_OnAttacked;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("I " + this.animalData.name + " have lost sight of: " + other.name);
        if (targetEnemy != null)
        {
            var otherAnimal = other.gameObject.GetComponent<AnimalHandler>();
            if (otherAnimal == targetEnemy)
            {
                Debug.Log("I " + this.animalData.name + " will stop fighting other creature: " + other.name);
                targetEnemy = null;
                //add other animals attacked function to event (this is bad behaviour)
                //OnAttack += otherAnimal.AnimalHandler_OnAttacked;
            }
        }
    }

    /*
    private void AnimalHandler_OnAttacked(object sender, OnAttackEventArgs e)
    {
        TakeDamage(e.damageDealt);
        if (health <= 0)
        {
            var aHandler = (AnimalHandler)sender;
            //attempt to unsubscribe from event (this doesn't go so well either)
            //OnAttack -= aHandler.AnimalHandler_OnAttacked;
            Deadded();
        }
    }
    */

    private void TakeDamage(int damage)
    {
        //reduce damage done by armor amount, to a minimum of 1
        health -= (1 <= (damage - armor)) ? (damage - armor) : 1;

        Debug.Log("I " + this.animalData.name + " have taken damage. Current health: " + health);

        //check if dead
        if (health <= 0)
        {
            Deadded();
        }
        else
        {
            //trigger damage animation if not dead
        }
    }

    private void Deadded()
    {
        EventManager.current.OnAttack -= Current_OnAttack;
        Debug.Log("I " + this.animalData.name + " have died: " + health);
        targetEnemy = null;
        //do death animation
        Destroy(this.transform.root.gameObject);
    }

    /*
    public class OnAttackEventArgs : EventArgs
    {
        public int damageDealt;
    }
    */
}
