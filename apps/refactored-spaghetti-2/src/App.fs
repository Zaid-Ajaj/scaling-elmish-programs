module App

open Elmish 
open Elmish.React
open Fable.Helpers.React 
open Fable.Helpers.React.Props

type Page = 
    | Counter of Counter.State
    | Loader of Loader.State

type State = CurrentPage of Page 

type Url = Url of string 

type Message = 
    | CounterMsg of Counter.Msg
    | LoaderMsg of Loader.Msg
    | NavigateTo of Url

let init() = 
    let initialCounterState, initialCounterCmd = Counter.init()
    let initialState = CurrentPage (Page.Counter initialCounterState)
    let initialCmd = Cmd.map CounterMsg initialCounterCmd
    initialState, initialCmd
    
let update msg state = 
    match state, msg with
    | CurrentPage (Page.Counter counterState), CounterMsg counterMsg ->
        let nextCounterState, nextCounterCmd = Counter.update counterMsg counterState 
        let nextState = CurrentPage (Page.Counter nextCounterState)
        nextState, Cmd.map CounterMsg nextCounterCmd

    | CurrentPage (Page.Loader loaderState), LoaderMsg loaderMsg  ->
        let nextLoaderState, nextLoadecCmd = Loader.update loaderMsg loaderState
        let nextState = CurrentPage (Page.Loader nextLoaderState)
        nextState, Cmd.map LoaderMsg nextLoadecCmd 
 
    | _, NavigateTo (Url "/counter") ->
        let initialCounterState, initialCounterCmd = Counter.init()
        let nextState = CurrentPage (Page.Counter initialCounterState)
        let nextCmd = Cmd.map CounterMsg initialCounterCmd
        nextState, nextCmd

    | _, NavigateTo (Url "/loader") ->
        let initialLoaderState, initialLoaderCmd = Loader.init()
        let nextState = CurrentPage (Page.Loader initialLoaderState)
        let nextCmd = Cmd.map LoaderMsg initialLoaderCmd
        nextState, nextCmd

    | _ -> state, Cmd.none

let divider = 
    div [ Style [ MarginTop 20; MarginBottom 20 ] ] [ ]


let render state dispatch = 

    let currentUrl = 
        match state with 
        | CurrentPage (Page.Counter _ ) -> "/counter"
        | CurrentPage (Page.Loader _) -> "/loader"

    let navItem nextUrl title = 
        let notActive = currentUrl <> nextUrl
        let navLinkClass = if notActive then "nav-link" else "nav-link active"
        li [ ClassName "nav-item" ] [
            a [ ClassName navLinkClass
                Href "#"
                OnClick (fun _ -> dispatch (NavigateTo (Url nextUrl))) ] 
              [ str title ]
        ]

    let currentPage = 
        match state with 
        | CurrentPage (Page.Counter counterState) -> 
            Counter.view counterState (CounterMsg >> dispatch)
        | CurrentPage (Page.Loader loaderState) ->
            Loader.view loaderState (LoaderMsg >> dispatch)  

    div [ Style [ Padding 20 ] ] [ 
        h1 [ ] [ str "Refactored Spaghetti 2.0" ]
        divider

        ul [ ClassName "nav nav-tabs" ] [
            navItem "/counter" "Counter"
            navItem "/loader" "Loader"
        ]

        divider
        currentPage
    ]


Program.mkProgram init update render 
|> Program.withReact "root"
|> Program.run  