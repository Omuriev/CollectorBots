using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private BoxCollider _spawnZone;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private float spawnPositionY = 0f;

    private void Start() => StartCoroutine(GenerateResource());

    private IEnumerator GenerateResource()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_delay);

        while(enabled)
        {
            SpawnResource();
            yield return waitTime;
        }
    }

    private void SpawnResource()
    {
        float spawnPositionX = Random.Range(_spawnZone.bounds.min.x, _spawnZone.bounds.max.x);
        float spawnPositionZ = Random.Range(_spawnZone.bounds.min.z, _spawnZone.bounds.max.z);

        Vector3 spawnPoint = new Vector3(spawnPositionX, spawnPositionY, spawnPositionZ);

        var resource = _pool.GetObject();
        resource.transform.SetParent(_parentTransform);
        resource.gameObject.SetActive(true);
        resource.transform.position = spawnPoint;
        resource.Destroyed += OnDestroyed;
    }

    private void OnDestroyed(Resource resource)
    {
        resource.Destroyed -= OnDestroyed;
        _pool.PutObject(resource);
    }
}
