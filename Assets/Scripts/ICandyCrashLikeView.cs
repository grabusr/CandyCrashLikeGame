namespace core
{

    public interface ICandyCrashLikeView
    {
        void Init(IGameModelInfo gameModelInfo);
        void OnMoveEvent(MoveResult moveResult);
    }

} // namespace core
