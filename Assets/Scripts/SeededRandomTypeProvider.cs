﻿using System;
using System.Collections.Generic;

using QuickTurnStudio.CandyCrashLike.Core;

namespace QuickTurnStudio.CandyCrashLike.LocalModel
{
    class SeededRandomTypeProvider : IBlockDataProvider
    {
        private Random randomDevice;
        private BlockData[] typesPool;

        public SeededRandomTypeProvider(int seed, BlockData[] typesPool)
        {
            randomDevice = new Random(seed);
            this.typesPool = new BlockData[typesPool.Length];
            typesPool.CopyTo(this.typesPool, 0);
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
