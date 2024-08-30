using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class ResourcesDisplayer : MonoBehaviour
{
    [SerializeField] private ResourcesCounter _resourcesCounter;
    [SerializeField] private TMP_Text _resourcesQuantityText;

    private void OnEnable() => _resourcesCounter.QuantityChanged += OnQuantityChanged;
    private void OnDisable() => _resourcesCounter.QuantityChanged -= OnQuantityChanged;

    private void OnQuantityChanged(int quantity)
    {
        _resourcesQuantityText.text = "Resources: " + quantity;
    }
}
