using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GhostCoder.Properties;

namespace GhostCoder.Menu
{
    public class ProcessIcon : IDisposable
    {
        private readonly NotifyIcon _icon;

        public ProcessIcon()
        {
            _icon = new NotifyIcon();
        }

        public void Display()
        {
            _icon.Icon = Resources.GhostIcon;
            _icon.Text = "Ghost Coder";
            _icon.Visible = true;
        }

        public void Dispose()
        {
            _icon.Dispose();
        }
    }
}
