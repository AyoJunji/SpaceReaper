using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class SoulsSO : ScriptableObject
{
    [SerializeField]
    private int souls;

    public int Value
    {
        get { return souls; }
        set { souls = value; }
    }
}
