using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRemover : MonoBehaviour
{
    [SerializeField] private ObjectPool _pool;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resource resource))
        {
            _pool.PutObject(resource);
        }
    }
}
