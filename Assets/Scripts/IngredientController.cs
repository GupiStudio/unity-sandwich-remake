using System.Collections;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    private Ingredient _ingredient;

    private BoxCollider _collider;

    private FlipCommand _command;

    private void Awake() 
    {
        _ingredient = GetComponent<Ingredient>();
        _collider = GetComponent<BoxCollider>();
    }

    public void Fold(SwipeDirection swipeDirection)
    {
        _command = new Flip(
            _ingredient,
            swipeDirection,
            GameManager.Instance.FlipDuration).Command;

        StartCoroutine(_command.Execute());

        GameManager.Instance.RecordFlip(_command);
    }
}