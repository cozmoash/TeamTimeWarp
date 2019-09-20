using System;

namespace TeamTimeWarp.Domain.Entities
{
    public interface ITimeWarpStateCalculator
    {
        TimeWarpUserState RecalculateTimeWarpState(TimeWarpUserState currentState, DateTime time);
    }
}