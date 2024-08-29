using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Resource _prefab;

    private Queue<Resource> _resources;

    private void Awake() => _resources = new Queue<Resource>();

    public Resource GetObject()
    {
        if (_resources.Count == 0)
        {
            Resource resource = Instantiate(_prefab, transform.position, Quaternion.identity);
            return resource;
        }

        return _resources.Dequeue();
    }

    public void PutObject(Resource resource)
    {
        if (resource.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
            resource.gameObject.SetActive(false);
            _resources.Enqueue(resource);
        }
    }
}
