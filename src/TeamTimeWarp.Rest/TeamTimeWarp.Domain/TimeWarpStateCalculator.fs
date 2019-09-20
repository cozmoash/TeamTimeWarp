namespace TeamPomodoro.Domain

open System
open System.Collections.Generic
open TeamTimeWarp.Domain.Entities

  
type TeamMember = string
      
type EmailAddress = string

type TimeWarpStateCalculator(pomodoroTimeProvider) = 

    let getNextPomodoroState = 
        function
        | TimeWarpState.Working -> TimeWarpState.Resting
        | TimeWarpState.Resting -> TimeWarpState.None
        | TimeWarpState.None -> TimeWarpState.None
        | _ -> TimeWarpState.None

    let rec calculateProgress( timeWarpUserState : TimeWarpUserState, time) = 


        let account = timeWarpUserState.Account
        let currentPomodoroState = timeWarpUserState.State
        let periodTimeSpan = pomodoroTimeProvider currentPomodoroState
        let periodStartTime : DateTime = timeWarpUserState.PeriodStartTime
        let periodFinishTime = periodStartTime.Add periodTimeSpan
        let now : DateTime = time
        let periodTimeLeft = periodFinishTime.Subtract(now)
        let agentType = timeWarpUserState.AgentType

        match (timeWarpUserState.State, periodTimeLeft) with
        | (TimeWarpState.None,_) -> new TimeWarpUserState(account,currentPomodoroState,periodStartTime, TimeSpan.Zero,0.,agentType)//{ teamMemberState with Progress = 0.; TimeLeft = TimeSpan.Zero }
        | (_, t) when t >= TimeSpan.Zero -> let progress = 1. - (periodTimeLeft.TotalMilliseconds / periodTimeSpan.TotalMilliseconds)
                                            new TimeWarpUserState(account,currentPomodoroState,periodStartTime,periodTimeLeft,progress,agentType)
                                            
        | _ ->  let newPomodoroState : TimeWarpState = getNextPomodoroState(currentPomodoroState)
                let newPeriodStart = periodFinishTime
                let newTeamMemberState = new TimeWarpUserState(account,newPomodoroState,newPeriodStart,periodTimeLeft,timeWarpUserState.Progress,agentType)
                calculateProgress(newTeamMemberState, time)

    interface ITimeWarpStateCalculator with 
        member this.RecalculateTimeWarpState(currentState, time) = 
            calculateProgress (currentState, time)

