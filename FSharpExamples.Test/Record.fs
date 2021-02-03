module RecordTests

open System
open Xunit

//------------------------------------------------------------------------------


type Person =
    { FirstName: string
      MiddleName: string option
      LastName: string }


[<Fact>]
let ``A basic record type example`` () =
    // Value creation
    let fred =
        { FirstName = "Fred"
          MiddleName = Some "Willard"
          LastName = "Flitstone" }

    // Deconstructing
    let { FirstName = firstName } = fred
    Assert.Equal("Fred", firstName)

