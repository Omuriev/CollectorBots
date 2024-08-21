using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    private Unit _currentUnit;
    private Base _base;

    public event Action Destroyed;

    public Unit AssignedUnit => _currentUnit;

    public void AssignUnit(Unit unit, Flag flag)
    {
        _currentUnit = unit;
        _currentUnit.UnitMover.MoveTo(flag.transform.position);
    }

    public void CreateBase()
    {
        _base = Instantiate(_basePrefab, transform.position, _basePrefab.transform.rotation);
        _base.AddUnit(_currentUnit);
        _base.SetStartUnitsQuantity(0);
        _currentUnit.ResetUnit();
        _currentUnit.SetBase(_base);

        Destroyed?.Invoke();
    }
}
