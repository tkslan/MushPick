using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Value : ScriptableObject
{
    [Range(0.01f,0.99f)]
    public float FloatVariable;
}
