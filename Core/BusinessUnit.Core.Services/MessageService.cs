using System.Composition;
using System.Windows.Forms;

namespace BusinessUnit.Core.Services
{
    [Export(typeof(IMessageService))]
    public class MessageService:IMessageService
    {

        public DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            var result = MessageBox.Show(text, caption, buttons, icon, defaultButton);
            return result;
        }

    }
}
