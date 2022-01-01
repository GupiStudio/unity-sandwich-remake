using UnityEngine;

public class SwipeDirection
{
    private bool _isUp;
    private bool _isRight;
    private bool _isDown;
    private bool _isLeft;
    
    public SwipeDirection(Vector3 touchMovement, float minimumDistance = 0.5f)
    {
        if (touchMovement.y < 0 && touchMovement.x > -minimumDistance && touchMovement.x < minimumDistance)
        {
            _isUp = true;
        }
        else if (touchMovement.x < 0 && touchMovement.y > -minimumDistance && touchMovement.y < minimumDistance)
        {
            _isRight = true;
        }
        else if (touchMovement.y > 0 && touchMovement.x > -minimumDistance && touchMovement.x < minimumDistance)
        {
            _isDown = true;
        }
        else if (touchMovement.x > 0 && touchMovement.y > -minimumDistance && touchMovement.y < minimumDistance)
        {
            _isLeft = true;
        }
    }

    public bool IsUp 
    { 
        get
        {
            if (_isUp)
                Debug.Log("SwipeDirection: IsUp");

            return _isUp;
        }
    }

    public bool IsRight
    {
        get
        {
            if (_isRight)
                Debug.Log("SwipeDirection: IsRight");

            return _isRight;
        }
    }

    public bool IsDown
    {
        get
        {
            if (_isDown)
                Debug.Log("SwipeDirection: IsDown");

            return _isDown;
        }
    }

    public bool IsLeft
    {
        get
        {
            if (_isLeft)
                Debug.Log("SwipeDirection: IsLeft");

            return _isLeft;
        }
    }
}