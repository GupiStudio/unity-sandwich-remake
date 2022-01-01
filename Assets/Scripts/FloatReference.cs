using System;

[Serializable]
public class FloatReference
{
    public bool UseContant = true;
    public float ConstantValue;
    public FloatVariableSO FloatVariable;

    public float Value
    {
        get => UseContant ? ConstantValue : FloatVariable.Value;
        set
        {
            if (UseContant)
                ConstantValue = value;
            else
                FloatVariable.Value = value;
        }
    }
}
