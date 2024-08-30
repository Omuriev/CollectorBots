using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    [SerializeField] private Transform _baseZone;
    [SerializeField] private Unit _prefab;
    [SerializeField] private BaseCreator _baseCreator;

    private float _minXOffset = -2f;
    private float _maxXOffset = 2f;
    private float _minZOffset = -2f;
    private float _maxZOffset = 2f;

    public void InitializeUnit(Base selectedBase)
    {
        _baseZone = selectedBase.DropZone.transform;
        Unit unit = GenerateUnit();
        unit.SetBase(selectedBase);
        selectedBase.AddUnit(unit);  
    }

    public Unit GenerateUnit()
    {
        Unit unit = Instantiate(_prefab, GetRandomPosition(), Quaternion.identity);
        unit.SetBaseCreator(_baseCreator);

        return unit;
    }

    private Vector3 GetRandomPosition()
    {
        float xOffset = Random.Range(_minXOffset, _maxXOffset);
        float zOffset = Random.Range(_minZOffset, _maxZOffset);

        return new Vector3(_baseZone.position.x + xOffset, 0.5f, _baseZone.position.z + zOffset);
    }
}
