using UnityEngine;

public class BaseSelector : MonoBehaviour
{
    private const KeyCode SelectKeyCode = KeyCode.Mouse0;

    private Base _base;
    private RaycastHit _hit;
    private bool _isSelect = false;

    public Base Base => _base;
    public bool IsSelect => _isSelect;

    private void Update()
    {
        if (Input.GetKeyDown(SelectKeyCode))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                _isSelect = true;

                if (hit.collider.TryGetComponent(out Base selectedBase))
                {
                    _base = selectedBase;
                    _hit = hit;
                }
            }
        }
    }
}
