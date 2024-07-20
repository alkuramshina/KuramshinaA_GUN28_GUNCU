using Board;

namespace Controllers.Commands
{
    public interface IGameplayCommand
    {
        void Interact(Cell cell);
    }
}