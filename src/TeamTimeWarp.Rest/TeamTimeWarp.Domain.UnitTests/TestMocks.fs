module TestMocks

open System
open System.Collections.Generic
open TeamPomodoro.Domain
open TeamTimeWarp.Domain.Entities
open TeamTimeWarp.Domain.Entities.Repositories

let mockPomodoroTimeProvider (pomodoroState : TimeWarpState) = 
    match pomodoroState with
    | TimeWarpState.Resting -> TimeSpan.FromMilliseconds 100.
    | TimeWarpState.Working -> TimeSpan.FromMilliseconds 400.
    | _ -> TimeSpan.Zero



type mockUserStateRepository(account, userState) = 

    let items = new Dictionary<int64,TimeWarpUserState>()
     
    interface ITimeWarpUserStateRepository with 
    //this is crap.
        member this.GetAll() = 
            new ResizeArray<TimeWarpUserState>(items.Values) :> IList<TimeWarpUserState>

        member this.GetLatestStateByAccountId(accountId) = 
            new ResizeArray<TimeWarpUserState>(items.Values) |> Seq.toList |> List.head
            
           
  
        member this.Add(item) = 
            items.Add(item.Id, item)