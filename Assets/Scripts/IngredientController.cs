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
        if (!_ingredient.CanFlipUp || GameManager.Singleton.IngredientIsAnimating) { return; }

        Debug.Log("FlipUp: can flip");

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currectStack = GetComponent<Stack>();

        Vector3 sideUp = new Vector3(
            transform.position.x, 
            0.1f * targetStack.Childrens.Length + 0.1f * currectStack.Childrens.Length, 
            transform.position.z + transform.lossyScale.z / 2);
        
        // StartCoroutine(RotateAroundSide(sideUp, Axis.Up, _ingredient.HitObject, _flipSpeed.Value));
        MoveTo(_ingredient.HitObject);
    }

    private void FlipRight()
    {
        if (!_ingredient.CanFlipRight || GameManager.Singleton.IngredientIsAnimating) { return; }

        Debug.Log("FlipRight: can flip");

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currentStack = GetComponent<Stack>();

        // Vector3 sideRight = new Vector3(
        //     transform.position.x + transform.localScale.x / 2, 
        //     0.05f * targetStack.Childrens.Length + 0.05f * currentStack.Childrens.Length, 
        //     transform.position.z);

        Vector3 sideRight = new Vector3(
            transform.position.x + transform.localScale.x / 2, 
            0.1f * targetStack.Childrens.Length + 0.1f * currentStack.Childrens.Length, 
            transform.position.z
            );
        
        // StartCoroutine(RotateAroundSide(sideRight, Axis.Right, _ingredient.HitObject, _flipSpeed.Value));
        MoveTo(_ingredient.HitObject);
    }

    private void FlipDown()
    {
        if (!_ingredient.CanFlipDown || GameManager.Singleton.IngredientIsAnimating) { return; }

        Debug.Log("FlipDown: can flip");

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currentStack = GetComponent<Stack>();

        Vector3 sideDown = new Vector3(
            transform.position.x, 
            0.1f * targetStack.Childrens.Length + 0.1f * currentStack.Childrens.Length, 
            transform.position.z - transform.lossyScale.z / 2);
        
        // StartCoroutine(RotateAroundSide(sideDown, Axis.Down, _ingredient.HitObject, _flipSpeed.Value));
        MoveTo(_ingredient.HitObject);
    }

    private void FlipLeft()
    {
        if (!_ingredient.CanFlipLeft || GameManager.Singleton.IngredientIsAnimating) { return; }

        Debug.Log("FlipLeft: can flip");

        Stack targetStack = _ingredient.HitObject.GetComponent<Stack>();
        Stack currentStack = GetComponent<Stack>();

        Vector3 sideLeft = new Vector3(
            transform.position.x - transform.localScale.x / 2, 
            0.1f * targetStack.Childrens.Length + 0.1f * currentStack.Childrens.Length, 
            transform.position.z);
        
        // StartCoroutine(RotateAroundSide(sideLeft, Axis.Left, _ingredient.HitObject, _flipSpeed.Value));
        MoveTo(_ingredient.HitObject);
    }

    private IEnumerator RotateAroundSide(Vector3 point, Vector3 axis, GameObject hitObject, float flipSpeed)
    {
        float degree = 0;

        GameManager.Singleton.IngredientIsAnimating = true;
        
        while (degree < 180)
        {
            GameManager.Singleton.StartReplaySampling(transform);

            transform.RotateAround(point, axis, flipSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
            degree = degree > 180 ? 180 : degree + flipSpeed * Time.deltaTime;
        }
        
        transform.localEulerAngles = axis * -180;
        GameManager.Singleton.StopReplaySamples();
        
        // make as child of target
        if (hitObject != null)
        {
            transform.parent = hitObject.transform;
        }

        _collider.enabled = false;
        yield return new WaitForSeconds(0.5f);

        hitObject.GetComponent<Stack>().UpdateStack();

        GameManager.Singleton.IngredientIsAnimating = false;
        
        yield return new WaitForSeconds(1.0f);

        GameManager.Singleton.FindStack();

        yield return null;
    }

    // from Sandwich-Clone

    private Vector3 _currentPosition;
    private Quaternion _currentRotation;


    private float _jumpHeight = .6f;
    private float _duration = .7f;

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
        if(hitObject.transform.position.z < transform.position.z)
        {
            StartCoroutine(Move(_duration, hitObject, Rotation.bottom, isBackMove));
        }
        else if(hitObject.transform.position.z > transform.position.z)
        {
            StartCoroutine(Move(_duration, hitObject, Rotation.top, isBackMove));
        }
        else
        {
            if(hitObject.transform.position.x < transform.position.x)
            {
                StartCoroutine(Move(_duration, hitObject, Rotation.left, isBackMove));
            }
            else if(hitObject.transform.position.x > transform.position.x)
            {
                StartCoroutine(Move(_duration, hitObject, Rotation.right, isBackMove));
            }
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

        var temp = isBackMove ? Vector3.zero : new Vector3(0, 0.1f * currentStack.Childrens.Length +  0.1f * targetStack.Childrens.Length, 0);
        
        var targetPosition = hitObject.transform.position + temp;

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

        GameManager.Singleton.IngredientIsAnimating = false;
        
        yield return new WaitForSeconds(1.0f);

        GameManager.Singleton.FindStack();
        
        _currentPosition = new Vector3(
            transform.position.x, 
            transform.position.y + hitObject.transform.position.y,
            transform.position.z);
    }
}