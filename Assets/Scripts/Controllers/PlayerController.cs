using Board;
using Zenject;

namespace Controllers
{
    public class PlayerController
    {
        private readonly CameraMover _cameraMover;
        private readonly RoundCounter _roundCounter;
        
        public void FirstMove(ColorType nextPlayer)
        {
            _roundCounter.UpdateMove(nextPlayer);
        }
        
        public void NextMove(ColorType nextPlayer)
        {
            _roundCounter.UpdateMove(nextPlayer);
            _cameraMover.MoveToNextPov();
        }

        [Inject]
        private PlayerController(CameraMover cameraMover,
            RoundCounter roundCounter)
        {
            _cameraMover = cameraMover;
            _roundCounter = roundCounter;
        }
    }
}