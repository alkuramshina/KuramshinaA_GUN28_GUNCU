using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotate;

    private Rigidbody _rigidbody;
    
    private IEnumerator Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        while (true)
        {
            var deltaRotation = Quaternion.Euler(_rotate * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);

            yield return new WaitForFixedUpdate();
        }
    }
}
