# FsOpenTelemetry

---

## What is FsOpenTelemetry?

FsOpenTelemetry is a single-file F# library that provides safe helpers for `Activity` and `ActivitySource`, plus constants for common OpenTelemetry attributes.

## Why use FsOpenTelemetry?

F# does not have the `?.` null-conditional operator. A span returned by `ActivitySource` can be null. FsOpenTelemetry provides extension methods that handle null spans.

The attribute catalogue covers common HTTP, database, messaging, .NET, URL, error, and resource keys from [OpenTelemetry Semantic Conventions v1.44.0](https://github.com/open-telemetry/semantic-conventions/tree/v1.44.0/model). It includes stable and development attributes. Legacy keys and helpers remain available for source compatibility. Source-code helpers retain the upstream behavior. They emit current file and line keys, an unqualified function name, and the legacy namespace attribute.

---

<div class="row row-cols-1 row-cols-md-2">
  <div class="col mb-4">
    <div class="card h-100">
      <div class="card-body">
        <h5 class="card-title">Tutorials</h5>
        <p class="card-text">Takes you by the hand through a series of steps to create your first thing. </p>
      </div>
      <div class="card-footer text-right   border-top-0">
        <a href="{{siteBaseUrl}}/Tutorials/Getting_Started.html" class="btn btn-primary">Get started</a>
      </div>
    </div>
  </div>
  <div class="col mb-4">
    <div class="card h-100">
      <div class="card-body">
        <h5 class="card-title">How-To Guides</h5>
        <p class="card-text">Guides you through the steps involved in addressing key problems and use-cases. </p>
      </div>
      <div class="card-footer text-right   border-top-0">
        <a href="{{siteBaseUrl}}/How_Tos/Doing_A_Thing.html" class="btn btn-primary">Learn Usecases</a>
      </div>
    </div>
  </div>
  <div class="col mb-4 mb-md-0">
    <div class="card h-100">
      <div class="card-body">
        <h5 class="card-title">Explanations</h5>
        <p class="card-text">Discusses key topics and concepts at a fairly high level and provide useful background information and explanation..</p>
      </div>
      <div class="card-footer text-right   border-top-0">
        <a href="{{siteBaseUrl}}/Explanations/Background.html" class="btn btn-primary">Dive Deeper</a>
      </div>
    </div>
  </div>
  <div class="col">
    <div class="card h-100">
      <div class="card-body">
        <h5 class="card-title">Api Reference</h5>
        <p class="card-text">Contain technical reference for APIs.</p>
      </div>
      <div class="card-footer text-right   border-top-0">
        <a href="{{siteBaseUrl}}/Api_Reference/FsOpenTelemetry/FsOpenTelemetry.html" class="btn btn-primary">Read Api Docs</a>
      </div>
    </div>
  </div>
</div>
