using UnityEngine;
using TMPro;

public class ResourcesDisplayer : MonoBehaviour
{
    [SerializeField] private ResourcesCounter _resourcesCounter;
    [SerializeField] private TMP_Text _resourcesQuantityText;

    private void OnEnable() => _resourcesCounter.QuantityChanged += OnResourceQuantityChanged;

    private void OnDisable() => _resourcesCounter.QuantityChanged -= OnResourceQuantityChanged;

    private void OnResourceQuantityChanged(int quantity) => _resourcesQuantityText.text = "Resources: " + quantity;
}
