using UnityEngine;

public class Flip
{
    private FlipCommand _command;

    public Flip(
        Ingredient ingredient,
        SwipeDirection swipeDirection,
        float duration)
    {
        if (swipeDirection.IsUp)
        {
            if (!ingredient.CanFlipUp) { return; }

            _command = new FlipUp(
                ingredient,
                duration
            );
        }
        else if (swipeDirection.IsRight)
        {
            if (!ingredient.CanFlipRight) { return; }

            _command = new FlipRight(
                ingredient,
                duration
            );
        }
        else if (swipeDirection.IsDown)
        {
            if (!ingredient.CanFlipDown) { return; }

            _command = new FlipDown(
                ingredient,
                duration
            );
        }
        else if (swipeDirection.IsLeft)
        {
            if (!ingredient.CanFlipLeft) { return; }

            _command = new FlipLeft(
                ingredient,
                duration
            );
        }
    }

    public FlipCommand Command => _command;
}