using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    public void CreateBase(Unit unit)
    {
        Base newBase = Instantiate(_basePrefab, transform.position, _basePrefab.transform.rotation);
        newBase.Initialize(
            unit.CurrentBase.ResourceStorage,
            unit.CurrentBase.UnitGenerator,
            unit.CurrentBase.ResourcesCounter.Displayer);
        newBase.AddUnit(unit);
        unit.Reset();
        unit.SetBase(newBase);
    }
}
