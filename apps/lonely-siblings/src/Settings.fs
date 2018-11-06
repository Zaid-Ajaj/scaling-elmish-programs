module Settings

open Elmish 
open Fable.Helpers.React.Props
open Fable.Helpers.React


type State = SelectedCountFactor of int

type Msg = 
    | ChangeFactor of int 

let init() = SelectedCountFactor 1, Cmd.none 

let update msg state =
    match msg with
    | ChangeFactor nextFactor ->
        let nextState = SelectedCountFactor nextFactor
        nextState, Cmd.none 


let view state dispatch = 
    let (SelectedCountFactor currentFactor) = state
    let factorBtn n = 
      let buttonClass = 
        if n = currentFactor 
        then "btn btn-success"
        else "btn btn-default"
      button [ OnClick (fun _ -> dispatch (ChangeFactor n))
               Style [ Margin 10 ]
               ClassName buttonClass ] 
             [ ofInt n ] 
    
    div [ ] [ 
        factorBtn 1
        factorBtn 5
        factorBtn 10
    ]

