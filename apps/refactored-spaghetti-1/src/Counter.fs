module Counter 

open Elmish 
open Fable.Helpers.React.Props
open Fable.Helpers.React

type State = { Count : int }

type Msg = 
    | Increment 
    | Decrement 
    | IncrementDelayed

let init() = { Count = 0 }, Cmd.none

let update msg state = 
    match msg with 
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none 
    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none
    | IncrementDelayed -> 
        let nextCmd = Cmd.afterTimeout 1000 Increment
        state, nextCmd

let view state dispatch = 
    div [  ] [
        makeButton (fun _ -> dispatch Increment) "Increment"
        makeButton (fun _ -> dispatch Decrement) "Decrement"
        makeButton (fun _ -> dispatch IncrementDelayed) "Increment Delayed"

        br [ ] 
        h1 [ Style [ MarginLeft 150 ] ] [ ofInt state.Count ]
    ]      