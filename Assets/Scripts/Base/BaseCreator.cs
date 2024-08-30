using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] private UnitGenerator _unitGenerator;

    public void CreateBase(Unit unit)
    {
        Base newBase = Instantiate(_basePrefab, unit.transform.position, _basePrefab.transform.rotation);
        newBase.Initialize(
            _resourceStorage,
            _unitGenerator);
        newBase.AddUnit(unit);
        unit.SetBase(newBase);
    }
}
