
using System;

public enum ReferenceType { Reference, Constant }

// Custom serializable class
[Serializable]
public class ValueReference
{
    public float FloatVariable { get { return ReferenceType == ReferenceType.Constant ? Amount : Value.FloatVariable; } }
    public float Amount = 1;
    public Value Value;
    public ReferenceType ReferenceType;
}
