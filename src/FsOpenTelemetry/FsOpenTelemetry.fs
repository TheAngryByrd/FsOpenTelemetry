namespace FsOpenTelemetry

open System
open System.Diagnostics
open System.Runtime.CompilerServices
open System.Collections.Generic
open Microsoft.FSharp.Quotations.Patterns


// Thanks https://github.com/fsprojects/FSharp.UMX
[<MeasureAnnotatedAbbreviation>]
type string<[<Measure>] 'm> = string


module private Funcs =
    /// <summary>
    /// Determines whether the given value is not null.
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <returns>True when value is not null, false otherwise.</returns>
    let inline isNotNull value =
        value
        |> isNull
        |> not

module private Unsafe =
    let inline cast<'a, 'b> (a: 'a) : 'b = (# "" a : 'b #)

type UMX =
    static member inline tag<[<Measure>] 'm>(x: string) : string<'m> = Unsafe.cast x
    static member inline untag<[<Measure>] 'm>(x: string<'m>) : string = Unsafe.cast x

    static member inline cast<[<Measure>] 'm1, [<Measure>] 'm2>(x: string<'m1>) : string<'m2> =
        Unsafe.cast x

/// In OpenTelemetry spans can be created freely and it’s up to the implementer to annotate them with attributes specific to the represented operation. Spans represent specific operations in and between systems. Some of these operations represent calls that use well-known protocols like HTTP or database calls. Depending on the protocol and the type of operation, additional information is needed to represent and analyze a span correctly in monitoring systems. It is also important to unify how this attribution is made in different languages. This way, the operator will not need to learn specifics of a language and telemetry collected from polyglot (multi-language) micro-service environments can still be easily correlated and cross-analyzed.
module SemanticConventions =
    /// The attributes described in this section are not specific to a particular operation but rather generic. They may be used in any Span they apply to. Particular operations may refer to or require some of these attributes.
    module General =
        /// These attributes may be used to describe the client and server in a connection-based network interaction and other network attributes.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/
        module Network =
            /// OSI transport layer or inter-process communication method.
            ///
            /// ValueType: string
            ///
            /// Examples: tcp; udp
            ///
            /// Should use a network_transport_values
            ///
            /// Required: No
            [<Literal>]
            let network_transport = "network.transport"

            /// network.transport MUST be one of the following
            [<Measure>]
            type network_transport_values

            /// TCP
            let network_transport_values_tcp: string<network_transport_values> = UMX.tag "tcp"
            /// UDP
            let network_transport_values_udp: string<network_transport_values> = UMX.tag "udp"
            /// Named or anonymous pipe.
            let network_transport_values_pipe: string<network_transport_values> = UMX.tag "pipe"
            /// Unix domain socket.
            let network_transport_values_unix: string<network_transport_values> = UMX.tag "unix"
            /// QUIC
            let network_transport_values_quic: string<network_transport_values> = UMX.tag "quic"

            /// Peer address of the network connection - IP address or Unix domain socket name.
            ///
            /// ValueType: string
            ///
            /// Examples: 10.1.2.80; /tmp/my.sock
            ///
            /// Required: No
            [<Literal>]
            let network_peer_address = "network.peer.address"

            /// Peer port number of the network connection.
            ///
            /// ValueType: int
            ///
            /// Examples: 65123
            ///
            /// Required: No
            [<Literal>]
            let network_peer_port = "network.peer.port"

            /// Local address of the network connection - IP address or Unix domain socket name.
            ///
            /// ValueType: string
            ///
            /// Examples: 10.1.2.80; /tmp/my.sock
            ///
            /// Required: No
            [<Literal>]
            let network_local_address = "network.local.address"

            /// Local port number of the network connection.
            ///
            /// ValueType: int
            ///
            /// Examples: 65123
            ///
            /// Required: No
            [<Literal>]
            let network_local_port = "network.local.port"

            /// OSI application layer or non-OSI equivalent.
            ///
            /// ValueType: string
            ///
            /// Examples: amqp; http; mqtt
            ///
            /// Required: No
            [<Literal>]
            let network_protocol_name = "network.protocol.name"

            /// The actual version of the protocol used for network communication.
            ///
            /// ValueType: string
            ///
            /// Examples: 1.1; 2
            ///
            /// Required: No
            [<Literal>]
            let network_protocol_version = "network.protocol.version"

            /// OSI network layer or non-OSI equivalent.
            ///
            /// ValueType: string
            ///
            /// Examples: ipv4; ipv6
            ///
            /// Should use a network_type_values
            ///
            /// Required: No
            [<Literal>]
            let network_type = "network.type"

            /// network.type MUST be one of the following
            [<Measure>]
            type network_type_values

            /// IPv4
            let network_type_values_ipv4: string<network_type_values> = UMX.tag "ipv4"
            /// IPv6
            let network_type_values_ipv6: string<network_type_values> = UMX.tag "ipv6"

            /// The internet connection type.
            ///
            /// ValueType: string
            ///
            /// Examples: wifi
            ///
            /// Required: No
            [<Literal>]
            let network_connection_type = "network.connection.type"

            /// network.connection.type MUST be one of the following or, if none of the listed values apply, a custom value:
            [<Measure>]
            type network_connection_type_values

            let network_connection_type_values_wifi: string<network_connection_type_values> =
                UMX.tag "wifi"

            let network_connection_type_values_wired: string<network_connection_type_values> =
                UMX.tag "wired"

            let network_connection_type_values_cell: string<network_connection_type_values> =
                UMX.tag "cell"

            let network_connection_type_values_unavailable: string<network_connection_type_values> =
                UMX.tag "unavailable"

            let network_connection_type_values_unknown: string<network_connection_type_values> =
                UMX.tag "unknown"

            /// This describes more details regarding the connection.type. It may be the type of cell technology connection, but it could be used for describing details about a wifi connection.
            ///
            /// ValueType: string
            ///
            /// Examples: LTE
            ///
            /// Required: No
            [<Literal>]
            let network_connection_subtype = "network.connection.subtype"

            /// network.connection.subtype MUST be one of the following or, if none of the listed values apply, a custom value:
            [<Measure>]
            type network_connection_subtype_values

            let network_connection_subtype_values_gprs: string<network_connection_subtype_values> =
                UMX.tag "gprs"

            let network_connection_subtype_values_edge: string<network_connection_subtype_values> =
                UMX.tag "edge"

            let network_connection_subtype_values_umts: string<network_connection_subtype_values> =
                UMX.tag "umts"

            let network_connection_subtype_values_cdma: string<network_connection_subtype_values> =
                UMX.tag "cdma"

            let network_connection_subtype_values_evdo_0: string<network_connection_subtype_values> =
                UMX.tag "evdo_0"

            let network_connection_subtype_values_evdo_a: string<network_connection_subtype_values> =
                UMX.tag "evdo_a"

            let network_connection_subtype_values_evdo_b: string<network_connection_subtype_values> =
                UMX.tag "evdo_b"

            let network_connection_subtype_values_cdma2000_1xrtt
                : string<network_connection_subtype_values> =
                UMX.tag "cdma2000_1xrtt"

            let network_connection_subtype_values_hsdpa: string<network_connection_subtype_values> =
                UMX.tag "hsdpa"

            let network_connection_subtype_values_hsupa: string<network_connection_subtype_values> =
                UMX.tag "hsupa"

            let network_connection_subtype_values_hspa: string<network_connection_subtype_values> =
                UMX.tag "hspa"

            let network_connection_subtype_values_iden: string<network_connection_subtype_values> =
                UMX.tag "iden"

            let network_connection_subtype_values_ehrpd: string<network_connection_subtype_values> =
                UMX.tag "ehrpd"

            let network_connection_subtype_values_hspap: string<network_connection_subtype_values> =
                UMX.tag "hspap"

            let network_connection_subtype_values_gsm: string<network_connection_subtype_values> =
                UMX.tag "gsm"

            let network_connection_subtype_values_td_scdma: string<network_connection_subtype_values> =
                UMX.tag "td_scdma"

            let network_connection_subtype_values_iwlan: string<network_connection_subtype_values> =
                UMX.tag "iwlan"

            let network_connection_subtype_values_nr: string<network_connection_subtype_values> =
                UMX.tag "nr"

            let network_connection_subtype_values_nrnsa: string<network_connection_subtype_values> =
                UMX.tag "nrnsa"

            let network_connection_subtype_values_lte_ca: string<network_connection_subtype_values> =
                UMX.tag "lte_ca"

            /// The name of the mobile carrier.
            ///
            /// ValueType: string
            ///
            /// Examples: sprint
            ///
            /// Required: No
            [<Literal>]
            let network_carrier_name = "network.carrier.name"

            /// The mobile carrier country code.
            ///
            /// ValueType: string
            ///
            /// Examples: 310
            ///
            /// Required: No
            [<Literal>]
            let network_carrier_mcc = "network.carrier.mcc"

            /// The mobile carrier network code.
            ///
            /// ValueType: string
            ///
            /// Examples: 001
            ///
            /// Required: No
            [<Literal>]
            let network_carrier_mnc = "network.carrier.mnc"

            /// The ISO 3166-1 alpha-2 2-character country code associated with the mobile carrier network.
            ///
            /// ValueType: string
            ///
            /// Examples: DE
            ///
            /// Required: No
            [<Literal>]
            let network_carrier_icc = "network.carrier.icc"

        /// These attributes describe the server in a client-server connection.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/#server-attributes
        module Server =
            /// Server domain name if available without reverse DNS lookup; otherwise, IP address or Unix domain socket name.
            ///
            /// ValueType: string
            ///
            /// Examples: example.com; 10.1.2.80; /tmp/my.sock
            ///
            /// Required: No
            [<Literal>]
            let server_address = "server.address"

            /// Server port number.
            ///
            /// ValueType: int
            ///
            /// Examples: 80; 8080; 443
            ///
            /// Required: No
            [<Literal>]
            let server_port = "server.port"

        /// These attributes describe the client in a client-server connection.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/#client-attributes
        module Client =
            /// Client address - domain name if available without reverse DNS lookup; otherwise, IP address or Unix domain socket name.
            ///
            /// ValueType: string
            ///
            /// Examples: client.example.com; 10.1.2.80; /tmp/my.sock
            ///
            /// Required: No
            [<Literal>]
            let client_address = "client.address"

            /// Client port number.
            ///
            /// ValueType: int
            ///
            /// Examples: 65123
            ///
            /// Required: No
            [<Literal>]
            let client_port = "client.port"

        /// These attributes describe the source in a network exchange/packet where there is no clear client/server relationship.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/#source-and-destination-attributes
        module Source =
            /// Source address - domain name if available without reverse DNS lookup; otherwise, IP address or Unix domain socket name.
            ///
            /// ValueType: string
            ///
            /// Examples: source.example.com; 10.1.2.80; /tmp/my.sock
            ///
            /// Required: No
            [<Literal>]
            let source_address = "source.address"

            /// Source port number.
            ///
            /// ValueType: int
            ///
            /// Examples: 3389; 2888
            ///
            /// Required: No
            [<Literal>]
            let source_port = "source.port"

        /// These attributes describe the destination in a network exchange/packet where there is no clear client/server relationship.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/#source-and-destination-attributes
        module Destination =
            /// Destination address - domain name if available without reverse DNS lookup; otherwise, IP address or Unix domain socket name.
            ///
            /// ValueType: string
            ///
            /// Examples: destination.example.com; 10.1.2.80; /tmp/my.sock
            ///
            /// Required: No
            [<Literal>]
            let destination_address = "destination.address"

            /// Destination port number.
            ///
            /// ValueType: int
            ///
            /// Examples: 3389; 2888
            ///
            /// Required: No
            [<Literal>]
            let destination_port = "destination.port"

        /// Attributes of the service.peer.* namespace may be used for any operation that accesses some remote service.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/#general-remote-service-attributes
        module Remote =
            /// Logical name of the service on the other side of the connection. SHOULD be equal to the actual service.name resource attribute of the remote service if any.
            ///
            /// ValueType: string
            ///
            /// Examples: shoppingcart
            ///
            /// Required: No
            [<Literal>]
            let service_peer_name = "service.peer.name"

            /// Logical namespace of the service on the other side of the connection. SHOULD be equal to the actual service.namespace resource attribute of the remote service if any.
            ///
            /// ValueType: string
            ///
            /// Examples: Shop
            ///
            /// Required: No
            [<Literal>]
            let service_peer_namespace = "service.peer.namespace"

        /// These attributes may be used for any operation with an authenticated and/or authorized enduser.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/
        module Identity =

            /// Unique identifier of an end user in the system. It may be a username, email address, or other identifier.
            ///
            /// ValueType: string
            ///
            /// Examples: username
            ///
            /// Required: No
            [<Literal>]
            let enduser_id = "enduser.id"

        /// These attributes may be used for any operation to store information about a thread that started a span.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/#general-thread-attributes
        module Thread =

            /// Current "managed" thread ID (as opposed to OS thread ID).
            ///
            /// ValueType: int
            ///
            /// Examples: 42
            ///
            /// Can use Thread.CurrentThread.ManagedThreadId
            ///
            /// Required: No
            [<Literal>]
            let thread_id = "thread.id"

            /// Current thread name.
            ///
            /// ValueType: string
            ///
            /// Examples: main
            ///
            /// Can use Thread.CurrentThread.Name
            ///
            /// Required: No
            [<Literal>]
            let thread_name = "thread.name"

        /// Often a span is closely tied to a certain unit of code that is logically responsible for handling the operation that the span describes (usually the method that starts the span). For an HTTP server span, this would be the function that handles the incoming request, for example. The attributes listed below allow to report this unit of code and therefore to provide more context about the span.
        ///
        /// https://opentelemetry.io/docs/specs/semconv/general/attributes/#source-code-attributes
        module SourceCode =
            /// The method or function fully-qualified name without arguments.
            ///
            /// ValueType: string
            ///
            /// Examples: com.example.MyHttpService.serveRequest
            ///
            /// Required: No
            [<Literal>]
            let code_function = "code.function.name"

            /// The "namespace" within which code.function is defined. Usually the qualified class or module name, such that code.namespace + some separator + code.function form a unique identifier for the code unit.
            ///
            /// ValueType: string
            ///
            /// Examples: com.example.MyHttpService
            ///
            /// Required: No
            [<Literal>]
            let code_namespace = "code.namespace"

            /// The source code file name that identifies the code unit as uniquely as possible (preferably an absolute file path).
            ///
            /// ValueType: string
            ///
            /// Examples: /usr/local/MyApplication/content_root/app/index.php
            ///
            /// Required: No
            [<Literal>]
            let code_filepath = "code.file.path"

            /// The line number in code.filepath best representing the operation. It SHOULD point within the code unit named in code.function.
            ///
            /// ValueType: int
            ///
            /// Examples: 42
            ///
            /// Required: No
            [<Literal>]
            let code_lineno = "code.line.number"

        module Exceptions =


            [<Literal>]
            let exception_ = "exception"

            /// The type of the exception (its fully-qualified class name, if applicable). The dynamic type of the exception should be preferred over the static type in languages that support it.
            ///
            /// ValueType: string
            ///
            /// Examples: java.net.ConnectException; OSError
            ///
            /// Required: No
            [<Literal>]
            let exception_type = "exception.type"

            /// The exception message.
            ///
            /// ValueType: string
            ///
            /// Examples: Division by zero; Can't convert 'int' object to str implicitly
            ///
            /// Required: No
            [<Literal>]
            let exception_message = "exception.message"

            /// A stacktrace as a string in the natural representation for the language runtime. The representation is to be determined and documented by each language SIG.
            ///
            /// ValueType: string
            ///
            /// Examples: Exception in thread "main" java.lang.RuntimeException: Test exception\n at com.example.GenerateTrace.methodB(GenerateTrace.java:13)\n at com.example.GenerateTrace.methodA(GenerateTrace.java:9)\n at com.example.GenerateTrace.main(GenerateTrace.java:5)
            ///
            /// Required: No
            [<Literal>]
            let exception_stacktrace = "exception.stacktrace"

            /// SHOULD be set to true if the exception event is recorded at a point where it is known that the exception is escaping the scope of the span.
            ///
            /// An exception is considered to have escaped (or left) the scope of a span, if that span is ended while the exception is still logically "in flight". This may be actually "in flight" in some languages (e.g. if the exception is passed to a Context manager's __exit__ method in Python) but will usually be caught at the point of recording the exception in most languages.
            ///
            /// It is usually not possible to determine at the point where an exception is thrown whether it will escape the scope of a span. However, it is trivial to know that an exception will escape, if one checks for an active exception just before ending the span, as done in the example above.
            ///
            /// It follows that an exception may still escape the scope of the span even if the exception.escaped attribute was not set or set to false, since the event might have been recorded at a time where it was not clear whether the exception will escape.
            ///
            /// ValueType: boolean
            ///
            /// Required: No
            [<Literal>]
            let exception_escaped = "exception.escaped"

module private SemanticHelpers =
    let inline createSourceCodeTags
        (filePath: string)
        (codeLine: int)
        (name_space: string)
        (functionName: string)
        =
        seq {
            SemanticConventions.General.SourceCode.code_filepath, box filePath
            SemanticConventions.General.SourceCode.code_lineno, box codeLine
            SemanticConventions.General.SourceCode.code_namespace, box name_space
            SemanticConventions.General.SourceCode.code_function, box functionName
        }

[<Extension>]
type ActivityExtensions =
    [<Extension>]

    /// <summary>
    /// Add or update the Activity baggage with the input key and value.
    ///
    /// If the input value is null
    ///     - if the collection has any baggage with the same key, then this baggage will get removed from the collection.
    ///     - otherwise, nothing will happen and the collection will not change.
    ///
    /// If the input value is not null
    ///     - if the collection has any baggage with the same key, then the value mapped to this key will get updated with the new input value.
    ///     - otherwise, the key and value will get added as a new baggage to the collection.
    ///
    ///
    /// https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.activity.setbaggage?view=net-6.0
    /// </summary>
    /// <param name="span">The activity to add the baggage to</param>
    /// <param name="key">The baggage key name</param>
    /// <param name="value">The baggage value mapped to the input key</param>
    /// <returns><see langword="this" /> for convenient chaining.</returns>
    static member inline SetBaggageSafe(span: Activity, key: string, value: string) =
        if not (isNull span) then
            span.AddBaggage(key, value)
        else
            span

    [<Extension>]
    /// <summary>
    /// Add <see cref="ActivityEvent" /> object to the <see cref="Events" /> list.
    ///
    /// https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.activity.addevent?view=net-6.0
    /// </summary>
    /// <param name="span">The activity to add the baggage to</param>
    /// <param name="e"> object of <see cref="ActivityEvent"/> to add to the attached events list.</param>
    /// <returns><see langword="this" /> for convenient chaining.</returns>
    static member inline AddEventSafe(span: Activity, e: ActivityEvent) =
        if Funcs.isNotNull span then span.AddEvent(e) else span

    [<Extension>]
    /// <summary>
    /// Add or update the Activity tag with the input key and value.
    ///
    /// If the input value is null
    ///     - if the collection has any tag with the same key, then this tag will get removed from the collection.
    ///     - otherwise, nothing will happen and the collection will not change.
    ///
    /// If the input value is not null
    ///     - if the collection has any tag with the same key, then the value mapped to this key will get updated with the new input value.
    ///     - otherwise, the key and value will get added as a new tag to the collection.
    /// </summary>
    /// <param name="span">The activity to add the baggage to</param>
    /// <param name="key">The tag key name</param>
    /// <param name="value">The tag value mapped to the input key</param>
    /// <returns><see langword="this" /> for convenient chaining.</returns>
    static member inline SetTagSafe(span: Activity, key, value: obj) =
        if Funcs.isNotNull span then
            span.SetTag(key, value)
        else
            span

    [<Extension>]
    static member inline SetStatusErrorSafe(span: Activity, description: string) =
        span
            .SetTagSafe("otel.status_code", "ERROR")
            .SetTagSafe("otel.status_description", description)

    [<Extension>]
    static member inline SetSourceCodeFilePath(span: Activity, value: string) =
        span.SetTagSafe(SemanticConventions.General.SourceCode.code_filepath, value)

    [<Extension>]
    static member inline SetSourceCodeLineNumber(span: Activity, value: int) =
        span.SetTagSafe(SemanticConventions.General.SourceCode.code_lineno, value)

    [<Extension>]
    static member inline SetSourceCodeNamespace(span: Activity, value: string) =
        span.SetTagSafe(SemanticConventions.General.SourceCode.code_namespace, value)

    [<Extension>]
    static member inline SetSourceCodeFunction(span: Activity, value: string) =
        span.SetTagSafe(SemanticConventions.General.SourceCode.code_function, value)

    [<Extension>]
    static member inline SetNetworkNetTransport
        (
            span: Activity,
            value: string<SemanticConventions.General.Network.net_transport_values>
        ) =
        span.SetTagSafe(SemanticConventions.General.Network.net_transport, UMX.untag value)

    [<Extension>]
    static member inline SetNetworkNetHostConnectionType
        (
            span: Activity,
            value: string<SemanticConventions.General.Network.net_host_connection_type_values>
        ) =
        span.SetTagSafe(
            SemanticConventions.General.Network.net_host_connection_type,
            UMX.untag value
        )

    [<Extension>]
    static member inline SetNetworkNetHostConnectionSubType
        (
            span: Activity,
            value: string<SemanticConventions.General.Network.net_host_connection_subtype_values>
        ) =
        span.SetTagSafe(
            SemanticConventions.General.Network.net_host_connection_subtype,
            UMX.untag value
        )


    /// <summary>https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/exceptions.md#semantic-conventions-for-exceptions</summary>
    /// <param name="span">The span to add the error information to</param>
    /// <param name="errorMessage">The exception message.</param>
    /// <param name="errorType">The type of the exception (its fully-qualified class name, if applicable). The dynamic type of the exception should be preferred over the static type in languages that support it.</param>
    /// <param name="stacktrace">A stacktrace as a string in the natural representation for the language runtime. The representation is to be determined and documented by each language SIG.</param>
    /// <param name="escaped">SHOULD be set to true if the exception event is recorded at a point where it is known that the exception is escaping the scope of the span. </param>
    [<Extension>]
    static member inline RecordError
        (
            span: Activity,
            errorMessage: string,
            errorType: string,
            ?stacktrace: string,
            ?escaped: bool
        ) =
        if Funcs.isNotNull span then
            let escaped = defaultArg escaped false

            let tags =
                ActivityTagsCollection(
                    [
                        yield
                            KeyValuePair(
                                SemanticConventions.General.Exceptions.exception_escaped,
                                box escaped
                            )
                        yield
                            KeyValuePair(
                                SemanticConventions.General.Exceptions.exception_type,
                                box errorType
                            )

                        if Option.isSome stacktrace then
                            yield
                                KeyValuePair(
                                    SemanticConventions.General.Exceptions.exception_stacktrace,
                                    box stacktrace.Value
                                )

                        yield
                            KeyValuePair(
                                SemanticConventions.General.Exceptions.exception_message,
                                box errorMessage
                            )
                    ]
                )

            ActivityEvent(SemanticConventions.General.Exceptions.exception_, tags = tags)
            |> span.AddEvent
        else
            span

    /// <summary>https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/exceptions.md#semantic-conventions-for-exceptions</summary>
    /// <param name="span">The span to add the error information to</param>
    /// <param name="errorMessage">The exception message.</param>
    /// <param name="errorType">The type of the exception (its fully-qualified class name, if applicable). The dynamic type of the exception should be preferred over the static type in languages that support it.</param>
    /// <param name="stacktrace">A stacktrace as a string in the natural representation for the language runtime. The representation is to be determined and documented by each language SIG.</param>
    /// <param name="escaped">SHOULD be set to true if the exception event is recorded at a point where it is known that the exception is escaping the scope of the span. </param>
    [<Extension>]
    static member inline RecordExceptions(span: Activity, e: exn, ?escaped: bool) =
        if Funcs.isNotNull span then
            let escaped = defaultArg escaped false
            let exceptionType = e.GetType().Name
            let exceptionStackTrace = e.ToString()
            let exceptionMessage = e.Message

            let tags =
                ActivityTagsCollection(
                    [
                        yield
                            KeyValuePair(
                                SemanticConventions.General.Exceptions.exception_escaped,
                                box escaped
                            )
                        yield
                            KeyValuePair(
                                SemanticConventions.General.Exceptions.exception_type,
                                box exceptionType
                            )
                        yield
                            KeyValuePair(
                                SemanticConventions.General.Exceptions.exception_stacktrace,
                                box exceptionStackTrace
                            )
                        if
                            not
                            <| String.IsNullOrEmpty(exceptionMessage)
                        then
                            yield
                                KeyValuePair(
                                    SemanticConventions.General.Exceptions.exception_message,
                                    box exceptionMessage
                                )
                    ]
                )

            ActivityEvent(SemanticConventions.General.Exceptions.exception_, tags = tags)
            |> span.AddEvent
        else
            span

[<Extension>]
type ActivitySourceExtensions =

    /// <summary>Creates and starts a new System.Diagnostics.Activity object if there is any listener to the Activity events, returns null otherwise.</summary>
    /// <param name="tracer">Provides APIs to create and start System.Diagnostics.Activity objects.</param>
    /// <param name="name">The operation name of the Activity.</param>
    /// <param name="name_space">The namespace where this code is located.</param>
    /// <param name="activityKind">The System.Diagnostics.ActivityKind</param>
    /// <param name="parentContext">The parent System.Diagnostics.ActivityContext object to initialize the created Activity object with.</param>
    /// <param name="tags">The optional tags list to initialize the created Activity object with.</param>
    /// <param name="links">The optional System.Diagnostics.ActivityLink list to initialize the created Activity object with.</param>
    /// <param name="startTime">The optional start timestamp to set on the created Activity object.</param>
    /// <param name="memberName">Uses CallerMemberName, should not be set unless you know what you're doing.</param>
    /// <param name="path">Uses CallerFilePath, should not be set unless you know what you're doing.</param>
    /// <param name="line">Uses CallerLineNumberAttribute, should not be set unless you know what you're doing.</param>
    /// <returns>The created System.Diagnostics.Activity object or null if there is no any listener.</returns>
    [<Extension>]
    static member inline StartActivityExt
        (
            tracer: ActivitySource,
            ?name: string,
            ?name_space: string,
            ?activityKind: ActivityKind,
            ?parentContext: ActivityContext,
            ?tags: IEnumerable<string * obj>,
            ?links: IEnumerable<ActivityLink>,
            ?startTime: DateTimeOffset,
            [<CallerMemberName>] ?memberName: string,
            [<CallerFilePath>] ?path: string,
            [<CallerLineNumberAttribute>] ?line: int
        ) =

        let name_space =
            name_space
            |> Option.defaultWith (fun () ->
                Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split("+")
                |> Seq.tryHead
                |> Option.defaultValue ""
            )


        let kind = defaultArg activityKind ActivityKind.Internal

        let tags =
            seq {
                yield!
                    SemanticHelpers.createSourceCodeTags
                        path.Value
                        line.Value
                        name_space
                        memberName.Value

                match tags with
                | Some t -> yield! t
                | None -> ()
            }
            |> Seq.map KeyValuePair

        let name =
            name
            |> Option.defaultWith (fun () -> $"{name_space}.{memberName.Value}")

        let span =
            tracer.StartActivity(
                name = name,
                kind = kind,
                ?parentContext = parentContext,
                tags = tags,
                ?links = links,
                ?startTime = startTime
            )

        span

    /// <summary>This should be used by methods in classes. Creates and starts a new System.Diagnostics.Activity object if there is any listener to the Activity events, returns null otherwise.</summary>
    /// <param name="tracer">Provides APIs to create and start System.Diagnostics.Activity objects.</param>
    /// <param name="ty">The type where the trace is located.</param>
    /// <param name="name_space">The namespace where this code is located.</param>
    /// <param name="activityKind">The System.Diagnostics.ActivityKind</param>
    /// <param name="parentContext">The parent System.Diagnostics.ActivityContext object to initialize the created Activity object with.</param>
    /// <param name="tags">The optional tags list to initialize the created Activity object with.</param>
    /// <param name="links">The optional System.Diagnostics.ActivityLink list to initialize the created Activity object with.</param>
    /// <param name="startTime">The optional start timestamp to set on the created Activity object.</param>
    /// <param name="memberName">Uses CallerMemberName, should not be set unless you know what you're doing.</param>
    /// <param name="path">Uses CallerFilePath, should not be set unless you know what you're doing.</param>
    /// <param name="line">Uses CallerLineNumberAttribute, should not be set unless you know what you're doing.</param>
    /// <returns>The created System.Diagnostics.Activity object or null if there is no any listener.</returns>
    [<Extension>]
    static member inline StartActivityForType
        (
            tracer: ActivitySource,
            ty: Type,
            ?activityKind: ActivityKind,
            ?parentContext: ActivityContext,
            ?tags: IEnumerable<string * obj>,
            ?links: IEnumerable<ActivityLink>,
            ?startTime: DateTimeOffset,
            [<CallerMemberName>] ?memberName: string,
            [<CallerFilePath>] ?path: string,
            [<CallerLineNumberAttribute>] ?line: int
        ) =
        let name_space = ty.FullName
        let name = $"{name_space}.{memberName.Value}"

        tracer.StartActivityExt(
            name,
            name_space = name_space,
            ?activityKind = activityKind,
            ?parentContext = parentContext,
            ?tags = tags,
            ?links = links,
            ?startTime = startTime,
            ?memberName = memberName,
            ?path = path,
            ?line = line
        )


    /// <summary>This should be used by methods in classes. Creates and starts a new System.Diagnostics.Activity object if there is any listener to the Activity events, returns null otherwise.</summary>
    /// <param name="tracer">Provides APIs to create and start System.Diagnostics.Activity objects.</param>
    /// <param name="activityKind">The System.Diagnostics.ActivityKind</param>
    /// <param name="parentContext">The parent System.Diagnostics.ActivityContext object to initialize the created Activity object with.</param>
    /// <param name="tags">The optional tags list to initialize the created Activity object with.</param>
    /// <param name="links">The optional System.Diagnostics.ActivityLink list to initialize the created Activity object with.</param>
    /// <param name="startTime">The optional start timestamp to set on the created Activity object.</param>
    /// <param name="memberName">Uses CallerMemberName, should not be set unless you know what you're doing.</param>
    /// <param name="path">Uses CallerFilePath, should not be set unless you know what you're doing.</param>
    /// <param name="line">Uses CallerLineNumberAttribute, should not be set unless you know what you're doing.</param>
    /// <typeparam name="'typAr">The type where the trace is located.</typeparam>
    /// <returns>The created System.Diagnostics.Activity object or null if there is no any listener.</returns>
    [<Extension>]
    static member inline StartActivityForType<'typAr>
        (
            tracer: ActivitySource,
            ?activityKind: ActivityKind,
            ?parentContext: ActivityContext,
            ?tags: IEnumerable<string * obj>,
            ?links: IEnumerable<ActivityLink>,
            ?startTime: DateTimeOffset,
            [<CallerMemberName>] ?memberName: string,
            [<CallerFilePath>] ?path: string,
            [<CallerLineNumberAttribute>] ?line: int
        ) =
        let ty = typeof<'typAr>

        tracer.StartActivityForType(
            ty,
            ?activityKind = activityKind,
            ?parentContext = parentContext,
            ?tags = tags,
            ?links = links,
            ?startTime = startTime,
            ?memberName = memberName,
            ?path = path,
            ?line = line
        )

    /// <summary>This should be used by functions in modules. Creates and starts a new System.Diagnostics.Activity object if there is any listener to the Activity events, returns null otherwise.</summary>
    /// <param name="tracer">Provides APIs to create and start System.Diagnostics.Activity objects.</param>
    /// <param name="activityKind">The System.Diagnostics.ActivityKind</param>
    /// <param name="parentContext">The parent System.Diagnostics.ActivityContext object to initialize the created Activity object with.</param>
    /// <param name="tags">The optional tags list to initialize the created Activity object with.</param>
    /// <param name="links">The optional System.Diagnostics.ActivityLink list to initialize the created Activity object with.</param>
    /// <param name="startTime">The optional start timestamp to set on the created Activity object.</param>
    /// <param name="memberName">Uses CallerMemberName, should not be set unless you know what you're doing.</param>
    /// <param name="path">Uses CallerFilePath, should not be set unless you know what you're doing.</param>
    /// <param name="line">Uses CallerLineNumberAttribute, should not be set unless you know what you're doing.</param>
    /// <returns>The created System.Diagnostics.Activity object or null if there is no any listener.</returns>
    [<Extension>]
    static member inline StartActivityForFunc
        (
            tracer: ActivitySource,
            ?activityKind: ActivityKind,
            ?parentContext: ActivityContext,
            ?tags: IEnumerable<string * obj>,
            ?links: IEnumerable<ActivityLink>,
            ?startTime: DateTimeOffset,
            [<CallerMemberName>] ?memberName: string,
            [<CallerFilePath>] ?path: string,
            [<CallerLineNumberAttribute>] ?line: int
        ) =

        tracer.StartActivityExt(
            ?name = None,
            ?name_space = None,
            ?activityKind = activityKind,
            ?parentContext = parentContext,
            ?tags = tags,
            ?links = links,
            ?startTime = startTime,
            ?memberName = memberName,
            ?path = path,
            ?line = line
        )
