using System.Windows.Forms;
using OpenLiveWriter.Api;

namespace WindowsLiveWriter.SourceCode
{
    /// <summary>
    /// Main class for the plugin. It registers the plugin and overrides the <see cref="CreateContent"/> method.
    /// </summary>
    //[InsertableContentSource("Source code plug-in", SidebarText = "Source code"), WriterPlugin("2506BDC0-6146-4f4d-8A9E-FE6A3965A07A", "Source code plug-in", ImagePath = "Resources.code.png", Description = "SourceCodePlugin is a plug-in to Windows Live Writer to enable code syntax highlighting using Alex Gorbatchev syntax highlighter.", PublisherUrl = "http://blog.pokluda.com")]
    [InsertableContentSource("Source code plug-in", SidebarText = "Source code"), WriterPlugin("2506BDC0-6146-4f4d-8A9E-FE6A3965A07A", "Source code plug-in", ImagePath = "Resources.code.png", Description = "博客园中推荐的插入代码", PublisherUrl = "http://blog.pokluda.com")]
    public class SourceCodePlugin : ContentSource
    {
        /// <summary>
        /// Create content using an Insert dialog box.
        /// </summary>
        /// <param name="dialogOwner">Owner for any dialog boxes shown.</param>
        /// <param name="content">Newly created content. If there is an existing contiguous selection within the editor when CreateContent is called, 
        /// the parameter will contain the contents of the selection by default.</param>
        /// <returns><see cref="DialogResult.OK"/> if content was successfully created, <see cref="DialogResult.Cancel"/> if the user cancels the Insert dialog.</returns>
        public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content)
        {
            DialogResult result = DialogResult.Cancel;

            using (CodeForm form = new CodeForm(content))
            {
                result = form.ShowDialog(dialogOwner);
                if (result == DialogResult.OK)
                {
                    content = form.Code;
                }
            }

            return result;
        }
    }
}