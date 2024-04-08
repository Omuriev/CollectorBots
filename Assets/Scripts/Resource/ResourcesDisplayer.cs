using UnityEngine;
using TMPro;

public class ResourcesDisplayer : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TMP_Text _resourcesQuantityText;

    private void OnEnable()
    {
        _base.QuantityChanged += OnResourceQuantityChanged;
    }

    private void OnDisable()
    {
        _base.QuantityChanged -= OnResourceQuantityChanged;
    }

    private void OnResourceQuantityChanged(int quantity)
    {
        _resourcesQuantityText.text = "Resources: " + quantity;
    }
}
