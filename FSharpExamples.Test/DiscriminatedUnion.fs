module DiscriminatedUnionTests

open System
open Xunit
open Swensen.Unquote

//------------------------------------------------------------------------------


type AppleVariety =
    | GoldenDelicious
    | GrannySmith
    | Fuji


[<Fact>]
let ``Discriminated union can be used like an enumeration`` () =

    let appleTypeToStr theApple =
        match theApple with
        | GoldenDelicious -> "Golden Delicious"
        | GrannySmith -> "Granny Smit"
        | Fuji -> "Fuji"

    let myApple = GoldenDelicious
    test <@ (appleTypeToStr myApple) = "Golden Delicious" @>


//------------------------------------------------------------------------------


type ProductCode = ProductCode of string


[<Fact>]
let ``Discriminated union as a simple type wrapper`` () =
    // Value creation
    let aProductCode = ProductCode "abc"

    // Deconstructing
    let (ProductCode primitiveStr) = aProductCode
    test <@ primitiveStr = "abc" @>


//------------------------------------------------------------------------------


[<Fact>]
let ``Unwrapping a discriminated union value in a parameter`` () =
    // A function that unwrapps the simple type in the parameter list.
    let unwrapProductCode (ProductCode innerProductCodeStr) =
        innerProductCodeStr

    let aProductCode = ProductCode "abc"
    test <@ (unwrapProductCode aProductCode) = "abc" @>


//------------------------------------------------------------------------------


type Shape =
    | Square of float // 1 associated value
    | Rectangle of float * float // 2 associated values
    | Circle of float // 1 associated value


[<Fact>]
let ``A discriminated union with associated data`` () =

    let shapeToName aShape =
        match aShape with
        | Square _ -> "square"
        | Rectangle _ -> "rectangle"
        | Circle _ -> "circle"

    let shapeArea aShape =
        match aShape with
        | Square side -> side * side
        | Rectangle (width, height) -> width * height
        | Circle radius -> 3.14159 * radius * radius

    let aShape = Rectangle(3.0, 4.0)
    test <@ (shapeToName aShape) = "rectangle" @>
    test <@ (shapeArea aShape) = 12.0 @>


//------------------------------------------------------------------------------


type UnitQuantity = private UnitQuantity of int

module UnitQuantity =
  let create qty =
    if qty < 1 then
      Error "UnitQuantity can not be negative"
    else if qty > 1000 then
      Error "UnitQuantity can not be more than 1000"
    else
      Ok (UnitQuantity qty)

  let value (UnitQuantity qty) = qty


[<Fact>]
let ``A discriminated union with a smart constructor`` () =
    let unitQtyResult1 = UnitQuantity.create -1
    test <@ unitQtyResult1 = Error "UnitQuantity can not be negative" @>

    let unitQtyResult2 = UnitQuantity.create 22
    match unitQtyResult2 with
    | Ok (UnitQuantity qty) -> test <@ qty = 22 @>
    | Error _ -> Assert.True(false)


//------------------------------------------------------------------------------


open System.Text.RegularExpressions

type WidgetProductCode = private WidgetProductCode of string

module WidgetProductCode =

    let create inputStr =
        let validatingRegex = Regex(@"W\d+");
        if validatingRegex.IsMatch inputStr then
            Ok (WidgetProductCode inputStr)
        else
            Error (sprintf "%s is not a valid WidgetProductCode." inputStr)


    let value (WidgetProductCode innerData) = innerData


[<Fact>]
let ``A discriminated union with a regex smart constructor`` () =
    let wpc1Result = WidgetProductCode.create "W123"
    match wpc1Result with
    | Ok theProductCode -> test <@ (WidgetProductCode.value theProductCode) = "W123" @>
    | Error _ -> Assert.True(false)

    let wpc2Result = WidgetProductCode.create "X123"
    match wpc2Result with
    | Ok _ -> Assert.True(false)
    | Error errMsg -> test <@ errMsg = "X123 is not a valid WidgetProductCode." @>
