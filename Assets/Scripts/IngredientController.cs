using System.Collections;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    private Ingredient _ingredient;

    private BoxCollider _collider;

    private void Awake() 
    {
        _ingredient = GetComponent<Ingredient>();
        _collider = GetComponent<BoxCollider>();
    }

    public bool Flip(SwipeDirection swipeDirection)
    {
        if (swipeDirection.IsUp)
        {
            FlipUp();
        }
        else if (swipeDirection.IsRight)
        {
            FlipRight();
        }
        else if (swipeDirection.IsDown)
        {
            FlipDown();
        }
        else if (swipeDirection.IsLeft)
        {
            FlipLeft();
        }

        return false;
    }

    private void FlipUp()
    {
        if (!_ingredient.CanFlipUp || GameManager.Instance.IngredientIsAnimating) { return; }

        Debug.Log("FlipUp: can flip");

        MoveTo(_ingredient.HitObject);
    }

    private void FlipRight()
    {
        if (!_ingredient.CanFlipRight || GameManager.Instance.IngredientIsAnimating) { return; }

        Debug.Log("FlipRight: can flip");

        MoveTo(_ingredient.HitObject);
    }

    private void FlipDown()
    {
        if (!_ingredient.CanFlipDown || GameManager.Instance.IngredientIsAnimating) { return; }

        Debug.Log("FlipDown: can flip");
        
        MoveTo(_ingredient.HitObject);
    }

    private void FlipLeft()
    {
        if (!_ingredient.CanFlipLeft || GameManager.Instance.IngredientIsAnimating) { return; }

        Debug.Log("FlipLeft: can flip");
        
        MoveTo(_ingredient.HitObject);
    }

    // from Sandwich-Clone

    private Vector3 _currentPosition;
    private Quaternion _currentRotation;


    private float _jumpHeight = .6f;

    private enum Rotation{
        top,
        bottom,
        left,
        right
    }

    private void OnEnable() 
    {
        _currentPosition = transform.position;
        _currentRotation = transform.rotation;
    }

    private void MoveTo(GameObject hitObject, bool isBackMove = false)
    {
        var flipDuration = GameManager.Instance.FlipDuration;

        if(hitObject.transform.position.z < transform.position.z)
        {
            StartCoroutine(Move(flipDuration, hitObject, Rotation.bottom, isBackMove));
        }
        else if(hitObject.transform.position.z > transform.position.z)
        {
            StartCoroutine(Move(flipDuration, hitObject, Rotation.top, isBackMove));
        }
        else if(hitObject.transform.position.x < transform.position.x)
        {
            StartCoroutine(Move(flipDuration, hitObject, Rotation.left, isBackMove));
        }
        else if(hitObject.transform.position.x > transform.position.x)
        {
            StartCoroutine(Move(flipDuration, hitObject, Rotation.right, isBackMove));
        }
    }

    private IEnumerator Move(float duration, GameObject hitObject, Rotation rotation, bool isBackMove)
    {
        float progress = 0f;
        Quaternion endRotation;

        switch(rotation)
        {
            case Rotation.top:
                endRotation = Quaternion.Euler(-180,0,0);
                break;
            case Rotation.bottom:
                endRotation = Quaternion.Euler(180,0,0);
                break;
            case Rotation.right:
                endRotation = Quaternion.Euler(0,0,180);
                break;
            case Rotation.left:
                endRotation = Quaternion.Euler(0,0,-180);
                break;
            default:
                endRotation = Quaternion.Euler(0,0,0);
                break;
        }

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currentStack = GetComponent<Stack>();

        var colliderSize = GetComponent<BoxCollider>().size;

        var projectionPosition = new Vector3(
            0, 
            colliderSize.y * currentStack.Childrens.Length +  colliderSize.y * targetStack.Childrens.Length,
            0);

        var adderVector = isBackMove ? Vector3.zero : projectionPosition;
        
        var targetPosition = hitObject.transform.position + adderVector;

        GameManager.Instance.IngredientIsAnimating = true;

        while(progress < duration)
        {
            progress += Time.deltaTime;
            var percent = Mathf.Clamp01(progress/duration);
            float height = (_jumpHeight) * Mathf.Sin(Mathf.PI * percent);

            transform.position = Vector3.Lerp(_currentPosition, targetPosition, percent) + new Vector3(0, height, 0);
            transform.rotation = Quaternion.Lerp(_currentRotation, endRotation, percent);
            yield return null;
        }

        // make as child of target
        if (hitObject != null)
        {
            transform.parent = hitObject.transform;
        }

        _collider.enabled = false;
        yield return new WaitForSeconds(0.5f);

        hitObject.GetComponent<Stack>().UpdateStack();

        GameManager.Instance.IngredientIsAnimating = false;
        
        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.FindStack();
        
        _currentPosition = new Vector3(
            transform.position.x, 
            transform.position.y + hitObject.transform.position.y,
            transform.position.z);
    }
}