using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    private List<Resource> _freeResources;

    private void Awake()
    {
        _freeResources = new List<Resource>();
    }

    public void AddResource(Resource resource)
    {
        _freeResources.Add(resource);
    }

    public Resource GetResource()
    {
        if (_freeResources.Count > 0)
        {
            Resource resource = _freeResources[0];
            return resource;
        }

        return null;
    }

   public void RemoveResource(Resource resource) => _freeResources.Remove(resource);
}
