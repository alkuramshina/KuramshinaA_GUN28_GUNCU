using System.Collections;
using UnityEngine;

namespace Netologia.Homework
{
    public class NewMover: MonoBehaviour
    {
        [SerializeField] private Vector3 _start;
        [SerializeField] private Vector3 _end;

        [SerializeField] private float _speed;
        [SerializeField] private float _delay;

        private IEnumerator Start()
        {
            var start = _start;
            var end = _end;
            
            while (true)
            {
                transform.position = Vector3.MoveTowards(transform.position, end, 
                    _speed * Time.deltaTime); 
                
                if (Vector3.Distance(transform.position, end) < 0.001f)
                {
                    (start, end) = (end, start);
                    
                    yield return new WaitForSeconds(_delay);
                }
                
                yield return null;
            }
        }
    }
}