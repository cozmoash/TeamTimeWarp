module ``Given a team member has started working``

open System
open TeamPomodoro.Domain
open NUnit.Framework
open FsUnit
open TeamTimeWarp.Domain.Entities


let mocktimes = TestMocks.mockPomodoroTimeProvider


let userAccount = new Account(int64 1,"ash","ashley@teamtimewarp.com",AccountType.Full)
let stateCalculator = new TimeWarpStateCalculator(mocktimes)
let timeWarpState = TimeWarpUserState.DefaultState(userAccount)
let mockUserStateRepository = new TestMocks.mockUserStateRepository(userAccount,timeWarpState)
let timeWarpUser = new UserStateManager(stateCalculator,mockUserStateRepository)
let teamMember = "ash"
let startTime = new DateTime(2000,12,12,12,12,12)
do timeWarpUser.StartWork(userAccount, startTime,1) |> ignore

[<TestFixture>] 
type ``When I get the current state of the team member`` ()=    
    let requestTime = new DateTime(2000,12,12,12,12,12,200)
    let (success,teamMemberState) = timeWarpUser.TryGetCurrentState ( userAccount.Id, requestTime)

    [<Test>]
    member test.``Then the progress of the user's time warp is reported`` ()=
        teamMemberState.Progress |> should equal 0.5 

    [<Test>]
    member test.``Then the user is set to Working state`` ()=
        teamMemberState.State |> should equal TimeWarpState.Working

    [<Test>]
    member test.``Then the state period start time is set`` ()=
        let expectedStartTime = startTime
        teamMemberState.PeriodStartTime |> should equal expectedStartTime

    [<Test>]
    member test.``Then the time left is calculated`` ()=
        let expectedTimeLeft = TimeSpan.FromMilliseconds(200.)
        teamMemberState.TimeLeft |> should equal expectedTimeLeft

    [<Test>]
    member test.``Then the agent type is reported`` ()=
        teamMemberState.AgentType |> should equal 1

[<TestFixture>] 
type ``When I get the current state of the team member after the work phase is complete`` ()=
    let requestTime = new DateTime(2000,12,12,12,12,12,450)
    let (success,teamMemberState) = timeWarpUser.TryGetCurrentState ( userAccount.Id, requestTime)

    [<Test>]
    member test.``Then the progress of the users pomodoro is reported of the rest period`` ()=
        let expectedProgress = 0.5
        teamMemberState.Progress |> should equal expectedProgress

    [<Test>]
    member test.``Then the user is set to Resting state`` ()=
        let expectedState = TimeWarpState.Resting
        teamMemberState.State |> should equal expectedState

    [<Test>]
    member test.``Then the state period start time is set`` ()=
        let expectedStartTime = new DateTime(2000,12,12,12,12,12,400)
        teamMemberState.PeriodStartTime |> should equal expectedStartTime

    [<Test>]
    member test.``Then the time left is calculated`` ()=
        let expectedTimeLeft = TimeSpan.FromMilliseconds(50.)
        teamMemberState.TimeLeft |> should equal expectedTimeLeft


[<TestFixture>] 
type ``When I get the current state of the team member after the work and rest phase is complete`` ()=
    let requestTime = new DateTime(2000,12,12,12,12,12,550)
    let (success,teamMemberState) = timeWarpUser.TryGetCurrentState ( userAccount.Id, requestTime)

    [<Test>]
    member test.``Then the progress of the of the None pomodoro state is indeterminate`` ()=
        let expectedProgress = 0.
        teamMemberState.Progress |> should equal expectedProgress

    [<Test>]
    member test.``Then the user is set to None state`` ()=
        let expectedState = TimeWarpState.None
        teamMemberState.State |> should equal expectedState

    [<Test>]
    member test.``Then the state period start time is set`` ()=
        let expectedStartTime = new DateTime(2000,12,12,12,12,12,500)
        teamMemberState.PeriodStartTime |> should equal expectedStartTime

    [<Test>]
    member test.``Then the time left is set to indeterminate`` ()=
        teamMemberState.TimeLeft |> should equal TimeSpan.Zero
