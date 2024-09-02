using System;
using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
    private int _resourcesQuantity = 0;

    public event Action<int> QuantityChanged;

    public int ResourcesQuantity => _resourcesQuantity;

    private void Start()
    {
        QuantityChanged?.Invoke(_resourcesQuantity);
    }

    public void AccrueResources(int quantity)
    {
        if (quantity >= 0)
        {
            _resourcesQuantity += quantity;
            QuantityChanged?.Invoke(_resourcesQuantity);
        }
    }

    public void WriteOffResources(int quantity)
    {
        if (quantity >= 0)
        {
            _resourcesQuantity -= quantity;
            QuantityChanged?.Invoke(_resourcesQuantity);
        }  
    }
}
