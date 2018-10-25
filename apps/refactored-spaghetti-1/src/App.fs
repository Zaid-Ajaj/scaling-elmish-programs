module App

open Elmish 
open Elmish.React
open Fable.Helpers.React 
open Fable.Helpers.React.Props

type Page = 
    | Counter 
    | Loader 

type State = {
    CurrentPage : Page 
    Counter : Counter.State 
    Loader : Loader.State 
}

type Message = 
    | CounterMsg of Counter.Msg
    | LoaderMsg of Loader.Msg
    | NavigateTo of Page


let init() = 
    let initialCounter = Counter.init()
    let initialLoader, _ = Loader.init()
    
    {
        Counter = initialCounter
        Loader = initialLoader 
        CurrentPage = Counter
    },

    Cmd.none

let update msg prevState = 
    match msg with
    | CounterMsg counterMsg ->
        let nextCounterState, nextCounterCmd = Counter.update counterMsg prevState.Counter 
        let appState = { prevState with Counter = nextCounterState }
        appState, Cmd.map CounterMsg nextCounterCmd

    | LoaderMsg loaderMsg ->
        let nextLoaderState, nextLoadecCmd = Loader.update loaderMsg prevState.Loader
        let appState= { prevState with Loader = nextLoaderState }
        appState, Cmd.map LoaderMsg nextLoadecCmd 
 
    | NavigateTo page ->
        let nextState = { prevState with CurrentPage = page }
        nextState, Cmd.none

let divider = 
    div [ Style [ MarginTop 20; MarginBottom 20 ] ] [ ]


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
            Counter.view state.Counter (CounterMsg >> dispatch)
        | Page.Loader ->
            Loader.view state.Loader (LoaderMsg >> dispatch)  

    div [ Style [ Padding 20 ] ] [ 
        h1 [ ] [ str "Refactored Spaghetti" ]
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