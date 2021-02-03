module EntityTests

open System
open Xunit
open Swensen.Unquote


//------------------------------------------------------------------------------


type ContactId =
    | ContactId of string


type PhoneNumber =
    | PhoneNumber of string


[<NoEquality; NoComparison>]
type Contact =
    { ContactId: ContactId
      PhoneNumber: PhoneNumber }


[<Fact>]
let ``Entity equality`` () =
    let contact1: Contact =
        { ContactId = ContactId "001"
          PhoneNumber = PhoneNumber "123" }

    let contact2: Contact =
        { ContactId = ContactId "002"
          PhoneNumber = PhoneNumber "456" }

    // if contact1 = contact2 then         // Error: The type Contact does not support equality.
    //     printfn "Cannot compare the entities directly."

    // The ID fields must be compared explicitly.
    test <@ contact2.ContactId <> contact1.ContactId @>


//------------------------------------------------------------------------------


[<NoEquality; NoComparison>]
type Contact2 =
    { ContactId1: string
      ContactId2: string
      PhoneNumber: PhoneNumber }
    with
    member this.Key =
        (this.ContactId1, this.ContactId2)


[<Fact>]
let ``Entity identity - multiple fields`` () =
    let contact1: Contact2 =
        { ContactId1 = "foo"
          ContactId2 = "bar"
          PhoneNumber = PhoneNumber "123" }

    let contact2: Contact2 =
        { ContactId1 = "foo"
          ContactId2 = "bar"
          PhoneNumber = PhoneNumber "456" }

    test <@ contact1.Key = contact2.Key @>
