using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private float _maxDistance;
    private Collider _collider;
    private RaycastHit _hit;
    private GameObject _hitObject;

    public GameObject HitObject => _hitObject;

    private void Awake() 
    {
        _collider = GetComponent<Collider>();
    }

    void Start()
    {
        _maxDistance = 1.0f;        
    }

    public bool CanSwipeLeft
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

    public bool CanSwipeRight
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

    public bool CanSwipeUp
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

    public bool CanSwipeDown
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