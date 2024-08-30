using System.Collections.Generic;
using Board;
using Zenject;

namespace Controllers
{
    public class PlayerController
    {
        private readonly CameraMover _cameraMover;
        private readonly RoundCounter _roundCounter;

        private readonly Dictionary<ColorType, Player> _players;
        private Player _activePlayer;
        
        public Player Start(ColorType startingPlayerColor)
        {
            _activePlayer = _players[startingPlayerColor];
            _roundCounter.UpdateTurn(_activePlayer.Color);

            return _activePlayer;
        }
        
        public Player NextTurn(ColorType playerColor)
        {
            _activePlayer = _players[playerColor];
            
            _roundCounter.UpdateTurn(_activePlayer.Color);
            _cameraMover.MoveToNextPov();

            return _activePlayer;
        }

        public bool IsLocked => _cameraMover.CameraIsMoving;

        [Inject]
        private PlayerController(CameraMover cameraMover,
            RoundCounter roundCounter)
        {
            _cameraMover = cameraMover;
            _roundCounter = roundCounter;

            _players = new Dictionary<ColorType, Player>
            {
                { ColorType.White, new Player(ColorType.White) },
                { ColorType.Black, new Player(ColorType.Black) }
            };
        }
    }

    public struct Player
    {
        public ColorType Color { get; private set; }

        public Player(ColorType color)
        {
            Color = color;
        }
    }
}