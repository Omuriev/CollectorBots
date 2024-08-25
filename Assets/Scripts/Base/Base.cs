using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private const KeyCode SelectKeyCode = KeyCode.Mouse0;
    private const KeyCode BuyUnitKeyCode = KeyCode.E;

    [SerializeField] private DropZone _dropZone;
    [SerializeField] private int _startUnitsQuantity;
    [SerializeField] private float _timeUntilNextUnitShipment = 2f;
    [SerializeField] private BaseSelector _baseSelector;
    [SerializeField] private UnitGenerator _unitGenerator;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private ResourcesCounter _resourcesCounter;

    private List<Unit> _units;

    private Flag _currentFlag;
    private Coroutine _sendUnitCoroutine;
    private int _resourceAmount;

    private int _amountOfResourcesToBuyUnit = 3;
    private int _amountOfResourcesToCreateBase = 5;

    public DropZone DropZone => _dropZone;
    public ResourcesCounter ResourcesCounter => _resourcesCounter;
    public Scanner Scanner => _scanner;
    public UnitGenerator UnitGenerator => _unitGenerator;
    public BaseSelector BaseSelector => _baseSelector;

    private void Awake() => _units = new List<Unit>();

    private void Start()
    {
        CreateUnits(_startUnitsQuantity);

        if (_sendUnitCoroutine != null)
        {
            StopCoroutine(_sendUnitCoroutine);
        }

        _sendUnitCoroutine = StartCoroutine(SendUnit());
    }

    private void Update()
    {
        SetFlag();
        BuyUnit();
    }

    public void Initialize(ResourcesCounter resourcesCounter, Scanner scanner, UnitGenerator unitGenerator, BaseSelector baseSelector, int unitQuantity = 0)
    {
        _resourcesCounter = resourcesCounter;
        _scanner = scanner;
        _unitGenerator = unitGenerator;
        _baseSelector = baseSelector;
        SetStartUnitsQuantity(unitQuantity);
    }

    public int SetStartUnitsQuantity(int value) => _startUnitsQuantity = value;

    public void GetResource(Resource resource)
    {
        _resourceAmount += resource.Count;
        _resourcesCounter.ChangeResourceQuantity(resource.Count);
    }

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
        unit.SetBase(this);
    }

    private void BuyUnit()
    {
        if (Input.GetKeyDown(BuyUnitKeyCode) && _baseSelector.Base == this && _baseSelector.IsSelect == true)
        {
            if (_resourceAmount >= _amountOfResourcesToBuyUnit)
            {
                _unitGenerator.InitializeUnit(this);
                _resourceAmount -= _amountOfResourcesToBuyUnit;
                _resourcesCounter.ChangeResourceQuantity(-_amountOfResourcesToBuyUnit);
            }
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
                if (_currentFlag != null && _resourceAmount >= _amountOfResourcesToCreateBase)
                {
                    _currentFlag.AssignUnit(unit);
                    _units.Remove(unit);
                    unit.ÑhangeAvailability(false);
                    _resourceAmount -= _amountOfResourcesToCreateBase;
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

    private void SetFlag()
    {
        if (_baseSelector.IsSelect == true && _baseSelector.Base == this)
        {
            if (Input.GetKeyDown(SelectKeyCode))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    if (hit.collider.TryGetComponent(out Base selectedBase) == false)
                    {
                        if (_currentFlag == null)
                        {
                            _currentFlag = CreateFlag(hit.point);
                            _currentFlag.transform.position = hit.point;
                        }
                        else
                        {
                            _currentFlag.transform.position = hit.point;
                        }
                    }
                }
            }
        }
    }

    private Flag CreateFlag(Vector3 hitPoint)
    {
        return Instantiate(_flagPrefab, hitPoint, Quaternion.identity);
    }
}
