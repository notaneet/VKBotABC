using System.Collections.Generic;

namespace VKBotABC.Utils
{
    public class Assoc<TK, TV>: Dictionary<TK, TV>
    {
        public bool Contains(TK key)
        {
            return TryGetValue(key, out _);
        }

        public TV GetOr(TK key, TV defaultValue)
        {
            return TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}