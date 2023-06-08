/*
 * File: Boethin.Net.DnsTools.Tests.ClientApp/Form1.cs
 *
 * This file is part of Boethin.Net.DnsTools, a DNS debugging library.
 * Copyright (C) 2013 Sebastian Boethin <sebastian@boethin.eu>
 * 
 * 
 * MIT License
 * 
 * Copyright (c) 2023 Sebastian Böthin
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 */

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Boethin.Net.DnsTools.DnsClient;
using Boethin.Net.DnsTools.DnsClient.DNS;
using Boethin.Net.DnsTools.Resolution;

namespace Boethin.Net.DnsTools.Tests.ConsoleResolver
{
  public partial class Form1 : Form
  {

    public Form1()
    {
      InitializeComponent();

      System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
      System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

    }

    // http://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
    delegate void SetTextCallback(Control ctrl, string text);

    private void SetText(Control ctrl, string text)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (ctrl.InvokeRequired)
      {
        SetTextCallback d = new SetTextCallback(SetText);
        this.Invoke(d, new object[] { ctrl, text });
      }
      else
      {
        ctrl.Text = text;
      }
    }

    public delegate void ClientSampleSetUp(Form1 form);

    private static readonly Dictionary<string, ClientSampleSetUp> ClientSamples = new Dictionary<string, ClientSampleSetUp> 
    { 
      { "example.com. via 8.8.8.8 TRUNCATED", form => {
        form.textBoxClientServer.Text = "8.8.8.8";
        form.radioButtonClientUDP.Checked = true;
        form.checkBoxClientRD.Checked = true;
        form.textBoxClientQNAME.Text = "example.com.";
        form.comboBoxClientQCLASS.SelectedItem = QCLASS.IN.ToString();
        form.comboBoxClientQTYPE.SelectedItem = QTYPE.ANY.ToString();
      } },
      { "example.com. non-recursive TCP", form => {
        form.textBoxClientServer.Text = "a.iana-servers.net";
        form.radioButtonClientTCP.Checked = true;
        form.checkBoxClientRD.Checked = false;
        form.textBoxClientQNAME.Text = "example.com.";
        form.comboBoxClientQCLASS.SelectedItem = QCLASS.IN.ToString();
        form.comboBoxClientQTYPE.SelectedItem = QTYPE.ANY.ToString();
      } },
      { ".com name servers", form => {
        form.textBoxClientServer.Text = "a.gtld-servers.net";
        form.radioButtonClientUDP.Checked = true;
        form.checkBoxClientRD.Checked = false;
        form.textBoxClientQNAME.Text = "com.";
        form.comboBoxClientQCLASS.SelectedItem = QCLASS.IN.ToString();
        form.comboBoxClientQTYPE.SelectedItem = QTYPE.NS.ToString();
      } },
      { ".de ANY using TCP", form => {
        form.textBoxClientServer.Text = "a.nic.de";
        form.radioButtonClientTCP.Checked = true;
        form.checkBoxClientRD.Checked = false;
        form.textBoxClientQNAME.Text = "de.";
        form.comboBoxClientQCLASS.SelectedItem = QCLASS.IN.ToString();
        form.comboBoxClientQTYPE.SelectedItem = QTYPE.ANY.ToString();
      } },
      { "Large TXT records (TEL)", form => {
        form.textBoxClientServer.Text = "a0.cth.dns.nic.tel";
        form.radioButtonClientTCP.Checked = true;
        form.checkBoxClientRD.Checked = false;
        form.textBoxClientQNAME.Text = "httpnet.tel.";
        form.comboBoxClientQCLASS.SelectedItem = QCLASS.IN.ToString();
        form.comboBoxClientQTYPE.SelectedItem = QTYPE.TXT.ToString();
      } },
      { "NAPTR records (ENUM)", form => {
        // http://www.denic.de/enum/technik-enum.html
        form.textBoxClientServer.Text = "ns1.denic.de";
        form.radioButtonClientUDP.Checked = true;
        form.checkBoxClientRD.Checked = false;
        form.textBoxClientQNAME.Text = "0.5.3.2.7.2.9.6.9.4.e164.arpa.";
        form.comboBoxClientQCLASS.SelectedItem = QCLASS.IN.ToString();
        form.comboBoxClientQTYPE.SelectedItem = QTYPE.NAPTR.ToString();
      } } 
    };

    #region event handler

    private void Form1_Load(object sender, EventArgs e)
    {
      bindingSourceOPCODE.DataSource = Enum.GetNames(typeof(OPCODE)).AsEnumerable();
      bindingSourceQCLASS.DataSource = Enum.GetNames(typeof(QCLASS)).AsEnumerable();
      bindingSourceQTYPE.DataSource = Enum.GetNames(typeof(QTYPE)).AsEnumerable();

      bindingSourceClientSamples.DataSource = ClientSamples.Keys.AsEnumerable();
      comboBoxClientSamples.SelectedItem = comboBoxClientSamples.Items[0];
      comboBoxClientSamples_SelectedIndexChanged(this, EventArgs.Empty);
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      // disable validators
      AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
      if (!Validate(true))
      {
        AutoValidate = System.Windows.Forms.AutoValidate.Disable;
      }
      errorProvider1.Clear();
    }

    private void comboBoxClientSamples_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxClientSamples.SelectedItem != null)
      {
        string key = comboBoxClientSamples.SelectedItem.ToString();
        ClientSamples[key](this);

        //switch (comboBoxClientSamples.SelectedItem.ToString())
        //{
        //  case "example.com. recursive":
        //    textBoxClientServer.Text = "8.8.8.8";
        //    radioButtonClientUDP.Checked = true;
        //    checkBoxClientRD.Checked = true;
        //    textBoxClientQNAME.Text = "example.com.";
        //    comboBoxClientQCLASS.SelectedItem = DnsClient.DNS.QCLASS.IN.ToString();
        //    comboBoxClientQTYPE.SelectedItem = DnsClient.DNS.QTYPE.ANY.ToString();

        //    break;

        //  case "example.com. non-recorsive":
        //    textBoxClientServer.Text = "a.iana-servers.net";
        //    radioButtonClientUDP.Checked = true;
        //    checkBoxClientRD.Checked = false;
        //    textBoxClientQNAME.Text = "example.com.";
        //    comboBoxClientQCLASS.SelectedItem = DnsClient.DNS.QCLASS.IN.ToString();
        //    comboBoxClientQTYPE.SelectedItem = DnsClient.DNS.QTYPE.ANY.ToString();

        //    break;

        //  case "at. TXT records":
        //    textBoxClientServer.Text = "ns2.univie.ac.at";
        //    radioButtonClientUDP.Checked = true;
        //    checkBoxClientRD.Checked = false;
        //    textBoxClientQNAME.Text = "at.";
        //    comboBoxClientQCLASS.SelectedItem = DnsClient.DNS.QCLASS.IN.ToString();
        //    comboBoxClientQTYPE.SelectedItem = DnsClient.DNS.QTYPE.TXT.ToString();
        //    break;

        //}
      }
    }

    private void buttonClientReset_Click(object sender, EventArgs e)
    {
      textBoxClientServer.Text = "ns.routing.net.";
      textBoxClientQNAME.Text = "ipv1.net.";

      textBoxClientServer.Text = "ns2.univie.ac.at.";
      textBoxClientQNAME.Text = "at.";

      //textBoxClientServer.Focus();
    }

    private void buttonClientSend_Click(object sender, EventArgs e)
    {
      string errorMessage;
      IPAddress serverAddr;
      Cursor = Cursors.WaitCursor;
      buttonClientSend.Enabled = false;

      if (!ValidateServerText(textBoxClientServer.Text, out errorMessage))
      {
        errorProvider1.SetError(textBoxClientServer, errorMessage);
        return;
      }
      try
      {
        // resolve server name by System.Net.Dns
        serverAddr = System.Net.Dns.GetHostAddresses(textBoxClientServer.Text.Trim()).First();
      }
      catch (SocketException)
      {
        errorProvider1.SetError(textBoxClientServer, "Server name cannot be resolved.");
        return;
      }

      try
      {
        IDnsClient client;
        if (radioButtonClientTCP.Checked)
          client = new DnsTcpClient();
        else
          client = new DnsUdpClient();

        Request request = new Request(
          (ushort)DateTime.Now.Ticks,
          checkBoxClientRD.Checked,
          (OPCODE)Enum.Parse(typeof(OPCODE), comboBoxClientOPCODE.SelectedValue.ToString()),
          new Question(
            textBoxClientQNAME.Text.Trim(),
            (QTYPE)Enum.Parse(typeof(QTYPE), comboBoxClientQTYPE.SelectedValue.ToString()),
            (QCLASS)Enum.Parse(typeof(QCLASS), comboBoxClientQCLASS.SelectedValue.ToString())));


        labelClientResponse.Text = String.Empty;
        textBoxClientResponse.Text = String.Empty;

        client.Connect(serverAddr);
        client.BeginProcess(request, new AsyncCallback(OnClientResponseReceived),
          new ClientAsyncState { Client = client, Request = request, Server = serverAddr });

        //try
        //{
        //  client.Connect(serverAddr);
        //  DnsClient.Response response = client.LookUp(request);
        //  labelClientResponse.Text = String.Format("{0}: {1} from [{2}] in {3}ms",
        //    (response.Header.AA ? "Authoritative Response" : "Non-Authoritative Response"),
        //    response.Header.RCODE.ToString(),
        //    serverAddr.ToString(),
        //    response.Timestamp.Subtract(request.Timestamp).TotalMilliseconds);
        //  StringBuilder result = new StringBuilder();
        //  RenderResponse(response, result);
        //  textBoxClientResponse.Text = result.ToString();
        //}
        //finally
        //{
        //  Cursor = Cursors.Default;
        //  buttonClientSend.Enabled = true;
        //}


      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private class ClientAsyncState
    {
      public IDnsClient Client;
      public Request Request;
      public IPAddress Server;
    }

    private static string ShortResponseInfo(ClientAsyncState state, Response response)
    {
      string s = String.Format("{0} bytes from {1} ({2}) in {3}ms: {4}",
        response.Data.Length,
          state.Server.ToString(),
          (state.Client as DnsUdpClient != null ? "UDP" : "TCP"),
          response.Timestamp.Subtract(state.Request.Timestamp).TotalMilliseconds,
                    response.Header.RCODE.ToString()
          );
      if (response.Header.AA)
        s += " (authoritative)";
      return s;
    }

    private void OnClientResponseReceived(IAsyncResult asyncResult)
    {
      try
      {
        ClientAsyncState state = (ClientAsyncState)asyncResult.AsyncState;
        Response response = state.Client.EndProcess(asyncResult);
        SetText(labelClientResponse, ShortResponseInfo(state, response));

        StringBuilder result = new StringBuilder();
        RenderResponse(response, result);
        SetText(textBoxClientResponse, result.ToString());
        //textBoxClientResponse.Text = result.ToString();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        Cursor = Cursors.Default;
        buttonClientSend.Enabled = true;
      }
    }

    private void textBoxClientServer_Validating(object sender, CancelEventArgs e)
    {
      string errorMessage;
      if (!ValidateServerText(textBoxClientServer.Text, out errorMessage))
      {
        errorProvider1.SetError(textBoxClientServer, errorMessage);
        e.Cancel = true;
      }
    }

    private void textBoxClientServer_Validated(object sender, EventArgs e)
    {
      errorProvider1.SetError(textBoxClientServer, "");
    }

    private void textBoxClientQNAME_Validating(object sender, CancelEventArgs e)
    {
      string errorMessage;
      if (!ValidateNameText(textBoxClientQNAME.Text, out errorMessage))
      {
        errorProvider1.SetError(textBoxClientQNAME, errorMessage);
        e.Cancel = true;
      }
    }

    private void textBoxClientQNAME_Validated(object sender, EventArgs e)
    {
      errorProvider1.SetError(textBoxClientQNAME, "");
    }

    #endregion

    #region validation

    private static bool ValidateNameText(string text, out string errorMessage)
    {
      errorMessage = null;

      if (String.IsNullOrEmpty(text))
      {
        errorMessage = "Domain name required.";
        return false;
      }

      // host name
      DnsDomain domain;
      if (DnsDomain.TryParse(text, out domain))
      {
        if (domain.Level >= 1)
          return true;
      }

      errorMessage = "Invalid domain name.";
      return false;
    }

    private static bool ValidateServerText(string text, out string errorMessage)
    {
      errorMessage = null;

      if (String.IsNullOrEmpty(text))
      {
        errorMessage = "Server name or address required.";
        return false;
      }

      // IP address
      IPAddress ipaddr;
      if (IPAddress.TryParse(text, out ipaddr))
      {
        if (ipaddr.AddressFamily == AddressFamily.InterNetwork || ipaddr.AddressFamily == AddressFamily.InterNetworkV6)
          return true;
        errorMessage = "Invalid IP address.";
        return false;
      }

      // host name
      DnsDomain domain;
      if (DnsDomain.TryParse(text, out domain))
      {
        if (domain.Level >= 2)
          return true;
        errorMessage = "Invalid host name.";
        return false;
      }

      errorMessage = "Invalid server name";
      return false;
    }

    #endregion

    #region static

    static void RenderResponse(Response response, StringBuilder sb)
    {

      sb.AppendLine(String.Format(response.Header.ToString()));

      sb.AppendLine(String.Format("Question Section ({0})", response.Questions.Count));
      foreach (Question q in response.Questions)
      {
        sb.AppendLine(String.Format("  {0}", q));
      }

      sb.AppendLine(String.Format("Answer Section ({0})", response.AnswerRecords.Count()));
      foreach (RR rr in response.AnswerRecords)
      {
        sb.AppendLine(String.Format("  {0}", rr));
      }

      sb.AppendLine(String.Format("Authority Section ({0})", response.AuthorityRecords.Count()));
      foreach (RR rr in response.AuthorityRecords)
      {
        sb.AppendLine(String.Format("  {0}", rr));
      }

      sb.AppendLine(String.Format("Additional Records ({0})", response.AdditionalRecords.Count()));
      foreach (RR rr in response.AdditionalRecords)
      {
        sb.AppendLine(String.Format("  {0}", rr));
      }

    }

    #endregion


  }
}
