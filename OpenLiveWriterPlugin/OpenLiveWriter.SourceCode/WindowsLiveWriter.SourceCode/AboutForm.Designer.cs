namespace WindowsLiveWriter.SourceCode
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.groupBoxPlugin = new System.Windows.Forms.GroupBox();
            this.linkDavidPokluda = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkHighlighter = new System.Windows.Forms.LinkLabel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBoxPlugin.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPlugin
            // 
            this.groupBoxPlugin.Controls.Add(this.linkDavidPokluda);
            this.groupBoxPlugin.Controls.Add(this.textBox1);
            this.groupBoxPlugin.Location = new System.Drawing.Point(13, 13);
            this.groupBoxPlugin.Name = "groupBoxPlugin";
            this.groupBoxPlugin.Size = new System.Drawing.Size(351, 82);
            this.groupBoxPlugin.TabIndex = 1;
            this.groupBoxPlugin.TabStop = false;
            this.groupBoxPlugin.Text = "Plug-in author";
            // 
            // linkDavidPokluda
            // 
            this.linkDavidPokluda.AutoSize = true;
            this.linkDavidPokluda.Location = new System.Drawing.Point(15, 59);
            this.linkDavidPokluda.Name = "linkDavidPokluda";
            this.linkDavidPokluda.Size = new System.Drawing.Size(122, 13);
            this.linkDavidPokluda.TabIndex = 1;
            this.linkDavidPokluda.TabStop = true;
            this.linkDavidPokluda.Text = "http://blog.pokluda.com";
            this.linkDavidPokluda.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDavidPokluda_LinkClicked);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(15, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(330, 33);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "This plug-in for Windows Live Writer was created by David Pokluda. More projects " +
                "and information are available at:";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(383, 13);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkHighlighter);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Location = new System.Drawing.Point(13, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 82);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Syntax highlighter author";
            // 
            // linkHighlighter
            // 
            this.linkHighlighter.AutoSize = true;
            this.linkHighlighter.Location = new System.Drawing.Point(15, 59);
            this.linkHighlighter.Name = "linkHighlighter";
            this.linkHighlighter.Size = new System.Drawing.Size(244, 13);
            this.linkHighlighter.TabIndex = 1;
            this.linkHighlighter.TabStop = true;
            this.linkHighlighter.Text = "http://alexgorbatchev.com/wiki/SyntaxHighlighter";
            this.linkHighlighter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHighlighter_LinkClicked);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(15, 19);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(330, 33);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "This plug-in expects syntax highlighter from Alex Gorbatchev. More information av" +
                "ailable at:";
            // 
            // AboutForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 209);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxPlugin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AboutForm";
            this.Text = "About information";
            this.groupBoxPlugin.ResumeLayout(false);
            this.groupBoxPlugin.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPlugin;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.LinkLabel linkDavidPokluda;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel linkHighlighter;
        private System.Windows.Forms.TextBox textBox2;
    }
}