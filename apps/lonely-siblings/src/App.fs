module App

open Elmish 
open Elmish.React
open Fable.Helpers.React 
open Fable.Helpers.React.Props

type Page = 
    | Counter 
    | Loader 
    | Settings

type State = {
    CurrentPage : Page 
    Counter : Counter.State 
    Loader : Loader.State 
    Settings : Settings.State
}

type Message = 
    | CounterMsg of Counter.Msg
    | LoaderMsg of Loader.Msg
    | SettingMsg of Settings.Msg 
    | NavigateTo of Page

let init() = 
    let initCounter, counterCmd = Counter.init()
    let initLoader, loaderCmd = Loader.init()
    let initSettings, settingsCmd = Settings.init() 
    let initState = {
        Counter = initCounter
        Loader = initLoader 
        Settings = initSettings
        CurrentPage = Counter
    }

    let initCmd = Cmd.batch [
        Cmd.map CounterMsg counterCmd
        Cmd.map LoaderMsg loaderCmd
        Cmd.map SettingMsg settingsCmd
    ]

    initState, initCmd

let update msg prevState = 
    match msg with
    | CounterMsg counterMsg ->
        let nextCounterState, nextCounterCmd = Counter.update counterMsg prevState.Counter 
        let nextState = { prevState with Counter = nextCounterState }
        nextState, Cmd.map CounterMsg nextCounterCmd

    | LoaderMsg loaderMsg ->
        let nextLoaderState, nextLoadecCmd = Loader.update loaderMsg prevState.Loader
        let nextState = { prevState with Loader = nextLoaderState }
        nextState, Cmd.map LoaderMsg nextLoadecCmd 
 
    | SettingMsg settingMsg -> 
        // TODO, propagate messages
        let nextSettings, settingCmd = Settings.update settingMsg prevState.Settings 
        let nextState = { prevState with Settings = nextSettings }
        nextState, Cmd.map SettingMsg settingCmd

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
        | Page.Counter -> Counter.view state.Counter (CounterMsg >> dispatch)
        | Page.Loader -> Loader.view state.Loader (LoaderMsg >> dispatch)  
        | Page.Settings -> Settings.view state.Settings (SettingMsg >> dispatch)

    div [ Style [ Padding 20 ] ] [ 
        h1 [ ] [ str "Lonely Siblings :(" ]
        divider

        ul [ ClassName "nav nav-tabs" ] [
            navItem Page.Counter "Counter"
            navItem Page.Loader "Loader"
            navItem Page.Settings "Settings"
        ]

        divider
        currentPage
    ]


Program.mkProgram init update render 
|> Program.withReact "root"
|> Program.run  