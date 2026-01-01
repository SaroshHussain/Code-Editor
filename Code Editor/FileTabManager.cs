using System;
using System.Windows.Forms;

namespace Code_Editor
{
    internal class FileTabManager
    {
        private const string DARK_COLOR = "#2d2d30";
        private const string ACTIVE_COLOR = "#007ACC";
        private const int CLOSE_BTN_WIDTH = 30;
        private const int TAB_HEIGHT = 35;

        public static Panel CreateFileTab(string fileName, Action<string> onFileClick, Action<string, Panel> onCloseClick)
        {
            Panel filePanel = new Panel();
            filePanel.Dock = DockStyle.Top;
            filePanel.Height = TAB_HEIGHT;
            filePanel.Padding = new Padding(0);

            Button fileBtn = CreateFileButton(fileName, onFileClick);

            Button closeBtn = CreateCloseButton(fileName, filePanel, onCloseClick);

            filePanel.Controls.Add(fileBtn);
            filePanel.Controls.Add(closeBtn);

            filePanel.Tag = new { FileName = fileName, FileButton = fileBtn };

            return filePanel;
        }

        private static Button CreateFileButton(string fileName, Action<string> onFileClick)
        {
            Button fileBtn = new Button();
            fileBtn.Text = fileName;
            fileBtn.Dock = DockStyle.Fill;
            fileBtn.Padding = new Padding(10, 5, 10, 5);
            fileBtn.FlatStyle = FlatStyle.Flat;
            fileBtn.FlatAppearance.BorderSize = 0;
            fileBtn.BackColor = ColorTranslator.FromHtml(DARK_COLOR);
            fileBtn.ForeColor = Color.White;
            fileBtn.TextAlign = ContentAlignment.MiddleLeft;
            fileBtn.Click += (s, e) => onFileClick?.Invoke(fileName);

            return fileBtn;
        }

        private static Button CreateCloseButton(string fileName, Panel filePanel, Action<string, Panel> onCloseClick)
        {
            Button closeBtn = new Button();
            closeBtn.Text = "X";
            closeBtn.Dock = DockStyle.Right;
            closeBtn.Width = CLOSE_BTN_WIDTH;
            closeBtn.FlatStyle = FlatStyle.Flat;
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.BackColor = ColorTranslator.FromHtml(DARK_COLOR);
            closeBtn.ForeColor = Color.White;
            closeBtn.Cursor = Cursors.Hand;
            closeBtn.Click += (s, e) => onCloseClick?.Invoke(fileName, filePanel);

            return closeBtn;
        }

        public static void SetTabActive(Button fileBtn)
        {
            fileBtn.BackColor = ColorTranslator.FromHtml(ACTIVE_COLOR);
            fileBtn.ForeColor = Color.White;
        }

        public static void SetTabInactive(Button fileBtn)
        {
            fileBtn.BackColor = ColorTranslator.FromHtml(DARK_COLOR);
            fileBtn.ForeColor = Color.White;
        }

        public static Button GetFileButtonFromTab(Panel filePanel)
        {
            foreach (Control control in filePanel.Controls)
            {
                if (control is Button btn && btn.Text != "X")
                {
                    return btn;
                }
            }
            return null;
        }
    }
}
