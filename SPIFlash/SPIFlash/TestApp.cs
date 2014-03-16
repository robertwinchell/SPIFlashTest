#region Using
// Imported namespaces (System)
using System;
using System.Windows.Forms;
#endregion
namespace SPIFlash
{
    #region TestApp
    /// <summary>Sample application demonstrating various methods for reading/writing different types of data to Serial Flash memory.</summary>
    /// <remarks>Developed by Ta0 Software (http://www.ta0soft.com/)</remarks>
    static class TestApp
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());
        }
    }
    #endregion
}