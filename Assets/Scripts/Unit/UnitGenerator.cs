using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    [SerializeField] private Transform _baseZone;
    [SerializeField] private BaseSelector _baseSelector;
    [SerializeField] private Unit _prefab;

    private float _minXOffset = -2f;
    private float _maxXOffset = 2f;
    private float _minZOffset = -2f;
    private float _maxZOffset = 2f;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InitializeUnit();
        }
    }

    public void InitializeUnit()
    {
        if (_baseSelector.Base != null)
        {
            if (_baseSelector.Base.ResourcesCounter.ResourcesQuantity >= _baseSelector.Base.AmountOfResourcesToSpawnUnit)
            {
                _baseZone = _baseSelector.Base.DropZone.transform;
                Unit unit = GenerateUnit();
                unit.SetBase(_baseSelector.Base);
                _baseSelector.Base.AddUnit(unit);
            }
        }   
    }

    public Unit GenerateUnit()
    {
        Vector3 spawnPoint = GetRandomPosition();
        Unit unit = Instantiate(_prefab);
        unit.transform.position = spawnPoint;
        
        return unit;
    }

    private Vector3 GetRandomPosition()
    {
        float xOffset = Random.Range(_minXOffset, _maxXOffset);
        float zOffset = Random.Range(_minZOffset, _maxZOffset);

        Vector3 spawnPoint = new Vector3(_baseZone.position.x + xOffset, 0.5f, _baseZone.position.z + zOffset);

        return spawnPoint;
    }
}
