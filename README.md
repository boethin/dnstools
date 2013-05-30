A DNS client library in C#
==============================================

The library includes tools for querying DNS servers and iterative DNS Resolution.

### The goals of the project

  - Error recovery in DNS configurations.
  - Experimental investigation of possibilities in the DNS.
  - Development of ways to optimize client implementations.

### Features:

  - UDP and TCP clients.
  - IPv4 and IPv6 (dual stack) support.
  - Synchronous and asynchronous request methods.
  - An event-based logging mechanism.
  - An interactively controllable resolution iterator.

### Sample usage

##### A simple non-recursive DNS request

<pre>
// using Boethin.Net.DnsTools.DnsClient;
// using Boethin.Net.DnsTools.DnsClient.DNS;

// Get IPv6 addresses (AAAA RRs) of example.com
using (IDnsClient client = new DnsUdpClient())
{
  // a.iana-servers.net. is authoritative for example.com.
  client.Connect("199.43.132.53");

  // Perform non-recursive DNS request
  Response response = client.Process(
    new Request(false, new Question("example.com.", QTYPE.AAAA)));

  // Display result
  if (response.Header.RCODE == RCODE.NoError) // response OK
  {
    foreach (Question q in response.Questions)
      Console.WriteLine("Question: {0}", q);
    foreach (RR rr in response.AnswerRecords)
      Console.WriteLine("Answer: {0}", rr);
  }
  else // some error
    Console.WriteLine("Unexpected RCODE: {0}", response.Header.RCODE.ToString());

  client.Close();
}
</pre>

###### Expected output:
<pre>
Question: example.com. IN AAAA
Answer: example.com. 172800 IN AAAA 2001:500:88:200::10
</pre>


### Future plans:
  - Implementation of EDNS [RFC 2671](http://tools.ietf.org/html/rfc2671).
  - Validation DNSSEC zones.
  - Improving of caching mechanisms.
  - Utilizing the resolver for consistency and existence tests.

Project structure
-----------------

### Core projects

##### *Boethin.Net.DnsTools.DnsClient*

This project contains a DNS client interface *IDnsClient* and a mapping of DNS data to a serializable class structure. The interface is implemented by two classes:

   - *DnsUdpClient*: A UDP client, extending [*System.Net.Sockets.UdpClient*](http://msdn.microsoft.com/en-us/library/system.net.sockets.udpclient%28v=vs.90%29.aspx).
   - *DnsTcpClient*: A TCP client, extending [*System.Net.Sockets.TcpClient*](http://msdn.microsoft.com/en-us/library/system.net.sockets.tcpclient%28v=vs.90%29.aspx).

The client classes are implemented as extensions of the standard network clients provided by the .NET framework. This way the greatest possible flexibility is ensured with minimal implementation effort.

##### *Boethin.Net.DnsTools.Resolution*

This project depends on the *DnsClient* project and contains an interactively controllable DNS resolver.

The resolver is serializable such that it's state can be saved after each iteration step. This way it's possible to implement an iterative step-by-step resolution (within a web application for example).

The mapping of IP addresses to ARPA domains for reverse resolution is implemeted by way of an extension of the *System.Net.IPAddress* class.

### Additional testing projects

##### *Boethin.Net.DnsTools.Tests.ClientApp*

A WinForm application utilizing the DNS client.

##### *Boethin.Net.DnsTools.Tests.ConsoleResolver*

An interactive console application providing iterative name resolution.

(Open *Boethin.Net.DnsTools.Tests.sln* for accessing the test projects.)


Remarks
-------

##### The following specifications have been taken into consideration:
  - [RFC 1035](http://tools.ietf.org/html/rfc1035) Domain Names - Implementation and Specification.
  - [RFC 2181](http://tools.ietf.org/html/rfc2181) Clarifications to the DNS Specification.
  - [RFC 3425](http://tools.ietf.org/html/rfc3425) Obsoleting IQUERY.
  - [RFC 1996](http://tools.ietf.org/html/rfc1996) A Mechanism for Prompt Notification of Zone Changes (DNS NOTIFY)
  - [RFC 3596](http://tools.ietf.org/html/rfc3596) DNS Extensions to Support IP Version 6 (AAAA RRs).
  - [RFC 2782](http://tools.ietf.org/html/rfc2782) A DNS RR for specifying the location of services (SRV RRs).
  - [RFC 3403](http://tools.ietf.org/html/rfc3403) Dynamic Delegation Discovery System (NAPTR RRs).
  - [RFC 4408](http://tools.ietf.org/html/rfc4408) Sender Policy Framework (SPF RRs).

##### External dependencies

The resolver library contains a mirror of the IANA Root Hints File wich needs to be synchronized.
- Source: [http://www.iana.org/domains/root/servers](http://www.iana.org/domains/root/servers)

##### Compatibility considerations

- The library is implemented in .NET 3.5. Both Visual Studio solutions (Boethin.Net.DnsTools.sln and Boethin.Net.DnsTools.Tests.sln) can be opened with Visual studio 2010 express.
- The code is compatible with Mono JIT compiler 2.6.7.

