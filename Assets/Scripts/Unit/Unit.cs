using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMover _unitMover;
    [SerializeField] private Transform _hand;
    [SerializeField] private BaseCreator _creator;

    private bool _isAvailable = true;

    private Resource _resource;
    private Vector3 _target;
    private Base _currentBase;
    private float _throwForce = 10f;

    public bool IsAvailable => _isAvailable;
    public Base CurrentBase => _currentBase;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Resource resource))
        {
            if (_resource == resource)
            {
                PickupResource();

                _target = _currentBase.transform.position;
                _unitMover.MoveTo(_target);
            }
        }

        if (collider.gameObject.TryGetComponent(out DropZone dropZone))
        {
            if (_currentBase.DropZone == dropZone)
            {
                if (_resource != null)
                {
                    _currentBase.GetResource(_resource);
                    ThrowResource();

                    ÑhangeAvailability(true);
                }
            }
        }

        if (collider.TryGetComponent(out Flag flag))
        {
            if (_currentBase.CurrentFlag == flag)
            {
                Destroy(flag.gameObject);
                _creator.CreateBase(this);
            }
        }
    }

    public void BringResource()
    {
        _unitMover.MoveTo(_target);
    }

    public void Reset()
    {
        _resource = null;
        _isAvailable = true;
    }

    public void SetBase(Base currentBase) => _currentBase = currentBase;

    public void ÑhangeAvailability(bool value) => _isAvailable = value;

    public void SetTargetResource(Resource resource) => _resource = resource;

    public void SetTarget(Vector3 position)
    {
        _target = position;
        _unitMover.MoveTo(_target);
        ÑhangeAvailability(false);
    }

    private void ThrowResource()
    {
        if (_resource.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(Vector3.forward * _throwForce * Time.deltaTime);

            _resource.gameObject.transform.SetParent(null, true);
            _resource.Throw();
            _resource = null;
        }
    }

    private void PickupResource()
    {
        if (_resource.TryGetComponent(out Rigidbody rigidbody))
        {
            _resource.gameObject.transform.SetParent(_hand);
            _resource.transform.position = _hand.position;

            rigidbody.isKinematic = true;
        }
    }
}
