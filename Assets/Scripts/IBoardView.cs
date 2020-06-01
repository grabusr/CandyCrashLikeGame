using UnityEngine;

namespace QuickTurnStudio.CandyCrashLike.UnityView
{
    public interface IBoardView
    {        
        void AnimationEnded(Element element);
        Vector3 GetPositionOfCoordinate(Core.Coordinate coordinate);
        bool IsAnimating();
    }
}
