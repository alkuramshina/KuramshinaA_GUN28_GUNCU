using TMPro;
using UnityEngine;

namespace Board
{
    public class RoundCounter: MonoBehaviour
    {
        private int _round;
        
        [SerializeField] private TMP_Text roundCounter;
        [SerializeField] private TMP_Text moveCounter;

        private void UpdateRound()
        {
            _round++;
            roundCounter.text = $"Round {_round}";
        }

        public void UpdateTurn(ColorType nextPlayer)
        {
            UpdateRound();
            
            moveCounter.text = $@"Ход {(nextPlayer switch
            { ColorType.White => "белых",
                ColorType.Black => "черных",
                _ => "кого-то" })}";
        }
    }
}