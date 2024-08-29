using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class ResourcesDisplayer : MonoBehaviour
{
    [SerializeField] private ResourcesCounter _resourcesCounter;
    [SerializeField] private TMP_Text _resourcesQuantityText;

    private List<ResourcesCounter> _countersList;

    private void Awake() => _countersList = new List<ResourcesCounter>();

    public void AddCounter(ResourcesCounter counter) => _countersList.Add(counter);

    public void UpdateInfo()
    {
        int resourcesCount = 0;

        foreach (var counter in _countersList)
        {
            resourcesCount += counter.ResourcesQuantity;
        }

        _resourcesQuantityText.text = "Resources: " + resourcesCount;
    }
}
