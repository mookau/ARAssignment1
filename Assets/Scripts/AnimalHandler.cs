using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHandler : MonoBehaviour
{
    public AnimalData animalData;    

    [SerializeField]
    private AnimalHandler targetEnemy;

    private Animator animator;

    private AttachUIElements uiElement;
    private Transform modelTransform;

    private bool destroyMe = false;

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

        animator = gameObject.GetComponent<Animator>();
        modelTransform = gameObject.transform.Find("Model");

        EventManager.current.OnAttack += Current_OnAttack;
    }

    private void Current_OnAttack(object sender, EventManager.OnAttackEventArgs e)
    {
        if (targetEnemy != null)
        {
            //check if we are currently in an encounter, and that our enemy is the sender
            if (e.attackerName == targetEnemy.name)
            {
                //double check that we are the correct target
                if (e.attackedName == name)
                {
                    TakeDamage(e.damageDealt);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if I'm not scheduled for destruction, do regular update
        if (!destroyMe)
        {
            if (targetEnemy != null)
            {
                if (modelTransform != null)
                {
                    modelTransform.LookAt(targetEnemy.transform.position, this.transform.root.transform.up);
                }
                //Debug.Log("timer:  " + timer);
                if (timer > timerInterval)
                {
                    timer -= timerInterval;
                    //invoke attack event
                    EventManager.current.InvokeAttack(this, new EventManager.OnAttackEventArgs { attackedName = targetEnemy.name, attackerName = this.name, damageDealt = this.attack });

                    //we have attempted to attack, play attack animation (TODO: check that we are actively engaged in a combat before animation, and not just attempting one)
                    animator.Play("Attack");
                }
                timer += (speed + 10) * Time.deltaTime;
            }
            else
            {
                if (modelTransform != null)
                {
                    modelTransform.localPosition = Vector3.zero;
                    modelTransform.localRotation = Quaternion.identity;
                }
                timer = 0f;
            }

            try
            {
                uiElement = this.transform.root.gameObject.GetComponentInChildren<AttachUIElements>();
                uiElement.UpdateText(health.ToString() + "/" + maxHealth.ToString());
            }
            catch
            {
                //shhhh, go away Rick, nothing to see here
            }            
        }        
        //check if the animator is done with the dying animation
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("DestroyState"))
        {
            Destroy(this.transform.root.gameObject);
        }
    }

    //triggers to check if enemy is nearby or not
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("I " + this.animalData.name + " have ecountered: " + other.name);
        if (targetEnemy == null)
        {
            var otherAnimal = other.gameObject.GetComponent<AnimalHandler>();
            if (otherAnimal != null)
            {
                //Debug.Log("I " + this.animalData.name + " see other creature: " + other.name + " is animal");
                targetEnemy = otherAnimal;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (timer == 0f && targetEnemy == null)
        {
            //Debug.Log("I " + this.animalData.name + " have changed target to: " + other.name);
            if (targetEnemy == null)
            {
                var otherAnimal = other.gameObject.GetComponent<AnimalHandler>();
                if (otherAnimal != null)
                {
                    //Debug.Log("I " + this.animalData.name + " see other creature: " + other.name + " is animal");
                    targetEnemy = otherAnimal;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("I " + this.animalData.name + " have lost sight of: " + other.name);
        if (targetEnemy != null)
        {
            var otherAnimal = other.gameObject.GetComponent<AnimalHandler>();
            if (otherAnimal == targetEnemy)
            {
                //Debug.Log("I " + this.animalData.name + " will stop fighting other creature: " + other.name);
                targetEnemy = null;
            }
        }
    }


    private void TakeDamage(int damage)
    {
        //reduce damage done by armor amount, to a minimum of 1
        health -= (1 <= (damage - armor)) ? (damage - armor) : 1;

        if (uiElement == null)
        {
            uiElement = this.transform.root.gameObject.GetComponentInChildren<AttachUIElements>();
        }

        uiElement.UpdateText(health.ToString() + "/" + maxHealth.ToString());

        //Debug.Log("I " + this.animalData.name + " have taken damage. Current health: " + health);

        //check if dead
        if (health <= 0)
        {
            Deadded();
        }
        else
        {
            //trigger damage animation if not dead
            animator.Play("Damage");
        }
    }

    private void Deadded()
    {
        EventManager.current.OnAttack -= Current_OnAttack;
        //Debug.Log("I " + this.animalData.name + " have died: " + health);
        targetEnemy = null;
        animator.Play("Die");
        destroyMe = true;
    }

}
