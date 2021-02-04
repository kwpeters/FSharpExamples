open System
open System.IO

type FileReader = string -> Result<string seq,exn>

type Customer = {
    CustomerId : string
    Email : string
    IsEligible : string
    IsRegistered : string
    DateRegistered : string
    Discount : string
}

let parseLine (line:string) : Customer option =
    match line.Split('|') with
    | [| customerId; email; eligible; registered; dateRegistered; discount |] ->
        Some {
            CustomerId = customerId
            Email = email
            IsEligible = eligible
            IsRegistered = registered
            DateRegistered = dateRegistered
            Discount = discount
        }
    | _ -> None

let parse (data:string seq) =
    data
    |> Seq.skip 1 // Ignore the header row
    |> Seq.map parseLine
    |> Seq.choose id // Ignore None and unwrap Some


let output data =
    data
    |> Seq.iter (fun x -> printfn "%A" x)


let readFile : FileReader =
    fun path ->
        try
            seq { use reader = new StreamReader(File.OpenRead(path))
                  while not reader.EndOfStream do
                      yield reader.ReadLine() }
            |> Ok
        with
        | ex -> Error ex


let import (fileReader: FileReader) path =
    match path |> fileReader with
    | Ok data -> data |> parse |> output
    | Error ex -> printfn "Error: %A" ex.Message


[<EntryPoint>]
let main argv =
    let inputFilePath = @"C:\Users\kwpeters.RA-INT\dev\kwp\FSharpExamples\part6\customers.csv"
    import readFile @"D:\temp\customers.csv"
    0
