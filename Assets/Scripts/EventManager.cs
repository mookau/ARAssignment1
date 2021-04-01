using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public event EventHandler<OnAttackEventArgs> OnAttack;

    private void Awake()
    {
        current = this;
    }

    public void InvokeAttack(object sender, EventManager.OnAttackEventArgs e)
    {
        OnAttack?.Invoke(sender, e);
    }

    public class OnAttackEventArgs : EventArgs
    {
        public string attackerName;
        public string attackedName;
        public int damageDealt;
    }
}
