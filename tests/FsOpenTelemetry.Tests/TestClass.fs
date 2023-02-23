namespace SomeTestClasses

open System.Diagnostics
open FsOpenTelemetry
open FsOpenTelemetry.Tests

type TestClass() =
    let typ = typeof<TestClass>

    // let rec activity () =
    //     Tracing.activitySource.StartActivityForQuote(<@ activity @>)

    member x.GetForName() =
        Tracing.activitySource.StartActivityExt("MyCustomName")

    member x.GetForNameCustomNamespace() =
        let customNamespace = "MyNamespace"
        Tracing.activitySource.StartActivityExt("MyCustomName", name_space = customNamespace)

    member x.GetForType() =
        Tracing.activitySource.StartActivityForType(typ)

    member x.GetForTypeArg() =
        Tracing.activitySource.StartActivityForType<TestClass>()

    // don't
    member x.GetForQuote() : Activity = null

    member x.GetForFunc() =
        Tracing.activitySource.StartActivityForFunc()

    member x.GetForFuncAsync() = async { return Tracing.activitySource.StartActivityForFunc() }

module TestModule =
    type Marker =
        class
        end

    let typ = typeof<Marker>

    // let inline activity () =
    //     Tracing.activitySource.StartActivityForQuote(<@ activity @>)

    let getForName () =
        Tracing.activitySource.StartActivityExt("MyCustomName")

    let getForNameCustomNamespace () =
        let customNamespace = "MyNamespace"
        Tracing.activitySource.StartActivityExt("MyCustomName", name_space = customNamespace)

    let getForType () =
        Tracing.activitySource.StartActivityForType(typ)

    let getForTypeArg () =
        Tracing.activitySource.StartActivityForType<Marker>()

    // // don't
    let getForQuote () : Activity = null
    //  activity ()

    let getForFunc () =
        Tracing.activitySource.StartActivityForFunc()


    let getForFuncAsync () = async { return Tracing.activitySource.StartActivityForFunc() }
