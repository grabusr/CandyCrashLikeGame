namespace QuickTurnStudio.CandyCrashLike.Core
{
    public interface IGameView
    {
        void Init(IGameModelInfo gameModelInfo);
        void OnMoveEvent(MoveResult[] moveResults);
    }
}
