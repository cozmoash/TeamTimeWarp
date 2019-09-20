using System;

using System.ComponentModel;
using Microsoft.FSharp.Core;
using TeamPomodoro.Domain;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Rest.Tests.Controllers.Mocks
{
    public static class FakeTimeCalculatorFactory
    {
        public static TimeWarpStateCalculator GetTimeWarpStateCalculator()
        {
            Func<TimeWarpState, TimeSpan> csFunc = i =>
                {
                    switch (i)
                    {
                        case (TimeWarpState.None):
                            return TimeSpan.Zero;
                        case (TimeWarpState.Resting):
                            return TimeSpan.FromMilliseconds(100);
                        case (TimeWarpState.Working):
                            return TimeSpan.FromMilliseconds(200);
                        default:
                            throw new InvalidEnumArgumentException("i", (int)i, typeof(TimeWarpState));
                    }
                };


            var fsharpFunc = FSharpFunc<TimeWarpState, TimeSpan>.FromConverter(
                new Converter<TimeWarpState, TimeSpan>(csFunc));
            var timeWarpStateCalculator = new TimeWarpStateCalculator(fsharpFunc);
            return timeWarpStateCalculator;
        }
    }
}