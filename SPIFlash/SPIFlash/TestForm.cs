#region Using
// Imported namespaces (System)
using System;
using System.Text;
using System.Windows.Forms;
#endregion
namespace SPIFlash
{
    #region TestForm
    /// <summary>Sample application demonstrating various methods for reading/writing different types of data to Serial Flash memory.</summary>
    public partial class TestForm : Form
    {
        #region Objects
        /// <summary>Stores an instance of a SPIFlash object in memory for as long as this Form is open.</summary>
        private SPIFlash spiFlash;
        #endregion
        #region Constructor
        /// <summary>Initializes a new instance of the TestForm class.</summary>
        public TestForm()
        {
            InitializeComponent();
        }
        #endregion
        #region TestForm Events
        /// <summary>Occurs as soon as the Form loads.</summary>
        private void TestForm_Load(object sender, EventArgs e)
        {
            // This is where the magic happens. Simply create a new SPIFlash object and you can begin reading and writing data to the device.
            // You may want to store this object in the private scope like I've done in this example, so the connection stays alive for as long as this Form is open.
            // A try/catch block is used to catch any exception errors that may occur, in this case it is used to detect if any DLN-series adapters are installed on the system.
            // If no adapters are found, no connection will be made and the GUI controls will remain disabled, preventing the user from reading or writing any data.
            try
            {
                // Create a new instance of the SPIFlash class and store it in memory
                // Note: You can also specify a custom port frequency and frame size:
                // spiFlash = new SPIFlash(frequency, frameSize);
                spiFlash = new SPIFlash();

                // Notify the user that the connection has been established and enable controls
                UpdateControls("Connected, SPI Flash ID: " + spiFlash.ID.ToString(), true);
            }
            catch (Exception ex)
            {
                // An error has occured, disable controls and display it to the user
                UpdateControls(ex.Message, false);
            }
        }
        #endregion
        #region Button Events
        /// <summary>Occurs when the Read Boolean button is clicked.</summary>
        private void btnReadBoolean_Click(object sender, EventArgs e)
        {
            // Call the ReadBoolean function and store the data retrieved in the result variable
            bool result = spiFlash.ReadBoolean(Convert.ToInt32(numAddress.Value));

            // Display the retrieved data to the user
            statusBar.Text = "Boolean Read: " + result.ToString();
        }

        /// <summary>Occurs when the Read Char button is clicked.</summary>
        private void btnReadChar_Click(object sender, EventArgs e)
        {
            // Call the ReadChar function and store the data retrieved in the result variable
            char result = spiFlash.ReadChar(Convert.ToInt32(numAddress.Value));

            // Display the retrieved data to the user
            statusBar.Text = "Char Read: " + result.ToString();
        }

        /// <summary>Occurs when the Read Integer button is clicked.</summary>
        private void btnReadInteger_Click(object sender, EventArgs e)
        {
            // Call the ReadInt32 function and store the data retrieved in the result variable
            int result = spiFlash.ReadInt32(Convert.ToInt32(numAddress.Value));

            // Display the retrieved data to the user
            statusBar.Text = "Integer Read: " + result.ToString();
        }

        /// <summary>Occurs when the Read String button is clicked.</summary>
        private void btnReadString_Click(object sender, EventArgs e)
        {
            // Call the ReadString function and store the data retrieved in the result variable
            string result = spiFlash.ReadString(Convert.ToInt32(numAddress.Value), Encoding.UTF8, 8);

            // Display the retrieved data to the user
            statusBar.Text = "String Read: " + result;
        }

        /// <summary>Occurs when the Write Boolean button is clicked.</summary>
        private void btnWriteBoolean_Click(object sender, EventArgs e)
        {
            // Create a boolean value to write to SPI Flash memory
            bool data = true;

            // Call the WriteBoolean function, specifying the data to write
            spiFlash.WriteBoolean(data, Convert.ToInt32(numAddress.Value));

            // Display the sent data to the user
            statusBar.Text = "Boolean Write: " + data.ToString();
        }

        /// <summary>Occurs when the Write Char button is clicked.</summary>
        private void btnWriteChar_Click(object sender, EventArgs e)
        {
            // Create a char value to write to SPI Flash memory
            char data = 'a';

            // Call the WriteChar function, specifying the data to write
            spiFlash.WriteChar(data, Convert.ToInt32(numAddress.Value));

            // Display the sent data to the user
            statusBar.Text = "Char Write: " + data.ToString();
        }

        /// <summary>Occurs when the Write Integer button is clicked.</summary>
        private void btnWriteInteger_Click(object sender, EventArgs e)
        {
            // Create a integer value to write to SPI Flash memory
            int data = 100;

            // Call the WriteInt32 function, specifying the data to write
            spiFlash.WriteInt32(data, Convert.ToInt32(numAddress.Value));

            // Display the sent data to the user
            statusBar.Text = "Integer Write: " + data.ToString();
        }

        /// <summary>Occurs when the Write String button is clicked.</summary>
        private void btnWriteString_Click(object sender, EventArgs e)
        {
            // Create a string value to write to SPI Flash memory
            string data = "Hi!";

            // Call the WriteString function, specifying the data to write
            spiFlash.WriteString(data, Encoding.UTF8, Convert.ToInt32(numAddress.Value));

            // Display the sent data to the user
            statusBar.Text = "String Write: " + data;
        }

        /// <summary>Occurs when the Erase Sector button is clicked.</summary>
        private void btnEraseSector_Click(object sender, EventArgs e)
        {
            // Erase the specified sector
            spiFlash.EraseSector(Convert.ToInt32(numAddress.Value));

            // Notify the user that the action has completed
            statusBar.Text = "Sector Erased: " + numAddress.Value.ToString();
        }

        /// <summary>Occurs when the Erase Chip button is clicked.</summary>
        private void btnEraseChip_Click(object sender, EventArgs e)
        {
            // Warn the user that this may take a while
            if (MessageBox.Show(this, "Note: This may take a very long time, are you sure you want to continue?", "Erase Chip", MessageBoxButtons.YesNo) == DialogResult.Yes) // User clicked yes
            {
                // Erase the entire chip
                spiFlash.EraseChip();

                // Notify the user that the action has completed
                statusBar.Text = "Chip Erased: " + spiFlash.ID.ToString();
            }
        }
        #endregion
        #region Private Functions
        /// <summary>Updates the StatusBar text and enabled/disables the GUI controls.</summary>
        /// <param name="statusText">System.String value containing the StatusBar text.</param>
        /// <param name="enable">System.Boolean value specifying whether to enable the GUI controls.</param>
        private void UpdateControls(string statusText, bool enable)
        {
            statusBar.Text = statusText;

            lblAddress.Enabled = enable;
            numAddress.Enabled = enable;
            grpRead.Enabled = enable;
            grpWrite.Enabled = enable;
            grpErase.Enabled = enable;
        }
        #endregion
    }
    #endregion
}
