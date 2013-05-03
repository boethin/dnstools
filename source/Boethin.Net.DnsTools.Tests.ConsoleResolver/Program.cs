/*
 * File: Boethin.Net.DnsTools.Tests.ConsoleResolver/Program.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <sebastian@boethin.eu>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Boethin.Net.DnsTools.DnsClient;
using Boethin.Net.DnsTools.DnsClient.DNS;
using Boethin.Net.DnsTools.DnsClient.Logging;
using Boethin.Net.DnsTools.Resolution;
using Boethin.Net.DnsTools.Resolution.Iterators;
using Boethin.Net.DnsTools.Resolution.Results;

namespace Boethin.Net.DnsTools.Tests.ConsoleResolver
{

  class Program
  {

    const string filename = "Boethin.Net.DnsTools.Tests.ConsoleResolver.dat";

    static void Main(string[] args)
    {
      try
      {
        File.Delete(filename);

        bool interactive = false;

        NameServer selected = null;
        while (true)
        {
          if (File.Exists(filename))
          {

            using (DnsUdpClient client = new DnsUdpClient())
            {
              client.Client.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.ReceiveTimeout, 1000);
              
              client.LogMessageCreated += new LogMessageEventHandler(OnLogMessage);
              ResolutionIterator iterator = DeSerializeObject<ResolutionIterator>(filename);

              iterator.Resolver.Connect(client);
              if (iterator.MoveNext(selected))
              {

                Console.WriteLine();

                ResolutionResult result = iterator.Current;

                //Console.WriteLine("Response from {0} - chosen from {1} authorities", result.Authorities.Selected.Name, result.Authorities.ZoneName);
                //foreach (NameServer ns in result.Authorities)
                //{
                //  Console.WriteLine("  {0}", ns);
                //}

                Console.WriteLine("Took {0}ms:", result.Duration.TotalMilliseconds);
                RenderResponse(result.Response);

                if (interactive)
                {
                  selected = null;
                  if (result.NextAuthorities != null)
                  {
                    Console.WriteLine();
                    Console.WriteLine("Next authorities:");
                    int i = 1;
                    foreach (NameServer ns in result.NextAuthorities)
                    {
                      Console.WriteLine("  ({0}) {1}", i, ns.ToString());
                      i++;
                    }

                    while (true)
                    {
                      Console.Write("Select next authority or ENTER for random: ");
                      int s;
                      string choice = Console.ReadLine();
                      if (!String.IsNullOrEmpty(choice))
                      {
                        if (int.TryParse(choice, out s))
                        {
                          if (s >= 1 && s <= result.NextAuthorities.Count)
                          {
                            selected = result.NextAuthorities[s - 1];
                            break;
                          }
                        }
                      }
                      else
                      {
                        break;
                      }
                    }

                  }
                }

                SerializeObject<ResolutionIterator>(filename, iterator);
                Console.WriteLine("====================================");
              }
              else
              {
                Console.WriteLine("finished.");
                File.Delete(filename);
              }
            }

          }
          else 
          {
            DnsDomain domain = null;
            QTYPE question;

            try
            {
              Console.Write("enter name: ");
              domain = DnsDomain.Parse(Console.ReadLine().Trim());
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message);
              goto next;
            }

            try
            {
              Console.Write("enter question: ");
              question = (QTYPE)Enum.Parse(typeof(QTYPE), Console.ReadLine().Trim(), true);
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message);
              goto next;
            }

            Console.WriteLine("OK: {0} IN {1}", domain, question.ToString());

            Resolver resolver = new Resolver(Options.Default, domain);
            ResolutionIterator iterator = resolver.GetIterator(question);
            iterator.LogMessageCreated += new LogMessageEventHandler(OnLogMessage);

            SerializeObject<ResolutionIterator>(filename, iterator);
          }

        next:
          Console.WriteLine();
          Console.WriteLine("type 'q' to quit, ENTER to continue.");
          ConsoleKeyInfo ck = Console.ReadKey();
          if (ck.KeyChar == 'q')
            break;
        }


      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

    static void OnLogMessage(Object sender, LogMessageEventArgs e)
    {
      Console.WriteLine(e.Message.ToString());
    }

    static void RenderResponse(Response response)
    {
      Console.WriteLine(response.Header.ToString());

      Console.WriteLine("Question Section ({0})", response.Questions.Count);
      foreach (Question q in response.Questions)
      {
        Console.WriteLine("  {0}", q);
      }

      Console.WriteLine("Answer Section ({0})", response.AnswerRecords.Count());
      foreach (RR rr in response.AnswerRecords)
      {
        Console.WriteLine("  {0}", rr);
      }

      Console.WriteLine("Authority Section ({0})", response.AuthorityRecords.Count());
      foreach (RR rr in response.AuthorityRecords)
      {
        Console.WriteLine("  {0}", rr);
      }

      Console.WriteLine("Additional Records ({0})", response.AdditionalRecords.Count());
      foreach (RR rr in response.AdditionalRecords)
      {
        Console.WriteLine("  {0}", rr);
      }

      //JavaScriptSerializer js = new JavaScriptSerializer();
      //js.RegisterConverters(new JavaScriptConverter[] {new JsoConverter() });
      //string json = js.Serialize(response);
      //Console.WriteLine(json);

      //Response r = js.Deserialize<Response>(json);

      Console.WriteLine("--");
    }

    static void SerializeObject<T>(string filename, T objectToSerialize)
      where T : class
    {
      Stream stream = File.Open(filename, FileMode.Create);
      BinaryFormatter bFormatter = new BinaryFormatter();
      bFormatter.Serialize(stream, objectToSerialize);
      stream.Close();
    }

    static T DeSerializeObject<T>(string filename)
      where T : class
    {
      object objectToSerialize;
      Stream stream = File.Open(filename, FileMode.Open);
      BinaryFormatter bFormatter = new BinaryFormatter();
      objectToSerialize = bFormatter.Deserialize(stream);
      stream.Close();
      return objectToSerialize as T;
    }

  }
}
