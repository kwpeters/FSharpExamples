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


//------------------------------------------------------------------------------

[<Fact>]
let ``Copying records`` () =
    let person1 =
        { FirstName = "Fred"
          MiddleName = Some "Willard"
          LastName = "Flintstone" }

    let person2 = {person1 with MiddleName = None}
    Assert.Equal("Fred", person2.FirstName)
    Assert.Equal(None, person2.MiddleName)
    Assert.Equal("Flintstone", person2.LastName)
