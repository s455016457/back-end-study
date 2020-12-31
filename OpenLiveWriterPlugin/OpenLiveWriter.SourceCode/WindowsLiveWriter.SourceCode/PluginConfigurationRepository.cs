using System;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsLiveWriter.SourceCode
{
    /// <summary>
    /// The instance of this class is responsible for persistance of <see cref="PluginConfigurationData"/> objects.
    /// It supports two targets - file and string. File persistance is used for persisting configurations between
    /// plugin runs. The string persistance is used to create correct attributes for <code>PRE</code> element saved
    /// into the blog post.
    /// </summary>
    /// <remarks>
    /// The configuration file is located at <code>%AppData%\Windows Live Writer\WindowsLiveWriter.SourceCode.config</code>.
    /// </remarks>
    class PluginConfigurationRepository
    {
        private const string ConfigFile = "WindowsLiveWriter.SourceCode.config";
        private readonly string _configurationFilePath;


        /// <summary>
        /// Default constructor. Initializes a new instance of the <see cref="PluginConfigurationRepository"/> class
        /// with some default conifguration values.
        /// </summary>
        public PluginConfigurationRepository() : this(new PluginConfigurationData())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfigurationRepository"/> class with a provided 
        /// configuration object of type <see cref="PluginConfigurationData"/>.
        /// </summary>
        /// <param name="config">The plugin configuration object.</param>
        public PluginConfigurationRepository(PluginConfigurationData config)
        {
            string folder = string.Format("{0}\\Windows Live Writer", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            _configurationFilePath = string.Format(@"{0}\{1}", folder, ConfigFile);
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            _config = config;
        }

        private PluginConfigurationData _config;
        /// <summary>
        /// Gets or sets the plugin configuration.
        /// </summary>
        /// <value>The plugin configuration.</value>
        public PluginConfigurationData Config
        {
            get { return _config; }
            set { _config = value; }
        }

        /// <summary>
        /// Loads the plugin configuration data from user's configuration file.
        /// </summary>
        /// <remarks>
        /// The configuration file is located at <code>%AppData%\Windows Live Writer\WindowsLiveWriter.SourceCode.config</code>.
        /// </remarks>
        public void LoadPluginConfigurationData()
        {
            if (File.Exists(_configurationFilePath) == false)
            {
                _config = new PluginConfigurationData();
            }

            XmlSerializer serializer = null;
            FileStream stream = null;
            try
            {
                serializer = new XmlSerializer(typeof(PluginConfigurationData));
                stream = new FileStream(_configurationFilePath, FileMode.Open);

                _config = (PluginConfigurationData)serializer.Deserialize(stream);
                stream.Close();
                stream = null;
            }
            #region ExceptionHandling
            catch (Exception ex)
            {
                if (
                    (ex is DirectoryNotFoundException) ||
                    (ex is PathTooLongException) ||
                    (ex is FileNotFoundException) ||
                    (ex is IOException) ||
                    (ex is SecurityException) ||
                    (ex is NotSupportedException) ||
                    (ex is ArgumentOutOfRangeException)
                   )
                {
                    string error = string.Format("Can't load configuration from '{0}' file.\n\nDetails: {1}", _configurationFilePath, ex.Message);
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    throw;
                }
            }
            #endregion //ExceptionHandling
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Saves the plugin configuration data into user's configuration file.
        /// </summary>
        /// <remarks>
        /// The configuration file is located at <code>%AppData%\Windows Live Writer\WindowsLiveWriter.SourceCode.config</code>.
        /// </remarks>
        public void SavePluginConfigurationData()
        {
            XmlSerializer serializer = null;
            TextWriter writer = null;
            try
            {
                serializer = new XmlSerializer(typeof(PluginConfigurationData));
                writer = new StreamWriter(_configurationFilePath);

                serializer.Serialize(writer, _config);
                writer.Close();
                writer = null;
            }
            #region ExceptionHandling
            catch (Exception ex)
            {
                if (
                    (ex is DirectoryNotFoundException) ||
                    (ex is PathTooLongException) ||
                    (ex is IOException) ||
                    (ex is UnauthorizedAccessException) ||
                    (ex is SecurityException)
                   )
                {
                    string error = string.Format("Can't save configuration into '{0}' file.\n\nDetails: {1}", _configurationFilePath, ex.Message);
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    throw;
                }
            }
            #endregion //ExceptionHandling
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }


        /// <summary>
        /// Loads configuration from serialized parameter string.
        /// </summary>
        /// <param name="parameters">Serialized configuration into a string object.</param>
        public void LoadFromParameters(string parameters)
        {
            string valueDelimiter = ":";
            string variableDelimiter = ";";

            string text = parameters.Trim();
            if (text.EndsWith(";") == false)
            {
                text += ";";
            }
            int valueFrom = 0;
            int valueTo = 0;

            bool exitLoop = false;
            while (exitLoop == false)
            {
                valueFrom = text.IndexOf(valueDelimiter);
                valueTo = text.IndexOf(variableDelimiter);

                if ((0 < valueFrom) && (valueFrom < valueTo))
                {
                    string variable = text.Substring(0, valueFrom).Trim();
                    string value = text.Substring(valueFrom + 1, valueTo - valueFrom - 1).Trim();

                    UpdateConfig(_config, variable, value);
                    text = text.Substring(valueTo+1).TrimStart();
                }
                else
                {
                    exitLoop = true;
                }
            }
        }

        /// <summary>
        /// Saves configuration into parameters string. This string will then be used in <code>PRE</code> element.
        /// </summary>
        /// <returns>Serialized configuration into a string object.</returns>
        public string SaveToParameters()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}: {1}; ", "brush", _config.Brush);
            if (!string.IsNullOrEmpty(_config.Highlight))
            {
                builder.AppendFormat("{0}: [{1}]; ", "highlight", _config.Highlight);
            }
            if (_config.UseServerDefaults == false)
            {
                builder.AppendFormat("{0}: {1}; ", "auto-links", _config.AutoLinks.ToString());
                if (!string.IsNullOrEmpty(_config.ClassName))
                {
                    builder.AppendFormat("{0}: {1}; ", "class-name", _config.ClassName);
                }
                builder.AppendFormat("{0}: {1}; ", "collapse", _config.Collapse.ToString());
                builder.AppendFormat("{0}: {1}; ", "first-line", _config.FirstLine);
                builder.AppendFormat("{0}: {1}; ", "gutter", _config.Gutter);
                builder.AppendFormat("{0}: {1}; ", "html-script", _config.HtmlScript.ToString());
                builder.AppendFormat("{0}: {1}; ", "light", _config.Light.ToString());
                builder.AppendFormat("{0}: {1}; ", "ruler", _config.Ruler.ToString());
                builder.AppendFormat("{0}: {1}; ", "smart-tabs", _config.SmartTabs.ToString());
                builder.AppendFormat("{0}: {1}; ", "tab-size", _config.TabSize.ToString());
                builder.AppendFormat("{0}: {1}; ", "toolbar", _config.Toolbar.ToString());
            }
            return builder.ToString().ToLower().Trim();
        }

        private void UpdateConfig(PluginConfigurationData config, string variable, string value)
        {
            switch (variable)
            {
                case "brush":
                    config.Brush = GetString(value);
                    break;
                case "auto-links":
                    config.AutoLinks = GetBoolean(value, config.AutoLinks);
                    break;
                case "class-name":
                    config.ClassName = GetString(value);
                    break;
                case "collapse":
                    config.Collapse = GetBoolean(value, config.Collapse);
                    break;
                case "first-line":
                    config.FirstLine = GetInt(value, config.FirstLine);
                    break;
                case "gutter":
                    config.Gutter = GetBoolean(value, config.Gutter);
                    break;
                case "highlight":
                    int from = value.IndexOf("[");
                    int to = value.LastIndexOf("]");
                    if ((0 <= from) && (from < to))
                        config.Highlight = GetString(value.Substring(from + 1, to - from - 1));
                    break;
                case "html-script":
                    config.HtmlScript = GetBoolean(value, config.HtmlScript);
                    break;
                case "light":
                    config.Light = GetBoolean(value, config.Light);
                    break;
                case "ruler":
                    config.Ruler = GetBoolean(value, config.Ruler);
                    break;
                case "smart-tabs":
                    config.SmartTabs = GetBoolean(value, config.SmartTabs);
                    break;
                case "tab-size":
                    config.TabSize = GetInt(value, config.TabSize);
                    break;
                case "toolbar":
                    config.Toolbar = GetBoolean(value, config.Toolbar);
                    break;
                default:
                    break;
            }
        }

        private bool GetBoolean(string text, bool defaultValue)
        {
            bool booleanValue = false;
            if (bool.TryParse(text, out booleanValue) == false)
            {
                booleanValue = defaultValue;
            }

            return booleanValue;
        }
        private int GetInt(string text, int defaultValue)
        {
            int intValue = 0;
            if (int.TryParse(text, out intValue) == false)
            {
                intValue = defaultValue;
            }

            return intValue;
        }
        private string GetString(string text)
        {
            return text;
        }
    }
}
