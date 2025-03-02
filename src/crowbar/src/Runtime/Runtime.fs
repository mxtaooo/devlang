
/// <summary>Crowbar Runtime, accept abstract syntax tree.</summary>
namespace Crowbar.Runtime

open System
open Microsoft.FSharp.NativeInterop

#nowarn "9"

module private RtCommon =
    type RtType = 
        | Unit = 0
        | Int = 1
        | Double = 2
        | String = 3
        | Pointer = 4

    // |    8 byte    |  4 byte  |      |   4 byte   |   8 byte  |       |
    // | used mem len | name len | name | value type | value len | value |
    type ReadOnlyMemoryEntry(ptr: voidptr) =
        let len = NativePtr.ofVoidPtr<uint64> ptr |> NativePtr.read |> int
        let span = new ReadOnlySpan<byte>(ptr, len)
        let nameLen =  BitConverter.ToUInt32(span.Slice(8, 4)) |> int
        let name = System.Text.Encoding.ASCII.GetString (span.Slice(12, nameLen))
        let ``type`` = BitConverter.ToInt32(span.Slice(8+4+nameLen, 4))
        let valueLen = BitConverter.ToUInt64(span.Slice(8+4+nameLen+4, 8)) |> int
        let valueSpan = span.Slice(8+4+nameLen+4+8, valueLen)
        member _.Ptr = ptr
        member _.Name = name
        // todo: FIX HERE !!!
        member _.Value: obj = 
            match ``type`` with
            | 1 -> BitConverter.ToInt32(valueSpan) |> box
            | 2 -> BitConverter.ToDouble(valueSpan) |> box 
            | _ -> null


module private Memory =
    let malloc () = failwith "not implement"

module private Debug =
    // accept env condition msg
    let debug env exp msg = failwith "not implement"

module Runtime = 
    open Crowbar.Lang.AbstractSyntaxTree

    // just execute stmt, and modify the env
    let executeStmt env stmt = 
        match stmt with
        | _ -> ()

    // use env to eval expression to a value, without modify env generally
    let eval env exp = failwith "Not Implemented"


    // open System
    // open System.Runtime.InteropServices
    // open Microsoft.FSharp.NativeInterop

    // #nowarn "9" // disable warnings for using NativePtr module

    // // https://docs.microsoft.com/en-us/dotnet/fsharp/whats-new/fsharp-45

    // let operatingSpan (span: Span<byte>) = 
    //     let s = Span<byte>()
    //     span.CopyTo(s)
    //     // ....
    //     ()

    // let ``some operatings`` () = 
    //     let hglobal = Marshal.AllocHGlobal(1024)
    //     let np0 = hglobal.ToPointer() |> NativePtr.ofVoidPtr<byte>
    //     let np = hglobal |> NativePtr.ofNativeInt<byte>
    //     let arr = Array.zeroCreate<int>(10)
    //     let si = NativePtr.stackalloc<int> 1
    //     let sb = si |> NativePtr.toVoidPtr |> NativePtr.ofVoidPtr<byte>
    //     // must free memory here!
    //     Marshal.FreeHGlobal(hglobal)
    //     ()
