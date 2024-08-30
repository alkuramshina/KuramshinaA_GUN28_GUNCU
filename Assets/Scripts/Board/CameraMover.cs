using System.Collections;
using UnityEngine;

namespace Board
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform[] povs;
        [SerializeField] private float movingSpeed = 4f;

        private Camera _camera;
        private int _nextPosition;

        private bool _cameraIsMoving;

        private void Awake()
        {
            _camera = Camera.main;

            _camera.transform.position = povs[0].position;
            _camera.transform.rotation = povs[0].rotation;

            _nextPosition = CalculateNext(0);
        }

        public void MoveToNextPov()
        {
            StartCoroutine(CameraMovement());
        }
        
        private IEnumerator CameraMovement()
        {
            _cameraIsMoving = true;

            while (Vector3.Distance(_camera.transform.position, povs[_nextPosition].position) >= 0.01f)
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position,
                    povs[_nextPosition].position, movingSpeed * Time.deltaTime);

                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation,
                    povs[_nextPosition].rotation, movingSpeed * Time.deltaTime);

                yield return null;
            }

            _nextPosition = CalculateNext(_nextPosition);
            _cameraIsMoving = false;
        }

        private int CalculateNext(int current)
        {
            var next = current + 1;
            return next >= povs.Length ? 0 : next;
        }
    }
}