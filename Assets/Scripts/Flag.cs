using UnityEngine;

public class Flag : MonoBehaviour 
{
    [SerializeField] private BaseCreator _baseCreator;

    public BaseCreator BaseCreator => _baseCreator;

    private void OnEnable() => _baseCreator.Destroyed += OnDestroyed;
    private void OnDisable() => _baseCreator.Destroyed -= OnDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            if (unit == _baseCreator.AssignedUnit)
                _baseCreator.CreateBase();
        }
    }

    private void OnDestroyed() => Destroy(gameObject);
}
