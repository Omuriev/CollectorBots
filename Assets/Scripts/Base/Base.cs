using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private const KeyCode SelectKeyCode = KeyCode.Mouse0;
    
    [SerializeField] private DropZone _dropZone;
    [SerializeField] private int _startUnitsQuantity;
    [SerializeField] private float _timeUntilNextUnitShipment = 2f;
    [SerializeField] private UnitGenerator _unitGenerator;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] private ResourcesCounter _resourcesCounter;

    private List<Unit> _units;
    private List<Resource> _resources;

    private Flag _currentFlag;
    private Coroutine _sendUnitCoroutine;

    private int _amountOfResourcesToBuyUnit = 3;
    private int _amountOfResourcesToCreateBase = 5;

    public DropZone DropZone => _dropZone;
    public Flag CurrentFlag => _currentFlag;

    private void Awake()
    {
        _units = new List<Unit>();
        _resources = new List<Resource>();
    }

    private void Start()
    {
        CreateUnits(_startUnitsQuantity);

        if (_sendUnitCoroutine != null)
        {
            StopCoroutine(_sendUnitCoroutine);
        }

        _sendUnitCoroutine = StartCoroutine(SendUnit());
    }

    public void Initialize(
        ResourceStorage storage, 
        UnitGenerator unitGenerator,
        int unitQuantity = 0)
    {
        _resourceStorage = storage;
        _unitGenerator = unitGenerator;
        
        SetStartUnitsQuantity(unitQuantity);
    }

    public int SetStartUnitsQuantity(int value) => _startUnitsQuantity = value;

    public void TakeResource(Resource resource)
    {
        int resourceCount = 1;
        _resourcesCounter.AccrueResources(resourceCount);
    }

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
        unit.SetBase(this);
    }

    public void BuyUnit()
    {
        if (_resourcesCounter.ResourcesQuantity >= _amountOfResourcesToBuyUnit)
        {
            _unitGenerator.InitializeUnit(this);
            _resourcesCounter.WriteOffResources(_amountOfResourcesToBuyUnit);
        }
    }

    public void CreateFlag(Vector3 hitPoint)
    {
        if (_currentFlag == null)
        {
            _currentFlag = Instantiate(_flagPrefab, hitPoint, Quaternion.identity);
            _currentFlag.transform.position = hitPoint;
        }
        else
        {
            _currentFlag.transform.position = hitPoint;
        }
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
                    _units.Remove(unit);
                    unit.MakeInaccessible();
                    unit.SetTarget(_currentFlag.transform.position);
                    _resourcesCounter.WriteOffResources(_amountOfResourcesToCreateBase);
                    unit = null;

                    yield return waitTime;
                }

                TrySendingUnit(unit);
            }

            yield return waitTime;
        }
    }

    private void TrySendingUnit(Unit unit)
    {
        Resource resource = _resourceStorage.GetResource();

        if (unit != null && resource != null)
        {
            unit.SetTarget(resource.transform.position, resource);
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
