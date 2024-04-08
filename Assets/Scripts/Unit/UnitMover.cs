using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _maxDistanceToObject = 0.5f;
    [SerializeField] private Rigidbody _rigidbody;

    private float _gravityFactor = 2f;

    public void MoveTo(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) > _maxDistanceToObject)
        {
            Vector3 verticalVelocity = _rigidbody.velocity;
            verticalVelocity.y = 0;
            verticalVelocity += Physics.gravity * Time.deltaTime * _gravityFactor;

            transform.position = Vector3.MoveTowards(transform.position, target + verticalVelocity, Time.deltaTime * _speed);
        }
    }
}
