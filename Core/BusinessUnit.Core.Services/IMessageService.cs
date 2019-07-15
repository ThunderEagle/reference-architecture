using System.Windows.Forms;

namespace BusinessUnit.Core.Services {
    public interface IMessageService {
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
    }
}