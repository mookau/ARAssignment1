using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animal", menuName = "Animal Data")]
public class AnimalData : ScriptableObject
{
    public new string name;
    public string imageName;
    public int attack;
    public int health;
    public int armor;
    public int speed;
}
