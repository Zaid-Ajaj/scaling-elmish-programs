[<AutoOpen>]
module Extensions

open Fable.Import.Browser
open Fable.Helpers.React.Props
open Fable.Helpers.React

open Elmish 

module Cmd = 
    let afterTimeout n (msg: 't) : Cmd<'t> = 
        [ fun dispatch ->
            async {
                do! Async.Sleep n 
                do (dispatch msg)
            } 
            |> Async.StartImmediate
        ] 

let makeButton f title = 
    button [ Style [ Margin 10 ]
             ClassName "btn btn-info"; 
             OnClick f ] 
           [ str title ]