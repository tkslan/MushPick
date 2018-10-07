﻿
using System;

public enum IngredientUnit { Reference, Constant }

// Custom serializable class
[Serializable]
public class ValueReference
{
    public float amount = 1;
    public Value Value;
    public bool UseConstant = false;
    public float FloatVariable { get { return UseConstant ? amount : Value.FloatVariable; } }
}
