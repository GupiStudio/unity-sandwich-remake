using UnityEngine;

public class ReplaySample
{
    private Transform _targetIngredient;
    private Vector3 _targetPosition;
    private Quaternion _targetRotation;


    public Transform TargetIngredient => _targetIngredient;

    public Vector3 TargetPosition
    {
        get => _targetPosition;
        set
        {
            _targetPosition = value;
        }
    }

    public Quaternion TargetRotation
    {
        get => _targetRotation;
        set
        {
            _targetRotation = value;
        }
    }

    public ReplaySample(Transform targetIngredient, bool useLocals = false)
    {
        _targetIngredient = targetIngredient;
        _targetPosition = useLocals ? targetIngredient.localPosition : targetIngredient.position;
        _targetRotation = useLocals ? targetIngredient.localRotation : targetIngredient.rotation;
    }

    public ReplaySample(Transform targetIngredient, Vector3 targetPosition, Quaternion targetRotation)
    {
        _targetIngredient = targetIngredient;
        _targetPosition = targetPosition;
        _targetRotation = targetRotation;
    }

    public void SetIngredientToSampledPosition()
    {
        _targetIngredient.position = _targetPosition;
    }

    public void SetIngredientToSampledRotation()
    {
        _targetIngredient.rotation = _targetRotation;
    }

    public void SetIngredientToAllSampledProperties()
    {
        SetIngredientToSampledPosition();
        SetIngredientToSampledRotation();
    }
}