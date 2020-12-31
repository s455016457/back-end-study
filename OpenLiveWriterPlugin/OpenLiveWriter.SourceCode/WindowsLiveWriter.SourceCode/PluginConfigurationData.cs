using System;

namespace WindowsLiveWriter.SourceCode
{
    /// <summary>
    /// Configuration class. Instance of this class will contain all the plugin configuration.
    /// For configuration persistance see <see cref="PluginConfigurationRepository"/> class.
    /// </summary>
    [Serializable]
    public class PluginConfigurationData
    {

        /// <summary>
        /// Default constructor. Initializes a new instance of the <see cref="PluginConfigurationData"/> class.
        /// </summary>
        public PluginConfigurationData()
        {
            //Options form
            UseServerDefaults = true;
            CodeFontFamily = "Microsoft Sans Serif";
            CodeFontSize = 8.25f;

            MainFormX = 0;
            MainFormY = 0;
            MainFormWidth = 0;
            MainFormHeight = 0;

            AutoLinks = true;
            ClassName = "";
            Collapse = false;
            Gutter = true;
            HtmlScript = false;
            Light = false;
            Ruler = false;
            SmartTabs = true;
            TabSize = 4;
            Toolbar = true;
            LoadFromClipboard = true;

            //Regular form
            Brush = "csharp";
            FirstLine = 1;
            Highlight = string.Empty;
        }

        public bool LoadFromClipboard { get; set; }
        public string CodeFontFamily { get; set; }
        public float CodeFontSize { get; set; }

        public int MainFormX { get; set; }
        public int MainFormY { get; set; }
        public int MainFormWidth { get; set; }
        public int MainFormHeight { get; set; }

        public bool UseServerDefaults { get; set; }
        public bool AutoLinks { get; set; }
        public string ClassName { get; set; }
        public bool Collapse { get; set; }
        public bool Gutter { get; set; }
        public bool HtmlScript { get; set; }
        public bool Light { get; set; }
        public bool Ruler { get; set; }
        public bool SmartTabs { get; set; }
        public int TabSize { get; set; }
        public bool Toolbar { get; set; }

        public string Brush { get; set; }
        public int FirstLine { get; set; }
        public string Highlight { get; set; }
    }
}