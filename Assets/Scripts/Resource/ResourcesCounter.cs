using System;
using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
    private ResourcesDisplayer _displayer;
    private int _resourcesQuantity = 0;

    public int ResourcesQuantity => _resourcesQuantity;

    public ResourcesDisplayer Displayer => _displayer;

    public void SetResourceDisplayer(ResourcesDisplayer displayer)
    {
        _displayer = displayer;
        _displayer.AddCounter(this);
    }

    public void AccrueResources(int quantity)
    {
        if (quantity >= 0)
        {
            _resourcesQuantity += quantity;
            _displayer.UpdateInfo();
        }
    }

    public void WriteOffResources(int quantity)
    {
        if (quantity >= 0)
        {
            _resourcesQuantity -= quantity;
            _displayer.UpdateInfo();
        }
            
    }
}
