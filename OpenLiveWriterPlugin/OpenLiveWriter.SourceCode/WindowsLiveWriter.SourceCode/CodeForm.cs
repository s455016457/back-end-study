using System;
using System.Drawing;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace WindowsLiveWriter.SourceCode
{
    /// <summary>
    /// This is the main plugin form for authoring source code.
    /// </summary>
    public partial class CodeForm : Form
    {
        private PluginConfigurationRepository _configDb = new PluginConfigurationRepository();
        private string _code;

        private const string prePrefix = "<pre class=\"";
        private const string preSuffix = "\">";
        private const string preClose = "</pre>";

        /// <summary>
        /// Gets or sets the source code. Setter will recognize if the selection contains surrounding <code>PRE</code>
        /// element and will parse the configuration and the source code correctly.
        /// </summary>
        /// <value>The source code.</value>
        public string Code 
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0}{1}{2}", prePrefix, _configDb.SaveToParameters(), preSuffix);
                builder.Append(HttpUtility.HtmlEncode(_code));
                builder.Append(preClose);
                return builder.ToString();
            }
            set
            {
                int preFrom = value.IndexOf(prePrefix);
                int fromPos = value.IndexOf(preSuffix);
                int toPos = value.LastIndexOf(preClose);
                if ((0 <= preFrom) && (preFrom < fromPos) && (fromPos < toPos))
                {
                    preFrom += prePrefix.Length;
                    string parameters = value.Substring(preFrom, fromPos - preFrom - 1);
                    _configDb.LoadFromParameters(parameters);
                    fromPos += preSuffix.Length;
                    _code = value.Substring(fromPos, toPos - fromPos);
                }
                else if (string.IsNullOrEmpty(value) == false)
                {
                    _code = value;
                }
                else if (_configDb.Config.LoadFromClipboard)
                {
                    _code = Clipboard.GetText();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeForm"/> class.
        /// </summary>
        /// <param name="code">The source code. It will recognize if the selection contains surrounding <code>PRE</code>
        /// element and will parse the configuration and the source code correctly.</param>
        public CodeForm(string code)
        {
            _configDb.LoadPluginConfigurationData();
            Code = code;
            InitializeComponent();
            LoadBrushes();

            comboBrush.Text = _configDb.Config.Brush;
            numericFirstLine.Value = _configDb.Config.FirstLine;
            textHighlight.Text = _configDb.Config.Highlight;
            textCode.Text = _code;
            textCode.Font = new Font(_configDb.Config.CodeFontFamily, _configDb.Config.CodeFontSize);

            if (_configDb.Config.MainFormHeight > 0 && _configDb.Config.MainFormWidth > 0)
            {
                this.Left = _configDb.Config.MainFormX;
                this.Top = _configDb.Config.MainFormY;
                this.Width = _configDb.Config.MainFormWidth;
                this.Height = _configDb.Config.MainFormHeight;
                this.StartPosition = FormStartPosition.Manual;
            }
        }

        private void LoadBrushes()
        {
            comboBrush.Items.Clear();
            comboBrush.Items.Add("csharp");
            comboBrush.Items.Add("bash");
            comboBrush.Items.Add("shell");
            comboBrush.Items.Add("cpp");
            comboBrush.Items.Add("css");
            comboBrush.Items.Add("pas");
            comboBrush.Items.Add("patch");
            comboBrush.Items.Add("groovy");
            comboBrush.Items.Add("xml");
            comboBrush.Items.Add("js");
            comboBrush.Items.Add("java");
            comboBrush.Items.Add("php");
            comboBrush.Items.Add("text");
            comboBrush.Items.Add("py");
            comboBrush.Items.Add("rails");
            comboBrush.Items.Add("bash");
            comboBrush.Items.Add("sql");
            comboBrush.Items.Add("vb");
            comboBrush.Items.Add("powershell");
            comboBrush.Items.Add("fsharp");
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            using (AboutForm form = new AboutForm())
            {
                form.ShowDialog();
            }
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            using (OptionsForm form = new OptionsForm(_configDb.Config))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _configDb.Config = form.PluginConfigurationData;
                    _configDb.SavePluginConfigurationData();
                    textCode.Font = new Font(_configDb.Config.CodeFontFamily, _configDb.Config.CodeFontSize);
                    textCode.Update();
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            _configDb.Config.Brush = comboBrush.Text;
            _configDb.Config.FirstLine = (int) numericFirstLine.Value;
            _configDb.Config.Highlight = textHighlight.Text;
            _configDb.Config.MainFormX = this.Left;
            _configDb.Config.MainFormY = this.Top;
            _configDb.Config.MainFormWidth = this.Width;
            _configDb.Config.MainFormHeight = this.Height;

            _configDb.SavePluginConfigurationData();
            _code = textCode.Text;

            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }
    }
}