using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitMover _unitMover;
    [SerializeField] private Transform _hand;
    
    private bool _isAvailable = true;

    private Resource _resource;
    private Vector3 _target;
    private Vector3 _basePosition;

    public bool IsAvailable => _isAvailable;

    private void Update()
    {
       BringResource();
    }

    public void SetBase(Vector3 basePosition)
    {
        _basePosition = basePosition;
    }

    public void SetTarget(Resource targetResource)
    {
        _target = targetResource.transform.position;
        _resource = targetResource;

        _isAvailable = false;

       _resource.BorrowResource(true);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Resource resource))
        {
            if (_resource == resource)
            {
                PickupResource();

                _target = _basePosition;
            }
        }

        if (collider.gameObject.TryGetComponent(out Base unitBase))
        {
            if (_resource != null)
            {
                unitBase.GetResource(_resource);
                ThrowResource();

                _isAvailable = true;
            }
        }
    }

    private void ThrowResource()
    {
        if (TryGetResourceRigidbody(out Rigidbody rigidbody) == false)
            return;

        rigidbody.isKinematic = false;
        rigidbody.AddForce(Vector3.forward * 10f * Time.deltaTime);

        _resource.BorrowResource(false);

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

    private void BringResource()
    {
        if (_resource != null)
            _unitMover.MoveTo(_target);
    }
}
