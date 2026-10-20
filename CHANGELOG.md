# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed
- Added common attributes from OpenTelemetry Semantic Conventions v1.44.0
- Added current `network.*`, `server.*`, and `client.*` attributes alongside legacy `net.*` attributes
- Updated documentation URLs to point to current OpenTelemetry specification
- Made `SemanticHelpers.createSourceCodeTags` public for direct use
- Changed `Funcs` and `Unsafe` from private to internal modules

### Added
- HTTP semantic conventions module with stable attributes for HTTP spans
- Database semantic conventions module with attributes for database operations
- Messaging semantic conventions module with attributes for messaging systems
- .NET-specific semantic conventions for runtime and ASP.NET Core
- Server and Client attribute modules for connection-based interactions
- Source and Destination attribute modules for network exchanges
- Current network transport and connection type values
- New cellular carrier attributes for mobile connections
- Current database, messaging, and source-code keys alongside legacy keys
- URL, error, user-agent, service, deployment, telemetry SDK, host, and process keys
- Registered .NET garbage collector and ASP.NET Core keys

## [0.1.0] - 2017-03-17
First release

### Added
- This release already has lots of features

[0.1.0]: https://github.com/user/MyCoolNewLib.git/releases/tag/v0.1.0
