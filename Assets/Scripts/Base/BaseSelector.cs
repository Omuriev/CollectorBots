using UnityEngine;

public class BaseSelector : MonoBehaviour
{
    private Base _base;

    public Base Base => _base;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Base selectedBase))
                {
                    _base = selectedBase;
                }
            }
        }
    }
}
