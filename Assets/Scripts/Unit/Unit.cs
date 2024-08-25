using System;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMover _unitMover;
    [SerializeField] private Transform _hand;
    
    private bool _isAvailable = true;

    private Resource _resource;
    private Vector3 _target;
    private Base _currentBase;
    private float _throwForce = 10f;

    public UnitMover UnitMover => _unitMover;
    public bool IsAvailable => _isAvailable;
    public Base CurrentBase => _currentBase;

    public void BringResource()
    {
        if (_resource != null)
            _unitMover.MoveTo(_target);
    }

    public void Reset()
    {
        _resource = null;
        _isAvailable = true;
    }

    public void SetBase(Base currentBase) => _currentBase = currentBase;

    public void ÑhangeAvailability(bool value) => _isAvailable = value;

    public void SetTarget(Resource targetResource)
    {
        _target = targetResource.transform.position;
        _resource = targetResource;

        ÑhangeAvailability(false);

       _resource.BorrowResource(true);
    }

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
            if (_currentBase.GetComponentInChildren<DropZone>() == dropZone)
            {
                if (_resource != null)
                {
                    _currentBase.GetResource(_resource);
                    ThrowResource();

                    ÑhangeAvailability(true);
                }
            }
        }
    }

    private void ThrowResource()
    {
        if (TryGetResourceRigidbody(out Rigidbody rigidbody) == false)
            return;

        rigidbody.isKinematic = false;
        rigidbody.AddForce(Vector3.forward * _throwForce * Time.deltaTime);

        _resource.BorrowResource(false);
        _resource.gameObject.transform.SetParent(null, true);
        _resource.Throw();
        _resource = null;
    }

    private void PickupResource()
    {
        if (TryGetResourceRigidbody(out Rigidbody rigidbody) == false)
            return;

        _resource.gameObject.transform.SetParent(_hand);
        _resource.transform.position = _hand.position;

        rigidbody.isKinematic = true;
    }

    private bool TryGetResourceRigidbody(out Rigidbody rigidbody)
    {
        rigidbody = _resource.GetComponent<Rigidbody>();

        if (rigidbody != null)
            return true;

        return false;
    }
}
