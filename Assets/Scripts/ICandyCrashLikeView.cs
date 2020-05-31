namespace core
{

    public interface ICandyCrashLikeView
    {
        void Init(IGameModelInfo gameModelInfo);
        void OnMoveEvent(MoveResult[] moveResults);
    }

} // namespace core
