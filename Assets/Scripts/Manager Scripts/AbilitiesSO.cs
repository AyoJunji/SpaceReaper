using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class AbilitiesSO : ScriptableObject
{
    [SerializeField] private bool hasDashAbility;
    [SerializeField] private bool hasThrowAbility;
    [SerializeField] private bool hasBubbleShield;

    public bool CheckDash
    {
        get { return hasDashAbility; }
        set { hasDashAbility = value; }
    }

    public bool CheckThrow
    {
        get { return hasThrowAbility; }
        set { hasThrowAbility = value; }
    }

    public bool CheckBubbleShield
    {
        get { return hasBubbleShield; }
        set { hasBubbleShield = value; }
    }


}
