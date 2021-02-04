namespace MyCodeTests

open Xunit
open Swensen.Unquote
open DemoCode.CustomerTypes
open DemoCode.Customer

[<AutoOpen>]
module TestHelpers =

    let failTest msg =
        Assert.True(false, msg)

    let passTest =
        Assert.True(true)

    let isCustomerAlreadyExistsException exn =
        match exn with
        | CustomerAlreadyExistsException -> passTest
        | ex -> failTest (sprintf "%A not expected" ex)


module ``Convert customer to eligible`` =

    let sourceCustomer = { CustomerId = "John"; IsRegistered = true; IsEligible = true }


    [<Fact>]
    let ``should succeed if not currently eligible`` () =
        let customer = { sourceCustomer with IsEligible = false }
        let upgraded = convertToEligible customer
        test <@ upgraded = sourceCustomer @>


    [<Fact>]
    let ``should return eligible customer unchanged`` () =
        let upgraded = convertToEligible sourceCustomer
        test <@ upgraded = sourceCustomer @>


module ``Create customer`` =

    let name = "John"


    [<Fact>]
    let ``should succeed if customer does not exist`` () =
        let existing = None
        let result = tryCreateCustomer name existing
        match result with
        | Ok customer -> test <@ customer = { CustomerId = name; IsRegistered = true; IsEligible = false } @>
        | Error ex -> failTest <| ex.ToString()


    [<Fact>]
    let ``should fail if customer does exist`` () =
        let existing = Some { CustomerId = name; IsRegistered = true; IsEligible = false }
        let result = tryCreateCustomer name existing
        match result with
        | Error ex -> isCustomerAlreadyExistsException ex
        | Ok customer -> failTest (sprintf "%A was not expected" customer)


//------------------------------------------------------------------------------


open DemoCode.Orders


module ``Order tests`` =


    [<Fact>]
    let ``addItem when adding a new item`` () =
        let myEmptyOrder = { Id = 1; Items = [] }
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let actual = myEmptyOrder |> addItem { ProductId = 1; Quantity = 1 }
        test <@ actual = expected @>


    [<Fact>]
    let ``addItem when adding an existing item`` () =
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 2 } ] }
        let actual = myOrder |> addItem { ProductId = 1; Quantity = 1 }
        test <@ actual = expected @>


    [<Fact>]
    let ``addItems when adding a new item`` () =
        let myEmptyOrder = { Id = 1; Items = [] }
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ] }
        let actual = myEmptyOrder |> addItems [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ]
        test <@ actual = expected @>


    [<Fact>]
    let ``addItems when adding an existing item`` () =
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 2 }; { ProductId = 2; Quantity = 5 } ] }
        let actual = myOrder |> addItems [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ]
        test <@ actual = expected @>


    [<Fact>]
    let ``removeItem called for existing item`` () =
        let myEmptyOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 1; Items = [] }
        let actual = myEmptyOrder |> removeItem 1
        test <@ actual = expected @>

    [<Fact>]
    let ``removeItem called for non-existant item`` () =
        let myOrder = { Id = 2; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 2; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let actual = myOrder |> removeItem 2
        test <@ actual = expected @>


    [<Fact>]
    let ``reduceItem - some existing`` () =
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 5 } ] }
        let expected = { Id = 1; Items = [ { ProductId = 1; Quantity = 2 } ] }
        let actual = myOrder |> reduceItem 1 3
        test <@ actual = expected @>


    [<Fact>]
    let ``reduceItem - all existing`` () =
        let myOrder = { Id = 2; Items = [ { ProductId = 1; Quantity = 5 } ] }
        let expected = { Id = 2; Items = [] }
        let actual = myOrder |> reduceItem 1 5
        actual = expected


    [<Fact>]
    let ``reduceItem - nonexistant productId`` () =
        let myOrder = { Id = 3; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 3; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let actual = myOrder |> reduceItem 2 5
        actual = expected


    [<Fact>]
    let ``reduceItem - empty order`` () =
        let myEmptyOrder = { Id = 4; Items = [] }
        let expected = { Id = 4; Items = [] }
        let actual = myEmptyOrder |> reduceItem 2 5
        actual = expected


    [<Fact>]
    let ``clearItems - items exist`` () =
        let myOrder = { Id = 1; Items = [ { ProductId = 1; Quantity = 1 } ] }
        let expected = { Id = 1; Items = [] }
        let actual = myOrder |> clearItems
        test <@ actual = expected @>


    [<Fact>]
    let ``clearItems - empty order`` () =
        let myEmptyOrder = { Id = 2; Items = [] }
        let expected = { Id = 2; Items = [] }
        let actual = myEmptyOrder |> clearItems
        test <@ actual = expected @>
