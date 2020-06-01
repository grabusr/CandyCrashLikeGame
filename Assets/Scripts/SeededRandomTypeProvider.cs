using System;
using System.Collections.Generic;

using core;

namespace LocalModel
{
    class SeededRandomTypeProvider : IBlockDataProvider
    {
        private Random randomDevice;
        private BlockData[] typesPool;

        public SeededRandomTypeProvider(int seed, List<BlockData> typesPool)
        {
            randomDevice = new Random(seed);
            this.typesPool = typesPool.ToArray();
        }

        public BlockData GetBlockData()
        {
            var randomIndex = randomDevice.Next(0, typesPool.Length);
            return typesPool[randomIndex];
        }

        public BlockData GetBlockDataFromPool(List<BlockData> typesPool)
        {
            var randomIndex = randomDevice.Next(0, typesPool.Count);
            return typesPool[randomIndex];
        }
    }
}
