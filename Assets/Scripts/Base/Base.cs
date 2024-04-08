using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private UnitGenerator _unitGenerator;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private int _startUnitsQuantity;
    [SerializeField] private float _timeUntilNextUnitShipment = 2f;

    private List<Unit> _units;
    private int resourceQuantity = 0;

    public event Action<int> QuantityChanged;

    private void Awake()
    {
        _units = new List<Unit>();
    }

    private void Start()
    {
        CreateUnits(_startUnitsQuantity);
        StartCoroutine(SendUnit());
    }

    public void GetResource(Resource resource)
    {
        resourceQuantity += resource.Count;
        QuantityChanged?.Invoke(resourceQuantity);
    }

    private IEnumerator SendUnit()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_timeUntilNextUnitShipment);

        while (enabled)
        {
            _scanner.ScanSpace();

            if (_scanner.ResourceCount > 0)
            {
                TrySendingUnit();
            }

            yield return waitTime;
        }
    }

    private void CreateUnits(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            Unit unit = _unitGenerator.GenerateUnit();
            unit.SetBase(transform.position);

            _units.Add(unit);
        }
    }

    private void TrySendingUnit()
    {
        Unit unit = GetAvailableUnit();
        Resource freeResource = _scanner.GetResource();
        
        if (unit != null && freeResource != null)
        {   
            unit.SetTarget(freeResource);
        }
    }

    private Unit GetAvailableUnit()
    {
        foreach (Unit unit in _units)
        {
            if (unit.IsAvailable == true)
                return unit;
        }

        return null;
    }
}
