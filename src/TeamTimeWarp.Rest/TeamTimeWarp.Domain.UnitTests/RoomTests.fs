//module ``Given team members have started working``
//
//open System
//open TeamPomodoro.Domain
//open NUnit.Framework
//open FsUnit
//open TeamTimeWarp.Domain.Entities
//
//let mocktimes = TestMocks.mockPomodoroTimeProvider
//let stateCalculator = new TimeWarpStateCalculator(mocktimes)
//
//let userAccount1 = new TimeWarpAccount(int64 1,"ash","ashley@teamtimewarp.com","password")
//let timeWarpUser1 = new TimeWarpUser(userAccount1,stateCalculator)
//
//let userAccount2 = new TimeWarpAccount(int64 2,"bean","ashley@teamtimewarp.com","password")
//let timeWarpUser2 = new TimeWarpUser(userAccount2,stateCalculator)
//
//let room = new Room(1,"test room",DateTime.Now)
//
//let startTime = new DateTime(2000,12,12,12,12,12)
//do timeWarpUser1.StartWork startTime |> ignore
//   timeWarpUser2.StartRest startTime |> ignore
//   room.Add timeWarpUser1
//   room.Add timeWarpUser2
//
//let requestTime = new DateTime(2000,12,12,12,12,12,200)
//let teamMemberStates = room.GetAll requestTime
//
//let ash = teamMemberStates.[0] //|> Seq.find(fun x -> x.Account.Id.Equals(1))
//let bean = teamMemberStates.[1]// |> Seq.find(fun x -> x.Account.Id.Equals(2))
//
//[<TestFixture>] 
//type ``When I get the current state of all team members in a room`` ()=
//    
//    [<Test>]
//    member test.``Then the progress of the users pomodoro are reported`` ()=
//        ash.Progress |> should equal 0.5
//        bean.Progress |> should equal 0
//
//    [<Test>]
//    member test.``Then the user is set to Working state`` ()=
//        ash.State |> should equal TimeWarpState.Working
//        bean.State |> should equal TimeWarpState.None
//
//    [<Test>]
//    member test.``Then the state period start time is set`` ()=
//        let expected1 = new DateTime(2000,12,12,12,12,12)
//        let expected2 = new DateTime(2000,12,12,12,12,12,100)
//        ash.PeriodStartTime |> should equal expected1
//        bean.PeriodStartTime |> should equal expected2
//
//        
