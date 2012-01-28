using System;
using System.Windows.Forms;

namespace genotank
{
    class Program {
        [STAThread]
        static void Main() {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Report());
        }
    }

}