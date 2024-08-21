using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private DropZone _dropZone;
    [SerializeField] private int _startUnitsQuantity;
    [SerializeField] private float _timeUntilNextUnitShipment = 2f;

    private List<Unit> _units;
    private Flag _currentFlag;
    private ResourcesCounter _resourcesCounter; 
    private Scanner _scanner;
    private Coroutine _sendUnitCoroutine;
    private UnitGenerator _unitGenerator;

    private int _amountOfResourcesToSpawnUnit = 3;
    private int _amountOfResourcesToCreateBase = 5;

    public DropZone DropZone => _dropZone;
    public ResourcesCounter ResourcesCounter => _resourcesCounter;
    public int AmountOfResourcesToSpawnUnit => _amountOfResourcesToSpawnUnit;

    private void Awake() => _units = new List<Unit>();

    private void Start()
    {
        Initialize();

        if (_sendUnitCoroutine != null)
        {
            StopCoroutine(_sendUnitCoroutine);
        }

        _sendUnitCoroutine = StartCoroutine(SendUnit());
    }

    public void Initialize()
    {
        _resourcesCounter = FindObjectOfType<ResourcesCounter>();
        _scanner = FindObjectOfType<Scanner>();
        _unitGenerator = FindObjectOfType<UnitGenerator>();

        CreateUnits(_startUnitsQuantity);
    }

    public int SetStartUnitsQuantity(int value) => _startUnitsQuantity = value;

    public void SetFlag(Flag flag) => _currentFlag = flag;

    public void GetResource(Resource resource) => _resourcesCounter.ChangeResourceQuantity(resource.Count);

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
        unit.SetBase(this);
    }

    private void CreateUnits(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            Unit unit = _unitGenerator.GenerateUnit();
            unit.SetBase(this);

            _units.Add(unit);
        }
    }

    private IEnumerator SendUnit()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_timeUntilNextUnitShipment);

        while (enabled)
        {
            Unit unit = GetAvailableUnit();

            if (unit != null)
            {
                if (_currentFlag != null && _resourcesCounter.ResourcesQuantity >= _amountOfResourcesToCreateBase)
                {
                    _currentFlag.BaseCreator.AssignUnit(unit, _currentFlag);
                    _units.Remove(unit);
                    unit.ÑhangeAvailability(false);
                    _resourcesCounter.ChangeResourceQuantity(-_amountOfResourcesToCreateBase);
                    _currentFlag = null;
                    unit = null;

                    yield return waitTime;
                }

                _scanner.ScanSpace();

                if (_scanner.ResourceCount > 0)
                {
                    TrySendingUnit(unit);
                }
            }

            yield return waitTime;
        }
    }

    private void TrySendingUnit(Unit unit)
    {
        Resource freeResource = _scanner.GetResource();

        if (unit != null && freeResource != null)
        {
            unit.SetTarget(freeResource);
            unit.BringResource();
        }
    }

    private Unit GetAvailableUnit()
    {
        foreach (Unit unit in _units)
        {
            if (unit.IsAvailable == true)
            {
                return unit;
            }
        }

        return null;
    }
}
