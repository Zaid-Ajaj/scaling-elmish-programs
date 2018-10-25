module Counter 

open Elmish 
open Fable.Helpers.React.Props
open Fable.Helpers.React

type State = { Count : int }

type Msg = Incr | Decr 

let init() = { Count = 0 }

let update msg state = 
    match msg with 
    | Incr -> { state with Count = state.Count + 1 }, Cmd.none 
    | Decr -> { state with Count = state.Count - 1 }, Cmd.none

let view state dispatch = 
    div [  ] [
        makeButton (fun _ -> dispatch Incr) "Increment"
        makeButton (fun _ -> dispatch Decr) "Decrement"

        br [ ] 
        h1 [ Style [ MarginLeft 150 ] ] [ str (sprintf "%d" state.Count) ]
    ]      