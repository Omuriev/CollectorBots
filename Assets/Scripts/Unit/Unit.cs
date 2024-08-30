using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMover _unitMover;
    [SerializeField] private Transform _hand;
    
    private bool _isAvailable = true;

    private Resource _currentResource;
    private Resource _targetResource;
    private Vector3 _targetPoint;
    private Base _currentBase;
    private float _throwForce = 10f;
    private BaseCreator _baseCreator;

    public bool IsAvailable => _isAvailable;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Resource resource))
            PickupResource(resource);

        if (collider.gameObject.TryGetComponent(out DropZone dropZone))
            ThrowResource(dropZone);

        if (collider.TryGetComponent(out Flag flag))
            BuildBase(flag);
    }

    public void Reset()
    {
        _currentResource = null;
        _isAvailable = true;
    }

    public void SetBase(Base currentBase) => _currentBase = currentBase;
    public void SetBaseCreator(BaseCreator baseCreator) => _baseCreator = baseCreator;

    public void MakeAccessible() => _isAvailable = true;
    public void MakeInaccessible() => _isAvailable = false;

    public void SetTarget(Vector3 position, Resource resource = null)
    {
        _targetPoint = position;
        _unitMover.MoveTo(_targetPoint);

        if (resource != null)
            _targetResource = resource;

        MakeInaccessible();
    }

    private void BuildBase(Flag flag)
    {
        Reset();

        if (_currentBase.CurrentFlag == flag)
        {
            Destroy(flag.gameObject);
            _baseCreator.CreateBase(this);
        }
    }

    private void ThrowResource(DropZone dropZone)
    {
        if (_currentBase.DropZone == dropZone)
        {
            if (_currentResource != null)
            {
                if (_currentResource.TryGetComponent(out Rigidbody rigidbody))
                {
                    rigidbody.isKinematic = false;
                    rigidbody.AddForce(Vector3.forward * _throwForce * Time.deltaTime);

                    _currentBase.TakeResource(_currentResource);

                    _currentResource.gameObject.transform.SetParent(null, true);
                    _currentResource.Throw();
                    _currentResource = null;
                    _targetResource = null;

                    MakeAccessible();
                }
            }
        }
    }

    private void PickupResource(Resource resource)
    {
        if (_targetResource == resource)
        {
            _currentResource = resource;

            if (_currentResource.TryGetComponent(out Rigidbody rigidbody))
            {
                _currentResource.gameObject.transform.SetParent(_hand);
                _currentResource.transform.position = _hand.position;
                rigidbody.isKinematic = true;

                _targetPoint = _currentBase.transform.position;
                _unitMover.MoveTo(_targetPoint);
            }
        }
    }
}
