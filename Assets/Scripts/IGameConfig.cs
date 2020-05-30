using System.Collections.Generic;

namespace LocalModel
{
    public interface IGameConfig
    {
        int BoardWidth { get; }
        int BoardHeight { get; }
        int[] TypesPool { get; }
        int Seed { get; }
    }
}
