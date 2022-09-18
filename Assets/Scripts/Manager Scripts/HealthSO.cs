using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class HealthSO : ScriptableObject
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    public int CurrentHealthValue
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int MaxHealthValue
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
}
