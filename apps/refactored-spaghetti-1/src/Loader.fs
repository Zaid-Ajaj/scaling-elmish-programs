module Loader 

open Elmish 
open Fable.Helpers.React.Props 
open Fable.Helpers.React 


type State = 
    | Initial 
    | Loading 
    | Loaded of string 

type Msg = 
    | StartLoading
    | LoadedData  of string 
    | Reset 

let init() = State.Initial, Cmd.none

let update msg state = 
    match msg with 
    | StartLoading -> 
        let nextState = State.Loading 
        let nextCmd = Cmd.afterTimeout 1000 (LoadedData  "Data is loaded")
        nextState, nextCmd 

    | LoadedData data -> 
        let nextState = State.Loaded data 
        nextState, Cmd.none 
         
    | _ ->
        state, Cmd.none 

let spinner = 
    div [  ] [
        span [ ] [
            i [ ClassName "fa fa-circle-notch fa-spin fa-2x" ] [ ]
        ]
    ]



let view state dispatch = 
    match state with 
    | State.Initial ->
        makeButton (fun _ -> dispatch StartLoading) "Start Loading"
    | State.Loading ->
        spinner 
    | State.Loaded data ->
        div [ ] 
            [ h3 [ ] [ str data ]
              makeButton (fun _ -> dispatch Reset) "Reset" ] 