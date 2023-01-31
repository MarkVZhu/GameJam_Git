using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character State/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("State Info")]
    public int maxHealth;

    public int currentHealth;

    public int attack;

    public int defence;
}
