using System;

namespace Chess.AI
{
    [Serializable]
    public class CacheFileEntry
    {
        public EvalCacheKey CacheKey { get; set; }
        public float Score { get; set; }
    }
}
