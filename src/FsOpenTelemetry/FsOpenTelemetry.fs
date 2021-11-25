namespace FsOpenTelemetry
open System
open System.Diagnostics
open System.Runtime.CompilerServices
open System.Collections.Generic



// Thanks https://github.com/fsprojects/FSharp.UMX
[<MeasureAnnotatedAbbreviation>] type string<[<Measure>] 'm> = string
module private Unsafe =
    let inline cast<'a, 'b> (a : 'a) : 'b =
        (# "" a : 'b #)

type UMX =
    static member inline tag<[<Measure>]'m> (x : string) : string<'m> = Unsafe.cast x
    static member inline untag<[<Measure>]'m> (x : string<'m>) : string = Unsafe.cast x
    static member inline cast<[<Measure>]'m1, [<Measure>]'m2> (x : string<'m1>) : string<'m2> = Unsafe.cast x

/// In OpenTelemetry spans can be created freely and it’s up to the implementor to annotate them with attributes specific to the represented operation. Spans represent specific operations in and between systems. Some of these operations represent calls that use well-known protocols like HTTP or database calls. Depending on the protocol and the type of operation, additional information is needed to represent and analyze a span correctly in monitoring systems. It is also important to unify how this attribution is made in different languages. This way, the operator will not need to learn specifics of a language and telemetry collected from polyglot (multi-language) micro-service environments can still be easily correlated and cross-analyzed.
module SemanticConventions =
    /// The attributes described in this section are not specific to a particular operation but rather generic. They may be used in any Span they apply to. Particular operations may refer to or require some of these attributes.
    module General =
        /// These attributes may be used for any network related operation. The net.peer.* attributes describe properties of the remote end of the network connection (usually the transport-layer peer, e.g. the node to which a TCP connection was established), while the net.host.* properties describe the local end. In an ideal situation, not accounting for proxies, multiple IP addresses or host names, the net.peer.* properties of a client are equal to the net.host.* properties of the server and vice versa.
        ///
        /// https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/span-general.md#general-network-connection-attributes
        module Network =
            /// Transport protocol used.
            ///
            /// ValueType: string
            ///
            /// Examples: ip_tcp
            ///
            /// Should use a net_transport_values
            ///
            /// Required: No
            let [<Literal>] net_transport = "net.transport"

            /// net.transport MUST be one of the following
            [<Measure>] type net_transport_values
            /// tcp_ip
            let net_transport_values_ip_tcp : string<net_transport_values> = UMX.tag "icp_tcp"
            /// ip_udp
            let net_transport_values_ip_udp : string<net_transport_values> = UMX.tag "ip_udp"
            // /// Another IP-based protocol
            let net_transport_values_ip : string<net_transport_values> = UMX.tag "ip"
            /// Unix Domain socket.
            let net_transport_values_unix : string<net_transport_values> = UMX.tag "unix"
            // Named or anonymous pipe.
            let net_transport_values_pipe : string<net_transport_values> = UMX.tag "pipe"
            ///Signals that there is only in-process communication not using a "real" network protocol in cases where network attributes would normally be expected. Usually all other network attributes can be left out in that case.
            let net_transport_values_inproc : string<net_transport_values> = UMX.tag "inproc"
            /// Something else (non IP-based).
            let net_transport_values_Other : string<net_transport_values> = UMX.tag "Other"

            /// Remote address of the peer (dotted decimal for IPv4 or RFC5952 for IPv6)
            ///
            /// ValueType: string
            ///
            /// Examples: 127.0.0.1
            ///
            /// Required: No
            let [<Literal>] net_peer_ip = "net.peer.ip"

            /// Remote port number.
            ///
            /// ValueType: int
            ///
            /// Examples: 80; 8080; 443
            ///
            /// Required: No
            let [<Literal>] net_peer_port = "net.peer.port"

            /// Remote hostname or similar.
            ///
            /// ValueType: string
            ///
            /// Examples: example.com
            ///
            /// Required: No
            // TODO: "See Note below"
            let [<Literal>] net_peer_name = "net.peer.name"

            /// Like net.peer.ip but for the host IP. Useful in case of a multi-IP host.
            ///
            /// ValueType: string
            ///
            /// Examples: example.com
            ///
            /// Required: No
            let [<Literal>] net_host_ip = "net.host.ip"

            /// Like net.peer.port but for the host port.
            ///
            /// ValueType: int
            ///
            /// Examples: 80; 8080; 443
            ///
            /// Required: No
            let [<Literal>] net_host_port = "net.host.port"

            /// Local hostname or similar
            ///
            /// ValueType: string
            ///
            /// Examples: localhost
            ///
            /// Required: No
            // TODO: "See Note below"
            let [<Literal>] net_host_name = "net.host.name"
            /// The internet connection type currently being used by the host.
            ///
            /// ValueType: string
            ///
            /// Examples: wifi
            ///
            /// Required: No
            let [<Literal>] net_host_connection_type = "net.host.connection.type"

            /// net.host.connection.type MUST be one of the following or, if none of the listed values apply, a custom value:
            [<Measure>] type net_host_connection_type_values

            let net_host_connection_type_values_wifi : string<net_host_connection_type_values> = UMX.tag "wifi"
            let net_host_connection_type_values_wired : string<net_host_connection_type_values> = UMX.tag "wired"
            let net_host_connection_type_values_cell : string<net_host_connection_type_values> = UMX.tag "cell"
            let net_host_connection_type_values_unavailable : string<net_host_connection_type_values> = UMX.tag "unavailable"
            let net_host_connection_type_values_unknown : string<net_host_connection_type_values> = UMX.tag "unknown"


            /// This describes more details regarding the connection.type. It may be the type of cell technology connection, but it could be used for describing details about a wifi connection.
            ///
            /// ValueType: string
            ///
            /// Examples: lte
            ///
            /// Required: No
            let [<Literal>] net_host_connection_subtype = "net.host.connection.subtype"

            /// net.host.connection.subtype MUST be one of the following or, if none of the listed values apply, a custom value:
            [<Measure>] type net_host_connection_subtype_values


            let net_host_connection_subtype_values_gprs : string<net_host_connection_type_values> = UMX.tag "gprs"
            let net_host_connection_subtype_values_edge : string<net_host_connection_type_values> = UMX.tag "edge"
            let net_host_connection_subtype_values_umts : string<net_host_connection_type_values> = UMX.tag "umts"
            let net_host_connection_subtype_values_cdma : string<net_host_connection_type_values> = UMX.tag "cdma"
            let net_host_connection_subtype_values_evdo_0 : string<net_host_connection_type_values> = UMX.tag "evdo_0"
            let net_host_connection_subtype_values_evdo_a : string<net_host_connection_type_values> = UMX.tag "evdo_a"
            let net_host_connection_subtype_values_cdma2000_1xrtt : string<net_host_connection_type_values> = UMX.tag "cdma2000_1xrtt"
            let net_host_connection_subtype_values_hsdpa : string<net_host_connection_type_values> = UMX.tag "hsdpa"
            let net_host_connection_subtype_values_hsupa : string<net_host_connection_type_values> = UMX.tag "hsupa"
            let net_host_connection_subtype_values_iden : string<net_host_connection_type_values> = UMX.tag "iden"
            let net_host_connection_subtype_values_ehrpd: string<net_host_connection_type_values> = UMX.tag "ehrpd"
            let net_host_connection_subtype_values_hspap: string<net_host_connection_type_values> = UMX.tag "hspap"
            let net_host_connection_subtype_values_gsm: string<net_host_connection_type_values> = UMX.tag "gsm"
            let net_host_connection_subtype_values_td_scdma: string<net_host_connection_type_values> = UMX.tag "td_scdma"
            let net_host_connection_subtype_values_iwlan: string<net_host_connection_type_values> = UMX.tag "iwlan"
            let net_host_connection_subtype_values_nr: string<net_host_connection_type_values> = UMX.tag "nr"
            let net_host_connection_subtype_values_nrnsa: string<net_host_connection_type_values> = UMX.tag "nrnsa"
            let net_host_connection_subtype_values_lte_ca: string<net_host_connection_type_values> = UMX.tag "lte_ca"


            /// The name of the mobile carrier.
            ///
            /// ValueType: string
            ///
            /// Examples: sprint
            ///
            /// Required: No
            let [<Literal>] net_host_carrier_name = "net.host.carrier.name"

            /// The mobile carrier country code.
            ///
            /// ValueType: string
            ///
            /// Examples: 310
            ///
            /// Required: No
            let [<Literal>] net_host_carrier_mcc = "net.host.carrier.mcc"

            /// The mobile carrier network code.
            ///
            /// ValueType: string
            ///
            /// Examples: 001
            ///
            /// Required: No
            let [<Literal>] net_host_carrier_mnc = "net.host.carrier.mnc"

            /// The ISO 3166-1 alpha-2 2-character country code associated with the mobile carrier network.
            ///
            /// ValueType: string
            ///
            /// Examples: DE
            ///
            /// Required: No
            let [<Literal>] net_host_carrier_icc = "net.host.carrier.icc"
        /// This attribute may be used for any operation that accesses some remote service. Users can define what the name of a service is based on their particular semantics in their distributed system. Instrumentations SHOULD provide a way for users to configure this name.
        ///
        /// https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/span-general.md#general-remote-service-attributes
        module Remote =
            /// The service.name of the remote service. SHOULD be equal to the actual service.name resource attribute of the remote service if any.
            ///
            /// ValueType: string
            ///
            /// Examples: AuthTokenCache
            ///
            /// Examples of peer.service that users may specify:
            ///
            /// A Redis cache of auth tokens as peer.service="AuthTokenCache".
            ///
            /// A gRPC service rpc.service="io.opentelemetry.AuthService" may be hosted in both a gateway, peer.service="ExternalApiService" and a backend, peer.service="AuthService".
            ///
            /// Required: No
            let [<Literal>] peer_service = "peer.service"
        /// These attributes may be used for any operation with an authenticated and/or authorized enduser.
        ///
        /// https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/span-general.md#general-identity-attributes
        module Identity =

            /// Username or client_id extracted from the access token or Authorization header in the inbound request from outside the system.
            ///
            /// ValueType: string
            ///
            /// Examples: username
            ///
            /// Required: No
            let [<Literal>] enduser_id = "enduser.id"

            /// Actual/assumed role the client is making the request under extracted from token or application security context.
            ///
            /// ValueType: string
            ///
            /// Examples: admin
            ///
            /// Required: No
            let [<Literal>] enduser_role = "enduser.role"

            /// Scopes or granted authorities the client currently possesses extracted from token or application security context. The value would come from the scope associated with an OAuth 2.0 Access Token or an attribute value in a SAML 2.0 Assertion.
            ///
            /// ValueType: string
            ///
            /// Examples: read:message, write:files
            ///
            /// Required: No
            let [<Literal>] enduser_scope = "enduser.scope"
        /// These attributes may be used for any operation to store information about a thread that started a span.
        ///
        /// https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/span-general.md#general-thread-attributes
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
            let [<Literal>] thread_id = "thread.id"

            /// Current thread name.
            ///
            /// ValueType: string
            ///
            /// Examples: main
            ///
            /// Can use Thread.CurrentThread.Name
            ///
            /// Required: No
            let [<Literal>] thread_name = "thread.name"

        /// Often a span is closely tied to a certain unit of code that is logically responsible for handling the operation that the span describes (usually the method that starts the span). For an HTTP server span, this would be the function that handles the incoming request, for example. The attributes listed below allow to report this unit of code and therefore to provide more context about the span.
        ///
        /// https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/span-general.md#source-code-attributes
        module SourceCode =
            /// The method or function name, or equivalent (usually rightmost part of the code unit's name).
            ///
            /// ValueType: string
            ///
            /// Examples: serveRequest
            ///
            /// Required: No
            let [<Literal>] code_function = "code.function"

            /// The "namespace" within which code.function is defined. Usually the qualified class or module name, such that code.namespace + some separator + code.function form a unique identifier for the code unit.
            ///
            /// ValueType: string
            ///
            /// Examples: com.example.MyHttpService
            ///
            /// Required: No
            let [<Literal>] code_namespace = "code.namespace"

            /// The source code file name that identifies the code unit as uniquely as possible (preferably an absolute file path).
            ///
            /// ValueType: string
            ///
            /// Examples: /usr/local/MyApplication/content_root/app/index.php
            ///
            /// Required: No
            let [<Literal>] code_filepath = "code.filepath"

            /// The line number in code.filepath best representing the operation. It SHOULD point within the code unit named in code.function.
            ///
            /// ValueType: string
            ///
            /// Examples: 42
            ///
            /// Required: No
            let [<Literal>] code_lineno = "code.lineno"
        module Exceptions =


            let [<Literal>] exception_ = "exception"
            /// The type of the exception (its fully-qualified class name, if applicable). The dynamic type of the exception should be preferred over the static type in languages that support it.
            ///
            /// ValueType: string
            ///
            /// Examples: java.net.ConnectException; OSError
            ///
            /// Required: No
            let [<Literal>] exception_type = "exception.type"
            /// The exception message.
            ///
            /// ValueType: string
            ///
            /// Examples: Division by zero; Can't convert 'int' object to str implicitly
            ///
            /// Required: No
            let [<Literal>] exception_message = "exception.message"
            /// A stacktrace as a string in the natural representation for the language runtime. The representation is to be determined and documented by each language SIG.
            ///
            /// ValueType: string
            ///
            /// Examples: Exception in thread "main" java.lang.RuntimeException: Test exception\n at com.example.GenerateTrace.methodB(GenerateTrace.java:13)\n at com.example.GenerateTrace.methodA(GenerateTrace.java:9)\n at com.example.GenerateTrace.main(GenerateTrace.java:5)
            ///
            /// Required: No
            let [<Literal>] exception_stacktrace = "exception.stacktrace"
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
            let [<Literal>] exception_escaped = "exception.escaped"

[<Extension>]
type ActivityExtensions =
    [<Extension>]
    static member inline AddBaggageSafe (span : Activity, key : string,value : string) =
        if span <> null then
            span.AddBaggage(key,value)
        else
            span

    [<Extension>]
    static member inline AddEventSafe (span : Activity, e : ActivityEvent) =
        if span <> null then
            span.AddEvent(e)
        else
            span

    [<Extension>]
    static member inline AddEventSafe (span : Activity, e : string) =
        span.AddEventSafe(ActivityEvent e)

    [<Extension>]
    static member inline SetTagSafe (span : Activity, key, value : obj) =
        if span <> null then
            span.SetTag(key,value)
        else
            span

    [<Extension>]
    static member inline SetStatusErrorSafe (span : Activity, description : string)  =
        span
            .SetTagSafe("otel.status_code","ERROR")
            .SetTagSafe("otel.status_description", description)

    [<Extension>]
    static member inline SetSourceCodeFilePath (span : Activity, value : string) =
        span.SetTagSafe(SemanticConventions.General.SourceCode.code_filepath, value)
    [<Extension>]
    static member inline SetSourceCodeLineNumber (span : Activity, value : int) =
        span.SetTagSafe(SemanticConventions.General.SourceCode.code_lineno, value)
    [<Extension>]
    static member inline SetSourceCodeNamespace (span : Activity, value : string) =
        span.SetTagSafe(SemanticConventions.General.SourceCode.code_namespace, value)
    [<Extension>]
    static member inline SetSourceCodeFunction(span : Activity, value : string) =
        span.SetTagSafe(SemanticConventions.General.SourceCode.code_function, value)

    [<Extension>]
    static member inline SetNetworkNetTransport(span : Activity, value : string<SemanticConventions.General.Network.net_transport_values>) =
        span.SetTagSafe(SemanticConventions.General.Network.net_transport, UMX.untag value)

    [<Extension>]
    static member inline SetNetworkNetHostConnectionType(span : Activity, value : string<SemanticConventions.General.Network.net_host_connection_type_values>) =
        span.SetTagSafe(SemanticConventions.General.Network.net_host_connection_type, UMX.untag value)

    [<Extension>]
    static member inline SetNetworkNetHostConnectionSubType(span : Activity, value : string<SemanticConventions.General.Network.net_host_connection_subtype_values>) =
        span.SetTagSafe(SemanticConventions.General.Network.net_host_connection_subtype, UMX.untag value)

    [<Extension>]
    static member inline RecordExceptions(span : Activity, e : exn, ?escaped : bool) =
        if span <> null then
            let escaped = defaultArg escaped false
            let exceptionType = e.GetType().Name
            let exceptionStackTrace =e.ToString()
            let exceptionMessage = e.Message
            let tags = ActivityTagsCollection([
                yield KeyValuePair(SemanticConventions.General.Exceptions.exception_escaped, escaped)
                yield KeyValuePair(SemanticConventions.General.Exceptions.exception_type, exceptionType)
                yield KeyValuePair(SemanticConventions.General.Exceptions.exception_stacktrace, exceptionStackTrace)
                if not <| String.IsNullOrEmpty(exceptionMessage) then
                    yield KeyValuePair(SemanticConventions.General.Exceptions.exception_message, exceptionMessage)
            ])
            ActivityEvent(SemanticConventions.General.Exceptions.exception_,tags = tags)
            |> span.AddEvent
        else
            span

[<Extension>]
type ActivitySourceExtensions =
    [<Extension>]
    static member inline StartActivityForName(
            tracer : ActivitySource,
            name: string,
            [<CallerMemberName>] ?memberName: string,
            [<CallerFilePath>] ?path: string,
            [<CallerLineNumberAttribute>] ?line: int) =
        let mi = Reflection.MethodBase.GetCurrentMethod()
        let ``namespace`` = mi.DeclaringType.FullName.Split("+") |> Seq.tryHead |> Option.defaultValue ""
        let span =
            name
            |> tracer.StartActivity
        span
            .SetSourceCodeFilePath(path.Value)
            .SetSourceCodeLineNumber(line.Value)
            .SetSourceCodeNamespace(``namespace``)
            .SetSourceCodeFunction(memberName.Value)

    [<Extension>]
    static member inline StartActivityForFunc(
            tracer : ActivitySource,
            [<CallerMemberName>] ?memberName: string,
            [<CallerFilePath>] ?path: string,
            [<CallerLineNumberAttribute>] ?line: int) =
        let mi = Reflection.MethodBase.GetCurrentMethod()
        let ``namespace`` = mi.DeclaringType.FullName.Split("+") |> Seq.tryHead |> Option.defaultValue ""
        let span =
            sprintf "%s.%s" ``namespace`` memberName.Value
            |> tracer.StartActivity
        span
            .SetSourceCodeFilePath(path.Value)
            .SetSourceCodeLineNumber(line.Value)
            .SetSourceCodeNamespace(``namespace``)
            .SetSourceCodeFunction(memberName.Value)