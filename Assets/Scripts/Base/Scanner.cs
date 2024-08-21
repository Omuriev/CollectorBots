using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform _resourcesParent;

    private List<Resource> _resources;
    private int _resourceCount;

    public int ResourceCount => _resourceCount;

    public void FindResourcesParent()
    {
        var resourceGenerator = FindObjectOfType<ResourceGenerator>();

        if (resourceGenerator != null)
            _resourcesParent = resourceGenerator.transform;
    }

    public Resource GetResource()
    {
        foreach (Resource resource in _resources)
        {
            if (resource.IsBorrow == false)
                return resource;
        }

        return null;
    }

    public void ScanSpace()
    {
        _resources = _resourcesParent.GetComponentsInChildren<Resource>().ToList();
        _resourceCount = _resources.Count;
    }
}
