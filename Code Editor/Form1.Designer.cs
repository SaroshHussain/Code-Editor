namespace Code_Editor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            editor = new Microsoft.Web.WebView2.WinForms.WebView2();
            code_queue = new ListBox();
            code_output = new RichTextBox();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            followToolStripMenuItem = new ToolStripMenuItem();
            run_code = new ToolStripMenuItem();
            crownMenuStrip1 = new ReaLTaiizor.Controls.CrownMenuStrip();
            saveToMemoryToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)editor).BeginInit();
            crownMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 76);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = Color.FromArgb(32, 32, 32);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1125, 470);
            splitContainer1.SplitterDistance = 192;
            splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.BackColor = Color.FromArgb(64, 64, 64);
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Margin = new Padding(3, 2, 3, 2);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(editor);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.BackColor = Color.FromArgb(32, 32, 32);
            splitContainer2.Panel2.Controls.Add(code_queue);
            splitContainer2.Panel2.Controls.Add(code_output);
            splitContainer2.Size = new Size(929, 470);
            splitContainer2.SplitterDistance = 607;
            splitContainer2.TabIndex = 0;
            // 
            // editor
            // 
            editor.AllowExternalDrop = true;
            editor.BackColor = SystemColors.Control;
            editor.CreationProperties = null;
            editor.DefaultBackgroundColor = Color.White;
            editor.Dock = DockStyle.Fill;
            editor.Location = new Point(0, 0);
            editor.Margin = new Padding(0);
            editor.Name = "editor";
            editor.Size = new Size(607, 470);
            editor.TabIndex = 0;
            editor.ZoomFactor = 1D;
            // 
            // code_queue
            // 
            code_queue.BackColor = SystemColors.WindowFrame;
            code_queue.Dock = DockStyle.Bottom;
            code_queue.FormattingEnabled = true;
            code_queue.Location = new Point(0, 206);
            code_queue.Name = "code_queue";
            code_queue.Size = new Size(318, 264);
            code_queue.TabIndex = 1;
            code_queue.SelectedIndexChanged += code_queue_SelectedIndexChanged;
            // 
            // code_output
            // 
            code_output.BackColor = Color.FromArgb(32, 32, 32);
            code_output.Dock = DockStyle.Fill;
            code_output.ForeColor = SystemColors.Info;
            code_output.Location = new Point(0, 0);
            code_output.Margin = new Padding(3, 2, 3, 2);
            code_output.Name = "code_output";
            code_output.Size = new Size(318, 470);
            code_output.TabIndex = 0;
            code_output.Text = "";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            openToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(181, 26);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            saveToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(181, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            exitToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(181, 26);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            editToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(49, 24);
            editToolStripMenuItem.Text = "Edit";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            helpToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // followToolStripMenuItem
            // 
            followToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            followToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            followToolStripMenuItem.Name = "followToolStripMenuItem";
            followToolStripMenuItem.Size = new Size(67, 24);
            followToolStripMenuItem.Text = "Follow";
            // 
            // run_code
            // 
            run_code.BackColor = Color.FromArgb(60, 63, 65);
            run_code.ForeColor = Color.FromArgb(220, 220, 220);
            run_code.Name = "run_code";
            run_code.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F5;
            run_code.Size = new Size(48, 24);
            run_code.Text = "Run";
            run_code.Click += run_code_Click;
            // 
            // crownMenuStrip1
            // 
            crownMenuStrip1.BackColor = Color.Black;
            crownMenuStrip1.ForeColor = Color.FromArgb(220, 220, 220);
            crownMenuStrip1.ImageScalingSize = new Size(20, 20);
            crownMenuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, helpToolStripMenuItem, followToolStripMenuItem, run_code, saveToMemoryToolStripMenuItem });
            crownMenuStrip1.Location = new Point(3, 48);
            crownMenuStrip1.Name = "crownMenuStrip1";
            crownMenuStrip1.Padding = new Padding(3, 2, 0, 2);
            crownMenuStrip1.Size = new Size(1125, 28);
            crownMenuStrip1.TabIndex = 1;
            crownMenuStrip1.Text = "crownMenuStrip1";
            // 
            // saveToMemoryToolStripMenuItem
            // 
            saveToMemoryToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            saveToMemoryToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            saveToMemoryToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            saveToMemoryToolStripMenuItem.Name = "saveToMemoryToolStripMenuItem";
            saveToMemoryToolStripMenuItem.Size = new Size(131, 24);
            saveToMemoryToolStripMenuItem.Text = "Save to Memory";
            saveToMemoryToolStripMenuItem.Click += saveToMemoryToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(1131, 548);
            Controls.Add(splitContainer1);
            Controls.Add(crownMenuStrip1);
            Font = new Font("Segoe UI", 9F);
            MainMenuStrip = crownMenuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Padding = new Padding(3, 48, 3, 2);
            Text = "Code Editor";
            Load += Form1_Load;
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)editor).EndInit();
            crownMenuStrip1.ResumeLayout(false);
            crownMenuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Microsoft.Web.WebView2.WinForms.WebView2 editor;
        private RichTextBox code_output;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem followToolStripMenuItem;
        private ToolStripMenuItem run_code;
        private ReaLTaiizor.Controls.CrownMenuStrip crownMenuStrip1;
        private ListBox code_queue;
        private ToolStripMenuItem saveToMemoryToolStripMenuItem;
    }
}
