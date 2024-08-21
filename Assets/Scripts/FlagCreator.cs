using UnityEngine;

public class FlagCreator : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    private Base _currentBase;
    private Flag _currentFlag;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Base selectedBase))
                {
                    _currentBase = selectedBase;

                    if (_currentFlag != null)
                    {
                        Destroy(_currentFlag.gameObject);
                        _currentFlag = null;
                    }
                }
                else
                {
                    if (_currentBase != null)
                    {
                        if (_currentFlag == null)
                        {
                            CreateFlag(hit.point);
                        }
                        else
                        {
                            _currentFlag.transform.position = hit.point;
                        }
                    }
                }
            }
        }
    }

    private void CreateFlag(Vector3 spawnPoint)
    {
        _currentFlag = Instantiate(_flag, spawnPoint, Quaternion.identity);
        _currentBase.SetFlag(_currentFlag);
    }
}
