- title : Scaling Elmish Applications
- description : Introduction to the techniques of building largs elmish applications
- author : Zaid Ajaj
- theme : night
- transition : default

***

## Scaling Elmish Applications

<br />
<br />

### Introduction to the techniques of breaking down large elmish programs into smaller ones
<br />
<br />
Zaid Ajaj - [@zaid-ajaj](http://www.twitter.com/zaid-ajaj)

***

### Table of content

* Introduction
* Basics of Elmish 
* Concept of an "Elm program"
* Importance of commands
* Larger programs 
* Challenges of breaking down programs
   - Breaking down parent state  
       - Keeping state of a single child program
       - Keeping state of all children programs
   - Child programs (updates, dispatch and commands)
* Application concerns: Root level vs. deep children
* Ongoing developements: the good and the lacking
* Resources to learn more
  
***

### Introduction

- About me
- The talk

***


### Basics of Elmish

    type State = { Count: int }

    type Msg = Increment | Decrement

    let init() : State = { Count = 0 } 

    let update msg state = 
        match msg with 
        | Increment -> { state with Count = state.Count + 1 }
        | Decrement -> { state with Count = state.Count - 1 }

    let view state dispatch = 
        div [ ] [ 
            button [ OnClick (fun _ -> dispatch Increment) ]
                   [ str "Increment" ]  
            button [ OnClick (fun _ -> dispatch Decrement) ]
                   [ str "Decrement" ]
            h1 [ ] 
               [ str (sprintf "%d" state.Count) ] 
        ]


***

![counter](images/counter1.0.gif)

***

***

### Introducing Commands

- Async messages schedulers

***

***
    type Message = 
        | Increment 
        | Decrement 
        | IncrementImmediate
        | IncrementDelayed 

    // update : Msg -> State -> State * Cmd<Msg>
    let update msg state  = 
        match msg with 
        | Increment -> 
            let nextState = { state with Count = state.Count + 1 }
            nextState, Cmd.none 

        | Decrement ->
            let nextState = { state with Count = state.Count - 1 }
            nextState, Cmd.none

        | IncrementImmediate ->
            let nextCmd = Cmd.ofMsg Increment 
            state, nextCmd

        | IncrementDelayed -> 
            let nextCmd = Cmd.afterTimeout 1000 Increment 
            state, nextCmd

***

![counter](images/counter2.0.gif)

***

***
    type State = 
        | Initial
        | Loading 
        | LoadedData of message:string 
 
    type Message = 
        | StartLoading
        | ReceivedLoadedData of message:string
        | Reset

    let update msg state = 
        match msg with 
        | StartLoading -> 
            let nextState = State.Loading
            nextState, Cmd.afterTimeout 1000 (ReceivedLoadedData "Your data is here")

        | ReceivedLoadedData receivedMessage ->
            let nextState = State.LoadedData receivedMessage
            nextState, Cmd.none

        | Reset -> 
            let nextState = State.Initial
            nextState, Cmd.none

***

***
    let view state dispatch = 
        match state with 
        | State.Initial -> 
            makeButton (fun _ -> dispatch StartLoading) "Start Loading"
        
        | State.Loading -> 
            spinner 
        
        | State.LoadedData message -> 
            div [ ] 
                [ showMessage message
                  makeButton (fun _ -> dispatch Reset) "Reset" ] 
***