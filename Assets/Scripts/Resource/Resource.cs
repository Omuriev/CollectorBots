using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private int _count = 1;
    private bool _isBorrow = false;

    public event Action<Resource> Destroyed;

    public int Count => _count;
    public bool IsBorrow => _isBorrow;

    public void Throw() => Destroyed?.Invoke(this);

    public void BorrowResource(bool isBorrow)
    {
        _isBorrow = isBorrow;
    }
}
