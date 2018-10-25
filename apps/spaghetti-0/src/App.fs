module App

open Elmish 
open Elmish.React
open Fable.Helpers.React 
open Fable.Helpers.React.Props

type Page = 
    | Counter 
    | Loader 

type LoaderState = 
    | Initial 
    | Loading
    | Loaded of string 

type State = {
    CurrentPage : Page 
    Count : int
    Loader : LoaderState 
}

type Message = 
    | Increment
    | Decrement
    | IncrementDelayed
    | StartLoading 
    | FinisihedLoading of string 
    | ResetLoader 
    | NavigateTo of Page


let init() =     
    {  Count = 0
       Loader = LoaderState.Initial 
       CurrentPage = Counter }, Cmd.none

let update msg state = 
    match msg with
    | Increment ->
        let nextState = { state with Count = state.Count + 1 }
        nextState, Cmd.none 

    | Decrement ->
        let nextState = { state with Count = state.Count - 1 }
        nextState, Cmd.none 

    | IncrementDelayed ->
        let nextCmd = Cmd.afterTimeout 1000 Increment 
        state, nextCmd 

    | StartLoading -> 
        let nextState = { state with Loader = LoaderState.Loading }
        let nextCmd = Cmd.afterTimeout 2000 (FinisihedLoading "Remote data") 
        nextState, nextCmd 
    
    | FinisihedLoading data -> 
        let nextState = { state with Loader = LoaderState.Loaded data }
        nextState, Cmd.none 

    | ResetLoader -> 
        let nextState = { state with Loader = LoaderState.Initial }
        nextState, Cmd.none 

    | NavigateTo page ->
        let nextState = { state with CurrentPage = page }
        nextState, Cmd.none

let divider = 
    div [ Style [ MarginTop 20; MarginBottom 20 ] ] [ ]


let spinner = 
    div [ Style [ Padding 20 ] ] [ 
        i [ ClassName "fa fa-circle-notch fa-spin fa-2x" ] [ ]
    ]

let render state dispatch = 

    let navItem nextPage title = 
        let notActive = state.CurrentPage <> nextPage
        let navLinkClass = if notActive then "nav-link" else "nav-link active"
        li [ ClassName "nav-item" ] [
            a [ ClassName navLinkClass
                Href "#"
                OnClick (fun _ -> dispatch (NavigateTo nextPage)) ] 
              [ str title ]
        ]

    let currentPage = 
        match state.CurrentPage with 
        | Page.Counter -> 
            div [ ] [ 
                makeButton (fun _ -> dispatch Increment) "Increment"
                makeButton (fun _ -> dispatch Decrement) "Decrement"
                makeButton (fun _ -> dispatch IncrementDelayed) "Increment Delayed"
                h3 [ ] [ str (sprintf "Count is at %d" state.Count) ]
            ]

        | Page.Loader ->
            match state.Loader with 
            | Initial ->  makeButton (fun _ -> dispatch StartLoading) "Start Loading"
            | Loading -> spinner
            | Loaded data -> 
                div [ ] [ 
                    h3 [ ] [ str data ]
                    makeButton (fun _ -> dispatch ResetLoader) "Reset"
                ]

    div [ Style [ Padding 20 ] ] [ 
        h1 [ ] [ str "Spaghetti" ]
        divider

        ul [ ClassName "nav nav-tabs" ] [
            navItem Page.Counter "Counter"
            navItem Page.Loader "Loader"
        ]

        divider
        currentPage
    ]


Program.mkProgram init update render 
|> Program.withReact "root"
|> Program.run  