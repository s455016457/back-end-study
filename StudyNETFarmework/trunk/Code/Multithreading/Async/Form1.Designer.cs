namespace Async
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSingleThread = new System.Windows.Forms.Button();
            this.btnAsync = new System.Windows.Forms.Button();
            this.btnMultithreading = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnAsync2 = new System.Windows.Forms.Button();
            this.btn_TaskProgress = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_TaskLongRunning = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSingleThread
            // 
            this.btnSingleThread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSingleThread.Location = new System.Drawing.Point(95, 411);
            this.btnSingleThread.Name = "btnSingleThread";
            this.btnSingleThread.Size = new System.Drawing.Size(78, 37);
            this.btnSingleThread.TabIndex = 0;
            this.btnSingleThread.Text = "单线程";
            this.btnSingleThread.UseVisualStyleBackColor = true;
            this.btnSingleThread.Click += new System.EventHandler(this.btnSingleThread_Click);
            // 
            // btnAsync
            // 
            this.btnAsync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAsync.Location = new System.Drawing.Point(179, 411);
            this.btnAsync.Name = "btnAsync";
            this.btnAsync.Size = new System.Drawing.Size(73, 37);
            this.btnAsync.TabIndex = 1;
            this.btnAsync.Text = "异步调用";
            this.btnAsync.UseVisualStyleBackColor = true;
            this.btnAsync.Click += new System.EventHandler(this.btnAsync_Click);
            // 
            // btnMultithreading
            // 
            this.btnMultithreading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMultithreading.Location = new System.Drawing.Point(337, 411);
            this.btnMultithreading.Name = "btnMultithreading";
            this.btnMultithreading.Size = new System.Drawing.Size(78, 37);
            this.btnMultithreading.TabIndex = 0;
            this.btnMultithreading.Text = "多线程";
            this.btnMultithreading.UseVisualStyleBackColor = true;
            this.btnMultithreading.Click += new System.EventHandler(this.btnMultithreading_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(643, 399);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // btnAsync2
            // 
            this.btnAsync2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAsync2.Location = new System.Drawing.Point(258, 411);
            this.btnAsync2.Name = "btnAsync2";
            this.btnAsync2.Size = new System.Drawing.Size(73, 37);
            this.btnAsync2.TabIndex = 1;
            this.btnAsync2.Text = "异步调用2";
            this.btnAsync2.UseVisualStyleBackColor = true;
            this.btnAsync2.Click += new System.EventHandler(this.btnAsync2_ClickAsync);
            // 
            // btn_TaskProgress
            // 
            this.btn_TaskProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_TaskProgress.Location = new System.Drawing.Point(421, 411);
            this.btn_TaskProgress.Name = "btn_TaskProgress";
            this.btn_TaskProgress.Size = new System.Drawing.Size(78, 37);
            this.btn_TaskProgress.TabIndex = 3;
            this.btn_TaskProgress.Text = "TAP进度";
            this.btn_TaskProgress.UseVisualStyleBackColor = true;
            this.btn_TaskProgress.Click += new System.EventHandler(this.btn_TaskProgress_ClickAsync);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Clear.Location = new System.Drawing.Point(12, 411);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(78, 37);
            this.btn_Clear.TabIndex = 4;
            this.btn_Clear.Text = "清空记录";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_TaskLongRunning
            // 
            this.btn_TaskLongRunning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_TaskLongRunning.Location = new System.Drawing.Point(505, 411);
            this.btn_TaskLongRunning.Name = "btn_TaskLongRunning";
            this.btn_TaskLongRunning.Size = new System.Drawing.Size(78, 37);
            this.btn_TaskLongRunning.TabIndex = 5;
            this.btn_TaskLongRunning.Text = "TaskLongRunning";
            this.btn_TaskLongRunning.UseVisualStyleBackColor = true;
            this.btn_TaskLongRunning.Click += new System.EventHandler(this.btn_TaskLongRunning_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 460);
            this.Controls.Add(this.btn_TaskLongRunning);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_TaskProgress);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnAsync2);
            this.Controls.Add(this.btnAsync);
            this.Controls.Add(this.btnMultithreading);
            this.Controls.Add(this.btnSingleThread);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSingleThread;
        private System.Windows.Forms.Button btnAsync;
        private System.Windows.Forms.Button btnMultithreading;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnAsync2;
        private System.Windows.Forms.Button btn_TaskProgress;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button btn_TaskLongRunning;
    }
}

