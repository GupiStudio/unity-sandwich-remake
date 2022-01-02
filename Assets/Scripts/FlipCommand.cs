using System.Collections;
using UnityEngine;

public abstract class FlipCommand
{
    private readonly Ingredient _ingredient;
    protected float Duration;

    public FlipCommand(Ingredient ingredient, float duration)
    {
        _ingredient = ingredient;
        Duration = duration;
    }

    protected Ingredient Ingredient => _ingredient;

    public abstract IEnumerator Execute();

    public abstract IEnumerator Undo();

    public IEnumerator Rotate(Quaternion quaternion, bool isUndo = false)
    {
        float _jumpHeight = .6f;
        float progress = 0f;
        var endRotation = quaternion;

        var hitObject = _ingredient.HitObject;
        var currentObject = _ingredient.gameObject;

        var currentPosition = currentObject.transform.position;
        var currentRotation = currentObject.transform.rotation;

        Stack targetStack = hitObject.GetComponent<Stack>();
        Stack currentStack = currentObject.GetComponent<Stack>();

        var collider = _ingredient.gameObject.GetComponent<BoxCollider>();

        var projectionPosition = new Vector3(
            0, 
            collider.size.y * currentStack.Childrens.Length +  collider.size.y * targetStack.Childrens.Length,
            0);
        
        var adderVector = isUndo ? Vector3.zero : projectionPosition;
        
        var targetPosition = hitObject.transform.position + adderVector;

        GameManager.Instance.IngredientIsAnimating = true;

        while(progress < Duration)
        {
            progress += Time.deltaTime;
            var percent = Mathf.Clamp01(progress / Duration);
            float height = (_jumpHeight) * Mathf.Sin(Mathf.PI * percent);

            currentObject.transform.position = Vector3.Lerp(
                currentPosition, 
                targetPosition, 
                percent) + new Vector3(0, height, 0);

            _ingredient.transform.rotation = Quaternion.Lerp(currentRotation, endRotation, percent);
            yield return null;
        }

        // make as child of target
        if (hitObject != null)
        {
            _ingredient.transform.parent = hitObject.transform;
        }

        collider.enabled = isUndo;

        yield return new WaitForSeconds(0.5f);

        hitObject.GetComponent<Stack>().UpdateStack();

        GameManager.Instance.IngredientIsAnimating = false;
        
        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.FindStack();
    }
}

public class FlipUp : FlipCommand
{
    public FlipUp(Ingredient ingredient, float duration) :  
        base(ingredient, duration) {}

    public override IEnumerator Execute()
    {
        yield return base.Rotate(Quaternion.Euler(-180,0,0));
    }

    public override IEnumerator Undo()
    {
        yield return base.Rotate(Quaternion.Euler(180,0,0), true);
    }
}

public class FlipRight : FlipCommand
{
    public FlipRight(Ingredient ingredient, float duration) :  
        base(ingredient, duration) {}

    public override IEnumerator Execute()
    {
        yield return base.Rotate(Quaternion.Euler(0,0,180));
    }

    public override IEnumerator Undo()
    {
        yield return base.Rotate(Quaternion.Euler(0,0,-180), true);
    }
}

public class FlipDown : FlipCommand
{
    public FlipDown(Ingredient ingredient, float duration) :  
        base(ingredient, duration) {}

    public override IEnumerator Execute()
    {
        yield return base.Rotate(Quaternion.Euler(180,0,0));
    }

    public override IEnumerator Undo()
    {
        yield return base.Rotate(Quaternion.Euler(-180,0,0), true);
    }
}

public class FlipLeft : FlipCommand
{
    public FlipLeft(Ingredient ingredient, float duration) :  
        base(ingredient, duration) {}

    public override IEnumerator Execute()
    {
        yield return base.Rotate(Quaternion.Euler(0,0,-180));
    }

    public override IEnumerator Undo()
    {
        yield return base.Rotate(Quaternion.Euler(0,0,180), true);
    }
}