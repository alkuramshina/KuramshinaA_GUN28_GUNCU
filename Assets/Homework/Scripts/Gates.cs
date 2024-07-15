using System;
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
            
            Destroy(ball);
        }

        private void DisplayScore() => Console.WriteLine($"Счет: {_score}");
    }
}