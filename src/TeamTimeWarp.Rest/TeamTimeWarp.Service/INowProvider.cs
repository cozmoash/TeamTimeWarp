using System;

namespace TeamTimeWarp.Service
{
    public interface INowProvider
    {
        DateTime Now { get; }
    }

    public class NowProvider : INowProvider
    {
        public DateTime Now { get { return DateTime.UtcNow; } }
    }
}