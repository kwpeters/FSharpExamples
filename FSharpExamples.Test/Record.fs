module RecordTests

open System
open Xunit

open Swensen.Unquote

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
    test <@ firstName = "Fred" @>


//------------------------------------------------------------------------------

[<Fact>]
let ``Copying records`` () =
    let person1 =
        { FirstName = "Fred"
          MiddleName = Some "Willard"
          LastName = "Flintstone" }

    let person2 = {person1 with MiddleName = None}
    test <@ person2.FirstName = "Fred" @>
    test <@ person2.MiddleName = None @>
    test <@ person2.LastName = "Flintstone" @>
