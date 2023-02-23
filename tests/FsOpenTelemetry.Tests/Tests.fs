namespace FsOpenTelemetry.Tests

open System
open Expecto
open FsOpenTelemetry

module Expect =
    open System.Diagnostics

    let expectDisplayName (activity: Activity) expected =
        Expect.equal activity.DisplayName expected "Incorrect DisplayName"

    let sourceCodeTags expectedNamespace expectedFilePath expectedFunction (activity: Activity) =
        let code_namespace =
            activity.GetTagItem SemanticConventions.General.SourceCode.code_namespace
            |> string

        let code_filepath =
            activity.GetTagItem SemanticConventions.General.SourceCode.code_filepath
            |> string

        let code_lineno =
            activity.GetTagItem SemanticConventions.General.SourceCode.code_lineno
            |> string
            |> Int32.Parse

        let code_function =
            activity.GetTagItem SemanticConventions.General.SourceCode.code_function
            |> string

        Expect.equal code_namespace expectedNamespace "Incorrect namespace"
        Expect.stringContains code_filepath expectedFilePath "Incorrect filepath"
        Expect.isGreaterThan code_lineno -1 "" // Assume parsing and greater than 0 is good enough, otherwise too easy to break tests with moving code around
        Expect.equal code_function expectedFunction "Incorrect function"


module SayTests =
    [<Tests>]
    let tests =
        testList "Tests" [
            testList "Classes" [
                testCase "GetForName"
                <| fun () ->
                    let testClass = SomeTestClasses.TestClass()
                    let activity = testClass.GetForName()
                    Expect.expectDisplayName activity "MyCustomName"

                    let ns =
                        // "SomeTestClasses.TestClass" // ? Figure out why this doesn't give clean namespaces
                        "<StartupCode$FsOpenTelemetry-Tests>.$TestClass"

                    Expect.sourceCodeTags ns "TestClass.fs" "GetForName" activity

                testCase "GetForNameCustomNamespace"
                <| fun () ->
                    let testClass = SomeTestClasses.TestClass()
                    let activity = testClass.GetForNameCustomNamespace()
                    Expect.expectDisplayName activity "MyCustomName"

                    Expect.sourceCodeTags
                        "MyNamespace"
                        "TestClass.fs"
                        "GetForNameCustomNamespace"
                        activity

                testCase "GetForType"
                <| fun () ->
                    let testClass = SomeTestClasses.TestClass()
                    let activity = testClass.GetForType()

                    Expect.expectDisplayName activity "SomeTestClasses.TestClass.GetForType"

                    Expect.sourceCodeTags
                        "SomeTestClasses.TestClass"
                        "TestClass.fs"
                        "GetForType"
                        activity

                testCase "GetForTypeArg"
                <| fun () ->
                    let testClass = SomeTestClasses.TestClass()
                    let activity = testClass.GetForTypeArg()

                    Expect.expectDisplayName activity "SomeTestClasses.TestClass.GetForTypeArg"

                    Expect.sourceCodeTags
                        "SomeTestClasses.TestClass"
                        "TestClass.fs"
                        "GetForTypeArg"
                        activity

                ptestCase "GetForQuote"
                <| fun () ->
                    let testClass = SomeTestClasses.TestClass()
                    let activity = testClass.GetForQuote()

                    Expect.expectDisplayName activity "SomeTestClasses.TestClass.GetForTypeArg"

                    Expect.sourceCodeTags
                        "SomeTestClasses.TestClass"
                        "TestClass.fs"
                        "GetForTypeArg"
                        activity

                testCase "GetForFunc"
                <| fun () ->
                    let testClass = SomeTestClasses.TestClass()
                    let activity = testClass.GetForFunc()

                    Expect.expectDisplayName
                        activity
                        "<StartupCode$FsOpenTelemetry-Tests>.$TestClass.GetForFunc"

                    Expect.sourceCodeTags
                        "<StartupCode$FsOpenTelemetry-Tests>.$TestClass"
                        "TestClass.fs"
                        "GetForFunc"
                        activity

                testCaseAsync "GetForFuncAsync"
                <| async {
                    let testClass = SomeTestClasses.TestClass()
                    let! activity = testClass.GetForFuncAsync()

                    Expect.expectDisplayName
                        activity
                        "<StartupCode$FsOpenTelemetry-Tests>.$TestClass.GetForFuncAsync"

                    Expect.sourceCodeTags
                        "<StartupCode$FsOpenTelemetry-Tests>.$TestClass"
                        "TestClass.fs"
                        "GetForFuncAsync"
                        activity
                }
            ]

            testList "Module" [
                testCase "GetForName"
                <| fun () ->
                    let activity = SomeTestClasses.TestModule.getForName ()
                    Expect.expectDisplayName activity "MyCustomName"

                    let ns = "SomeTestClasses.TestModule"

                    Expect.sourceCodeTags ns "TestClass.fs" "getForName" activity

                testCase "GetForNameCustomNamespace"
                <| fun () ->
                    let activity = SomeTestClasses.TestModule.getForNameCustomNamespace ()
                    Expect.expectDisplayName activity "MyCustomName"

                    Expect.sourceCodeTags
                        "MyNamespace"
                        "TestClass.fs"
                        "getForNameCustomNamespace"
                        activity

                testCase "GetForType"
                <| fun () ->
                    let activity = SomeTestClasses.TestModule.getForType ()

                    Expect.expectDisplayName activity "SomeTestClasses.TestModule+Marker.getForType"

                    Expect.sourceCodeTags
                        "SomeTestClasses.TestModule+Marker"
                        "TestClass.fs"
                        "getForType"
                        activity

                testCase "GetForTypeArg"
                <| fun () ->
                    let activity = SomeTestClasses.TestModule.getForTypeArg ()

                    Expect.expectDisplayName
                        activity
                        "SomeTestClasses.TestModule+Marker.getForTypeArg"

                    Expect.sourceCodeTags
                        "SomeTestClasses.TestModule+Marker"
                        "TestClass.fs"
                        "getForTypeArg"
                        activity

                ptestCase "GetForQuote"
                <| fun () ->
                    let activity = SomeTestClasses.TestModule.getForQuote ()

                    Expect.expectDisplayName activity "SomeTestClasses.TestClass.GetForTypeArg"

                    Expect.sourceCodeTags
                        "SomeTestClasses.TestClass"
                        "TestClass.fs"
                        "GetForTypeArg"
                        activity

                testCase "GetForFunc"
                <| fun () ->
                    let activity = SomeTestClasses.TestModule.getForFunc ()

                    Expect.expectDisplayName activity "SomeTestClasses.TestModule.getForFunc"

                    Expect.sourceCodeTags
                        "SomeTestClasses.TestModule"
                        "TestClass.fs"
                        "getForFunc"
                        activity
                testCaseAsync "GetForFuncAsync"
                <| async {
                    let! activity = SomeTestClasses.TestModule.getForFuncAsync ()

                    Expect.expectDisplayName activity "SomeTestClasses.TestModule.getForFuncAsync"

                    Expect.sourceCodeTags
                        "SomeTestClasses.TestModule"
                        "TestClass.fs"
                        "getForFuncAsync"
                        activity
                }
            ]
        ]
