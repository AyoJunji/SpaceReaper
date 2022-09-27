using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class LevelHandler : ScriptableObject
{
    [SerializeField] private bool beatenLevelOne;
    [SerializeField] private bool beatenLevelTwo;
    [SerializeField] private bool beatenLevelThree;

    public bool CheckLevelOne
    {
        get { return beatenLevelOne; }
        set { beatenLevelOne = value; }
    }

    public bool CheckLevelTwo
    {
        get { return beatenLevelTwo; }
        set { beatenLevelTwo = value; }
    }

    public bool CheckLevelThree
    {
        get { return beatenLevelThree; }
        set { beatenLevelThree = value; }
    }
}