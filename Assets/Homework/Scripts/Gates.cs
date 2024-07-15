using UnityEngine;

namespace Netologia.Homework
{
    public class Gates : MonoBehaviour
    {
        private int _score = 0;
        
        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Ball>(out var ball)) 
                return;
            
            _score++;
            
            DisplayScore();
            Destroy(ball.gameObject);
        }

        private void DisplayScore() => Debug.Log($"Счет: {_score}");
    }
}