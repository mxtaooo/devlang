module Crowbar.Compiler

open System
open System.IO

module Tokenizer =
    // let reader = new StreamReader("")

    type CharReader(reader: TextReader)  = 
        let mutable marking = false
        let mutable chars = List.empty
        member _.Mark() = marking <- true
        member _.UnMark() = marking <- false; chars <- List.empty
        member _.Read() = 
            match (marking, chars) with
            | (false, []) -> reader.Read() |> char
            | (false, head::tail) -> chars <- tail; head
            | (true, _) -> chars <- chars @ [reader.Read() |> char]; List.last chars
        member _.Peek() = reader.Peek() |> char
        member _.Close() = if isNull reader then reader.Close()
        interface IDisposable with
            member _.Dispose() =
                if isNull reader then reader.Dispose()
