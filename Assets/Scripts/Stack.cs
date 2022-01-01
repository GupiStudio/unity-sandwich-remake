using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    private Collider _collider;
    private Transform[] _childrens;

    public Transform[] Childrens => _childrens;

    private void Awake() 
    {
        _childrens = GetComponentsInChildren<Transform>();
        _collider = GetComponent<Collider>();
    }

    void Start()
    {
        Stack[] allStacksInObject = GetComponents<Stack>();
        
        for (int i = 0; i < allStacksInObject.Length; i++)
        {
            if (allStacksInObject[i] != this)
            {
                Destroy(allStacksInObject[i]);
            }
        }
    }

    public void UpdateChildrens()
    {
        _childrens = GetComponentsInChildren<Transform>();
        
        for (int i = 1; i < _childrens.Length; i++)
        {
            Destroy(_childrens[i].GetComponent<Stack>());
        }
    }
}
