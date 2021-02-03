module OptionTests

open System
open Xunit
open Swensen.Unquote


//------------------------------------------------------------------------------


type Person =
    { FirstName: string
      MiddleName: string option
      LastName: string }


[<Fact>]
let ``A basic option example`` () =
    let fred: Person =
        { FirstName = "Fred"
          MiddleName = None
          LastName = "Flintstone" }

    let { MiddleName = middleName } = fred
    test <@ middleName.IsNone @>


    let middleName' =
        match middleName with
        | Some middleName -> middleName
        | None -> "<no middle name>"

    test <@ middleName' = "<no middle name>" @>

