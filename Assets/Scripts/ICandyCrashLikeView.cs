namespace QuickTurnStudio.CandyCrashLike.Core
{
    public interface ICandyCrashLikeView
    {
        void Init(IGameModelInfo gameModelInfo);
        void OnMoveEvent(MoveResult[] moveResults);
    }
}
