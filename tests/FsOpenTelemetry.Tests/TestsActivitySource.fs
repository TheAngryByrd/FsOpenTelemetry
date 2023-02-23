namespace FsOpenTelemetry.Tests

module rec Tracing =
    open System.Diagnostics
    type internal Marker = interface end
    let assemblyName = typeof<Marker>.Assembly.GetName();
    let activitySource = new ActivitySource(assemblyName.Name, assemblyName.Version.ToString())

    let activityListener = new ActivityListener(
            ShouldListenTo = (fun s -> true),
            SampleUsingParentId = (fun _ -> ActivitySamplingResult.AllData),
            Sample = (fun _ -> ActivitySamplingResult.AllData)
        )
    ActivitySource.AddActivityListener(activityListener)
