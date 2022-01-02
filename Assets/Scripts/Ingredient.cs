using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private float _maxDistance = 1.0f;
    private Collider _collider;
    private RaycastHit _hit;
    
    private GameObject _hitObject;
    public GameObject HitObject => _hitObject;

    private void Awake() 
    {
        _collider = GetComponent<Collider>();
    }

    public bool CanFlipLeft
    {
        get
        {
            if(Physics.BoxCast(
                _collider.bounds.center, 
                transform.localScale / 4, 
                transform.right, 
                out _hit, 
                transform.rotation, 
                _maxDistance))
            {
                _hitObject = _hit.collider.transform.gameObject;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool CanFlipRight
    {
        get
        {
            if(Physics.BoxCast(
                _collider.bounds.center, 
                transform.localScale / 4, 
                -transform.right, 
                out _hit, 
                transform.rotation, 
                _maxDistance))
            {
                _hitObject = _hit.collider.transform.gameObject;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool CanFlipUp
    {
        get
        {
            if (Physics.BoxCast(
                _collider.bounds.center, 
                transform.localScale / 4, 
                -transform.forward, 
                out _hit, 
                transform.rotation, 
                _maxDistance))
            {
                _hitObject = _hit.collider.transform.gameObject;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool CanFlipDown
    {
        get
        {
            if (Physics.BoxCast(
                _collider.bounds.center, 
                transform.localScale / 4, 
                transform.forward, 
                out _hit, 
                transform.rotation, 
                _maxDistance))
            {
                _hitObject = _hit.collider.transform.gameObject;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}