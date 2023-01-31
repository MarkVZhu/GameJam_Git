using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public CharacterData_SO characterData;

    #region Read from Data_SO
    public int MaxHealth { get => characterData != null ? characterData.maxHealth : 0; set => characterData.maxHealth = value; }

    public int CurrentHealth { get => characterData != null ? characterData.currentHealth : 0; set => characterData.currentHealth = value; }

    public int Attack { get => characterData != null ? characterData.attack : 0; set => characterData.attack = value; }

    public int Defence { get => characterData != null ? characterData.defence : 0; set => characterData.defence = value; }
    #endregion
}
