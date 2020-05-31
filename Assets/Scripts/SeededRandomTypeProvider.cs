using System;
using System.Collections.Generic;

using core;

namespace LocalModel
{
    class SeededRandomTypeProvider : IRandomTypeProvider
    {
        private Random randomDevice;

        public SeededRandomTypeProvider(int seed)
        {
            randomDevice = new Random(seed);
        }

        public BlockData GetRandomElementType(List<BlockData> typesPool)
        {
            var randomIndex = randomDevice.Next(0, typesPool.Count);
            return typesPool[randomIndex];
        }
    }
}
