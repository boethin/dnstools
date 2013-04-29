Boethin.Net.DnsTools, a DNS debugging library.
==============================================

The C# library includes tools for querying DNS servers and resolving DNS queries via UDP and TCP using IPv4 and IPv6.

The goals of the project:
-------------------------
  - Error recovery in DNS configurations.
  - Experimental investigation of possibilities in the DNS.
  - Development of ways to optimize client implementations.
  
The following specifications has been taken into consideration:
---------------------------------------------------------------
  - [RFC 1035][http://tools.ietf.org/html/rfc1035] Domain Names - Implementation and Specification.
  - [RFC 2181} Clarifications to the DNS Specification.
  - [RFC 3425] Obsoleting IQUERY.
  - [RFC 1996] A Mechanism for Prompt Notification of Zone Changes (DNS NOTIFY)
  - [RFC 3596] DNS Extensions to Support IP Version 6 (AAAA RRs).
  - [RFC 2782] A DNS RR for specifying the location of services (SRV RRs).
  - [RFC 3403] Dynamic Delegation Discovery System (NAPTR RRs).
  - [RFC 4408] Sender Policy Framework (SPF RRs).

Future plans:
-------------
  - Implementation of EDNS [RFC 2671].
  - Validation DNSSEC zones.
  - Improving of caching mechanisms.
  - Utilizing the resolver for consistency and existence tests.

Project structure
-----------------

The library consists of two projects:

Boethin.Net.DnsTools.DnsClient

This project contains a DNS client interface (IDnsClient) and a mapping of DNS data to a serializable class structure. The interface is implemented two classes, providing UDP and TCP access to DNS servers.
   
The client classes are implemented as extensions of the standard network clients provided by the .NET framework (System.Net.Sockets.UdpClient, System.Net.Sockets.TcpClient). This way the greatest possible flexibility is ensured with minimal implementation effort.

Boethin.Net.DnsTools.Resolution

This project depends on the DnsClient project and contains an interactively controllable DNS resolver.

The resolver is serializable such that it's state can be saved after each iteration step. This way it's possible to implement an iterative resolution within a web application for example.

The mapping of IP addresses to ARPA domains for reverse resolution is implemeted by way of an extension of the System.Net.IPAddress class.


Additionally there are test projects provided (Boethin.Net.DnsTools.Tests.sln):

Boethin.Net.DnsTools.Tests.ClientApp

A WinForm application for utilizing the DNS client.


Boethin.Net.DnsTools.Tests.ConsoleResolver

A console application providing iterative name resolution.


Remarks:

The resolver library contains a mirror of the IANA Root Hints File wich needs to be synchronized.
Source: [http://www.iana.org/domains/root/servers][http://www.iana.org/domains/root/servers]

The library is implemented in .NET 3.5. Buth Visual Studio solutions (Boethin.Net.DnsTools.sln and Boethin.Net.DnsTools.Tests.sln) can be opened with Visual studio 2010 express.

The code is compatible with Mono JIT compiler 2.6.7.
  
  



