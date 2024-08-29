using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const KeyCode SelectKeyCode = KeyCode.Mouse0;
    private const KeyCode BuyUnitKeyCode = KeyCode.Q;

    private Base _base;

    private void Update()
    {
        if (Input.GetKeyDown(SelectKeyCode))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Base selectedBase))
                {
                    _base = selectedBase;
                }
                else
                {
                    if (_base != null)
                        _base.CreateFlag(hit.point);
                }
            }
        }

        if (Input.GetKeyDown(BuyUnitKeyCode))
        {
            if (_base != null)
                _base.BuyUnit();
        }
    }
}
