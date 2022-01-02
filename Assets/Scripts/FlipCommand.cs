using System.Collections;
using UnityEngine;

public abstract class FlipCommand
{
    protected Transform CurrentTransform;
    protected Transform TargetTransform;
    protected float Duration;

    public FlipCommand(ref Transform currentTransform, Transform targetTransform, float duration)
    {
        CurrentTransform = currentTransform;
        TargetTransform = targetTransform;
        Duration = duration;
    }

    public abstract IEnumerator Execute();

    public abstract IEnumerator Undo();

    public IEnumerator Rotate(Quaternion quaternion, bool isUndo = false)
    {
        float _jumpHeight = .6f;
        float progress = 0f;
        var endRotation = quaternion;

        var ingredient = TargetTransform.gameObject.GetComponent<Ingredient>();
        var hitObject = ingredient.HitObject;
        var currentObject = CurrentTransform.gameObject;

        var currentPosition = currentObject.transform.position;
        var currentRotation = currentObject.transform.rotation;

        Stack targetStack = hitObject.GetComponent<Stack>();
        Stack currentStack = currentObject.GetComponent<Stack>();

        var colliderSize = CurrentTransform.gameObject.GetComponent<BoxCollider>().size;

        var projectionPosition = new Vector3(
            0, 
            colliderSize.y * currentStack.Childrens.Length +  colliderSize.y * targetStack.Childrens.Length,
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
            CurrentTransform.rotation = Quaternion.Lerp(currentRotation, endRotation, percent);
            yield return null;
        }

        // make as child of target
        if (hitObject != null)
        {
            CurrentTransform.parent = hitObject.transform;
        }

        // _collider.enabled = false;
        yield return new WaitForSeconds(0.5f);

        hitObject.GetComponent<Stack>().UpdateStack();

        GameManager.Instance.IngredientIsAnimating = false;
        
        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.FindStack();
    }
}

public class FlipUp : FlipCommand
{
    public FlipUp(ref Transform currentTransform, Transform targetTransform, float duration) :  
        base(ref currentTransform, targetTransform, duration) {}

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
    public FlipRight(ref Transform currentTransform, Transform targetTransform, float duration) :  
        base(ref currentTransform, targetTransform, duration) {}

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
    public FlipDown(ref Transform currentTransform, Transform targetTransform, float duration) :  
        base(ref currentTransform, targetTransform, duration) {}

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
    public FlipLeft(ref Transform currentTransform, Transform targetTransform, float duration) :  
        base(ref currentTransform, targetTransform, duration) {}

    public override IEnumerator Execute()
    {
        yield return base.Rotate(Quaternion.Euler(0,0,-180));
    }

    public override IEnumerator Undo()
    {
        yield return base.Rotate(Quaternion.Euler(0,0,180), true);
    }
}