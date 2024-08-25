using System;
using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
    private int _resourcesQuantity = 0;

    public event Action<int> QuantityChanged;

    public void ChangeResourceQuantity(int quantity)
    {
        _resourcesQuantity += quantity;
        QuantityChanged?.Invoke(_resourcesQuantity);
    }
}
