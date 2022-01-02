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
        Stack[] stacks = GetComponents<Stack>();

        foreach (Stack stack in stacks)
        {
            if (stack != this)
                Destroy(stack);
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
