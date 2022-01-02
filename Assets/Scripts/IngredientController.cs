using System.Collections;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    [SerializeField] private FloatReference _flipSpeed;

    private Ingredient _ingredient;

    private Collider _collider;

    private void Awake() 
    {
        _ingredient = GetComponent<Ingredient>();
        _collider = GetComponent<Collider>();
    }

    public bool Flip(SwipeDirection swipeDirection)
    {
        if (swipeDirection.IsUp)
        {
            FlipDown();
        }
        else if (swipeDirection.IsRight)
        {
            FlipLeft();
        }
        else if (swipeDirection.IsDown)
        {
            FlipUp();
        }
        else if (swipeDirection.IsLeft)
        {
            FlipRight();
        }

        return false;
    }

    private void FlipUp()
    {
        if (!_ingredient.CanFlipUp || GameManager.Singleton.IngredientIsAnimating) { return; }

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currectStack = GetComponent<Stack>();
        
        Vector3 sideUp = new Vector3(
            transform.position.x, 
            0.05f * targetStack.Childrens.Length + 0.05f * currectStack.Childrens.Length, 
            transform.position.z - transform.lossyScale.z / 2);
        
        StartCoroutine(RotateAroundSide(sideUp, _ingredient.HitObject, Vector3.left, _flipSpeed.Value));
    }

    private void FlipRight()
    {
        if (!_ingredient.CanFlipRight || GameManager.Singleton.IngredientIsAnimating) { return; }

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currentStack = GetComponent<Stack>();

        Vector3 sideRight = new Vector3(
            transform.position.x - transform.lossyScale.x / 2, 
            0.05f * targetStack.Childrens.Length + 0.05f * currentStack.Childrens.Length, 
            transform.position.z);
        
        StartCoroutine(RotateAroundSide(sideRight, _ingredient.HitObject, Vector3.forward, _flipSpeed.Value));
    }

    private void FlipDown()
    {
        if (!_ingredient.CanFlipDown || GameManager.Singleton.IngredientIsAnimating) { return; }

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currentStack = GetComponent<Stack>();
        
        Vector3 sideDown = new Vector3(
            transform.position.x, 
            0.05f * targetStack.Childrens.Length + 0.05f * currentStack.Childrens.Length, 
            transform.position.z + transform.lossyScale.z / 2);
        
        StartCoroutine(RotateAroundSide(sideDown, _ingredient.HitObject, Vector3.right, _flipSpeed.Value));
    }

    private void FlipLeft()
    {
        if (!_ingredient.CanFlipLeft || GameManager.Singleton.IngredientIsAnimating) { return; }

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currentStack = GetComponent<Stack>();
        
        Vector3 sideLeft = new Vector3(
            transform.position.x + transform.lossyScale.x / 2, 
            0.05f * targetStack.Childrens.Length + 0.05f * currentStack.Childrens.Length, 
            transform.position.z);
        
        StartCoroutine(RotateAroundSide(sideLeft, _ingredient.HitObject, Vector3.back, _flipSpeed.Value));
    }

    private IEnumerator RotateAroundSide(Vector3 point, GameObject hitObject, Vector3 axis, float flipSpeed)
    {
        float degree = 0;

        GameManager.Singleton.IngredientIsAnimating = true;
        
        while (degree <= 180)
        {
            GameManager.Singleton.StartReplaySampling(transform);
            transform.RotateAround(point, axis, flipSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
            degree += flipSpeed * Time.deltaTime;
        }
        
        transform.localEulerAngles = axis * -180;
        GameManager.Singleton.StopReplaySamples();
        
        if (hitObject != null)
        {
            transform.parent = hitObject.transform;
        }

        _collider.enabled = false;
        yield return new WaitForSeconds(0.5f);

        hitObject.GetComponent<Stack>().UpdateStack();
        GameManager.Singleton.IngredientIsAnimating = false;
        yield return new WaitForSeconds(1.0f);

        if (GameManager.Singleton != null)
        {
            GameManager.Singleton.FindStack();
        }

        yield return null;
    }
}