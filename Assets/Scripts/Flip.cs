using UnityEngine;

public class Flip
{
    private FlipCommand _command;

    public Flip(
        SwipeDirection swipeDirection,
        Transform currentTransform, 
        Transform targetTransform, 
        float duration)
    {
        if (swipeDirection.IsUp)
        {
            _command = new FlipUp(
                ref currentTransform,
                targetTransform,
                duration
            );
        }
        else if (swipeDirection.IsRight)
        {
            _command = new FlipRight(
                ref currentTransform,
                targetTransform,
                duration
            );
        }
        else if (swipeDirection.IsDown)
        {
            _command = new FlipDown(
                ref currentTransform,
                targetTransform,
                duration
            );
        }
        else if (swipeDirection.IsLeft)
        {
            _command = new FlipLeft(
                ref currentTransform,
                targetTransform,
                duration
            );
        }
    }

    public FlipCommand Command => _command;
}