/*
 * File: Boethin.Net.DnsTools.Tests.ClientApp/Form1.Designer.cs
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

﻿namespace Boethin.Net.DnsTools.Tests.ConsoleResolver
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.labelClientResponse = new System.Windows.Forms.Label();
      this.textBoxClientResponse = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.comboBoxClientQTYPE = new System.Windows.Forms.ComboBox();
      this.bindingSourceQTYPE = new System.Windows.Forms.BindingSource(this.components);
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.comboBoxClientQCLASS = new System.Windows.Forms.ComboBox();
      this.bindingSourceQCLASS = new System.Windows.Forms.BindingSource(this.components);
      this.textBoxClientQNAME = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.radioButtonClientTCP = new System.Windows.Forms.RadioButton();
      this.radioButtonClientUDP = new System.Windows.Forms.RadioButton();
      this.checkBoxClientRD = new System.Windows.Forms.CheckBox();
      this.comboBoxClientOPCODE = new System.Windows.Forms.ComboBox();
      this.bindingSourceOPCODE = new System.Windows.Forms.BindingSource(this.components);
      this.label5 = new System.Windows.Forms.Label();
      this.buttonClientSend = new System.Windows.Forms.Button();
      this.textBoxClientServer = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.label6 = new System.Windows.Forms.Label();
      this.comboBoxClientSamples = new System.Windows.Forms.ComboBox();
      this.bindingSourceClientSamples = new System.Windows.Forms.BindingSource(this.components);
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceQTYPE)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceQCLASS)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceOPCODE)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceClientSamples)).BeginInit();
      this.SuspendLayout();
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabControl1.Location = new System.Drawing.Point(13, 13);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(592, 471);
      this.tabControl1.TabIndex = 0;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.groupBox3);
      this.tabPage1.Controls.Add(this.groupBox1);
      this.tabPage1.Location = new System.Drawing.Point(4, 23);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(584, 444);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Client";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.labelClientResponse);
      this.groupBox3.Controls.Add(this.textBoxClientResponse);
      this.groupBox3.Location = new System.Drawing.Point(15, 229);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(555, 210);
      this.groupBox3.TabIndex = 1;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Response";
      // 
      // labelClientResponse
      // 
      this.labelClientResponse.AutoSize = true;
      this.labelClientResponse.Location = new System.Drawing.Point(7, 19);
      this.labelClientResponse.Name = "labelClientResponse";
      this.labelClientResponse.Size = new System.Drawing.Size(0, 14);
      this.labelClientResponse.TabIndex = 0;
      // 
      // textBoxClientResponse
      // 
      this.textBoxClientResponse.Location = new System.Drawing.Point(7, 38);
      this.textBoxClientResponse.Multiline = true;
      this.textBoxClientResponse.Name = "textBoxClientResponse";
      this.textBoxClientResponse.ReadOnly = true;
      this.textBoxClientResponse.Size = new System.Drawing.Size(533, 166);
      this.textBoxClientResponse.TabIndex = 1;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.comboBoxClientSamples);
      this.groupBox1.Controls.Add(this.label6);
      this.groupBox1.Controls.Add(this.groupBox2);
      this.groupBox1.Controls.Add(this.radioButtonClientTCP);
      this.groupBox1.Controls.Add(this.radioButtonClientUDP);
      this.groupBox1.Controls.Add(this.checkBoxClientRD);
      this.groupBox1.Controls.Add(this.comboBoxClientOPCODE);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.buttonClientSend);
      this.groupBox1.Controls.Add(this.textBoxClientServer);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Location = new System.Drawing.Point(15, 16);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(555, 196);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Request";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.comboBoxClientQTYPE);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.comboBoxClientQCLASS);
      this.groupBox2.Controls.Add(this.textBoxClientQNAME);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Location = new System.Drawing.Point(22, 79);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(518, 73);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Question";
      // 
      // comboBoxClientQTYPE
      // 
      this.comboBoxClientQTYPE.DataSource = this.bindingSourceQTYPE;
      this.comboBoxClientQTYPE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxClientQTYPE.FormattingEnabled = true;
      this.comboBoxClientQTYPE.Location = new System.Drawing.Point(269, 41);
      this.comboBoxClientQTYPE.Name = "comboBoxClientQTYPE";
      this.comboBoxClientQTYPE.Size = new System.Drawing.Size(95, 22);
      this.comboBoxClientQTYPE.TabIndex = 5;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(217, 45);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(42, 14);
      this.label4.TabIndex = 4;
      this.label4.Text = "Type:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(20, 45);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(49, 14);
      this.label3.TabIndex = 2;
      this.label3.Text = "Class:";
      // 
      // comboBoxClientQCLASS
      // 
      this.comboBoxClientQCLASS.DataSource = this.bindingSourceQCLASS;
      this.comboBoxClientQCLASS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxClientQCLASS.FormattingEnabled = true;
      this.comboBoxClientQCLASS.Location = new System.Drawing.Point(80, 41);
      this.comboBoxClientQCLASS.Name = "comboBoxClientQCLASS";
      this.comboBoxClientQCLASS.Size = new System.Drawing.Size(95, 22);
      this.comboBoxClientQCLASS.TabIndex = 3;
      // 
      // textBoxClientQNAME
      // 
      this.textBoxClientQNAME.Location = new System.Drawing.Point(80, 19);
      this.textBoxClientQNAME.Name = "textBoxClientQNAME";
      this.textBoxClientQNAME.Size = new System.Drawing.Size(413, 20);
      this.textBoxClientQNAME.TabIndex = 1;
      this.textBoxClientQNAME.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxClientQNAME_Validating);
      this.textBoxClientQNAME.Validated += new System.EventHandler(this.textBoxClientQNAME_Validated);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(19, 23);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(49, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Name::";
      // 
      // radioButtonClientTCP
      // 
      this.radioButtonClientTCP.AutoSize = true;
      this.radioButtonClientTCP.Location = new System.Drawing.Point(494, 19);
      this.radioButtonClientTCP.Name = "radioButtonClientTCP";
      this.radioButtonClientTCP.Size = new System.Drawing.Size(46, 18);
      this.radioButtonClientTCP.TabIndex = 3;
      this.radioButtonClientTCP.Text = "TCP";
      this.radioButtonClientTCP.UseVisualStyleBackColor = true;
      // 
      // radioButtonClientUDP
      // 
      this.radioButtonClientUDP.AutoSize = true;
      this.radioButtonClientUDP.Checked = true;
      this.radioButtonClientUDP.Location = new System.Drawing.Point(440, 19);
      this.radioButtonClientUDP.Name = "radioButtonClientUDP";
      this.radioButtonClientUDP.Size = new System.Drawing.Size(46, 18);
      this.radioButtonClientUDP.TabIndex = 2;
      this.radioButtonClientUDP.TabStop = true;
      this.radioButtonClientUDP.Text = "UDP";
      this.radioButtonClientUDP.UseVisualStyleBackColor = true;
      // 
      // checkBoxClientRD
      // 
      this.checkBoxClientRD.AutoSize = true;
      this.checkBoxClientRD.Location = new System.Drawing.Point(242, 50);
      this.checkBoxClientRD.Name = "checkBoxClientRD";
      this.checkBoxClientRD.Size = new System.Drawing.Size(145, 18);
      this.checkBoxClientRD.TabIndex = 6;
      this.checkBoxClientRD.Text = "Recursion Desired";
      this.checkBoxClientRD.UseVisualStyleBackColor = true;
      // 
      // comboBoxClientOPCODE
      // 
      this.comboBoxClientOPCODE.DataSource = this.bindingSourceOPCODE;
      this.comboBoxClientOPCODE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxClientOPCODE.FormattingEnabled = true;
      this.comboBoxClientOPCODE.Location = new System.Drawing.Point(103, 46);
      this.comboBoxClientOPCODE.Name = "comboBoxClientOPCODE";
      this.comboBoxClientOPCODE.Size = new System.Drawing.Size(95, 22);
      this.comboBoxClientOPCODE.TabIndex = 5;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(25, 50);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(56, 14);
      this.label5.TabIndex = 4;
      this.label5.Text = "Opcode:";
      // 
      // buttonClientSend
      // 
      this.buttonClientSend.Location = new System.Drawing.Point(465, 162);
      this.buttonClientSend.Name = "buttonClientSend";
      this.buttonClientSend.Size = new System.Drawing.Size(75, 23);
      this.buttonClientSend.TabIndex = 9;
      this.buttonClientSend.Text = "Send";
      this.buttonClientSend.UseVisualStyleBackColor = true;
      this.buttonClientSend.Click += new System.EventHandler(this.buttonClientSend_Click);
      // 
      // textBoxClientServer
      // 
      this.textBoxClientServer.Location = new System.Drawing.Point(103, 19);
      this.textBoxClientServer.MaxLength = 255;
      this.textBoxClientServer.Name = "textBoxClientServer";
      this.textBoxClientServer.Size = new System.Drawing.Size(297, 20);
      this.textBoxClientServer.TabIndex = 1;
      this.textBoxClientServer.WordWrap = false;
      this.textBoxClientServer.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxClientServer_Validating);
      this.textBoxClientServer.Validated += new System.EventHandler(this.textBoxClientServer_Validated);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(25, 23);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(56, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Server:";
      // 
      // tabPage2
      // 
      this.tabPage2.Location = new System.Drawing.Point(4, 23);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(584, 444);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "tabPage2";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(25, 166);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(63, 14);
      this.label6.TabIndex = 10;
      this.label6.Text = "Samples:";
      // 
      // comboBoxClientSamples
      // 
      this.comboBoxClientSamples.DataSource = this.bindingSourceClientSamples;
      this.comboBoxClientSamples.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxClientSamples.FormattingEnabled = true;
      this.comboBoxClientSamples.Location = new System.Drawing.Point(102, 162);
      this.comboBoxClientSamples.Name = "comboBoxClientSamples";
      this.comboBoxClientSamples.Size = new System.Drawing.Size(298, 22);
      this.comboBoxClientSamples.TabIndex = 11;
      this.comboBoxClientSamples.SelectedIndexChanged += new System.EventHandler(this.comboBoxClientSamples_SelectedIndexChanged);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(612, 496);
      this.Controls.Add(this.tabControl1);
      this.Name = "Form1";
      this.Text = "HttpNet.DnsTools.DemoApp";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceQTYPE)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceQCLASS)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceOPCODE)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceClientSamples)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox textBoxClientServer;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button buttonClientSend;
    private System.Windows.Forms.BindingSource bindingSourceQCLASS;
    private System.Windows.Forms.BindingSource bindingSourceQTYPE;
    private System.Windows.Forms.ComboBox comboBoxClientOPCODE;
    private System.Windows.Forms.BindingSource bindingSourceOPCODE;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.CheckBox checkBoxClientRD;
    private System.Windows.Forms.RadioButton radioButtonClientTCP;
    private System.Windows.Forms.RadioButton radioButtonClientUDP;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.ComboBox comboBoxClientQTYPE;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboBoxClientQCLASS;
    private System.Windows.Forms.TextBox textBoxClientQNAME;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TextBox textBoxClientResponse;
    private System.Windows.Forms.ErrorProvider errorProvider1;
    private System.Windows.Forms.Label labelClientResponse;
    private System.Windows.Forms.ComboBox comboBoxClientSamples;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.BindingSource bindingSourceClientSamples;
  }
}

