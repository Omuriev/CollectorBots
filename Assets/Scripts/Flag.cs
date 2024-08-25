using UnityEngine;

public class Flag : MonoBehaviour 
{
    [SerializeField] private Base _basePrefab;

    private Base _base;
    private ResourcesCounter _resourcesCounter;
    private Scanner _scanner;
    private UnitGenerator _unitGenerator;
    private Unit _currentUnit;

    public Unit AssignedUnit => _currentUnit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            if (unit == _currentUnit)
                CreateBase();
        }
    }

    public void AssignUnit(Unit unit)
    {
        _currentUnit = unit;
        _currentUnit.UnitMover.MoveTo(transform.position);
    }

    public void CreateBase()
    {
        Base currentUnitBase = _currentUnit.CurrentBase;
        _base = Instantiate(_basePrefab, transform.position, _basePrefab.transform.rotation);
        _base.Initialize(currentUnitBase.ResourcesCounter, currentUnitBase.Scanner, currentUnitBase.UnitGenerator, currentUnitBase.BaseSelector);
        _base.AddUnit(_currentUnit);
        _currentUnit.Reset();
        _currentUnit.SetBase(_base);
        Destroy(this.gameObject);
    }
}
