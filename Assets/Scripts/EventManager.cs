using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public class OnAttackEventArgs : EventArgs
    {
        public string attackerName;
        public int damageDealt;
    }
}
