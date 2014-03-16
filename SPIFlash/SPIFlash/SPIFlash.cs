#region Using
// Imported namespaces (System)
using System;
using System.IO;
using System.Text;
// Imported namespaces (DLN)
using Dln;
using Dln.SpiMaster;
#endregion
namespace SPIFlash
{
    #region SPIFlash
    /// <summary>Wrapper class containing helper functions for reading/writing data to Serial Flash memory.</summary>
    /// <remarks>Namespaces marked (Exposed) contain members and functions that are accessibile by the end-user when creating a SPIFlash object.</remarks>
    public class SPIFlash
    {
        #region Enums (Not exposed)
        /// <summary>For internal use only.</summary>
        private enum CommandID : byte
        {
            PageProgram = 0x02,
            ReadArray = 0x03,
            ReadStatusRegister = 0x05,
            WriteEnable = 0x06,
            ReadFlashID = 0x9F,
            BulkErase = 0xC7,
            SectorErase = 0xD8
        }
        #endregion
        #region Objects (Not exposed)
        /// <summary>Stores an instance of a Dln.Connection object in memory for as long as the instance of this class is alive.</summary>
        private Connection p_Connection;
        /// <summary>Stores an instance of a Dln.Device object in memory for as long as the instance of this class is alive.</summary>
        private Device p_Device;
        /// <summary>Stores the globally unique identifier in memory for as long as the instance of this class is alive.</summary>
        private uint p_ID;
        /// <summary>Stores an instance of a Dln.SpiMaster.Port object in memory for as long as the instance of this class is alive.</summary>
        private Port p_SPI;
        #endregion
        #region Properties (Exposed)
        /// <summary>Gets the current DLN-series device adapter installed on the system.</summary>
        /// <value>Dln.Device object encapsulating the DLN-series device adapter.</value>
        public Device Device
        {
            get { return p_Device; }
        }

        /// <summary>Gets a boolean value (True/False) containing whether the current SPI Flash device is enabled.</summary>
        /// <value>System.Boolean value containing the state of the current SPI Flash device.</value>
        public bool Enabled
        {
            get
            {
                if (p_SPI != null) return p_SPI.Enabled;
                return false;
            }
        }

        /// <summary>Gets the globally unique identifier representing the current SPI Flash device.</summary>
        /// <value>System.UInt32 value containing the globally unique identifer.</value>
        public uint ID
        {
            get { return p_ID; }
        }

        /// <summary>Gets the current SPI Flash device installed on the system.</summary>
        /// <value>Dln.SpiMaster.Port object encapsulating the SPI Flash device.</value>
        public Port SPI
        {
            get { return p_SPI; }
        }
        #endregion
        #region Constructor (Exposed)
        /// <summary>Initializes a new instance of the SPIFlash class given the specified frequency and frame size.</summary>
        /// <param name="frequency">System.Int32 value specifying the port frequency, in Hz (Defaults to 1000000).</param>
        /// <param name="frameSize">System.Int32 value specifying the data frame size, in bytes (Defaults to 8).</param>
        public SPIFlash(int frequency = 1000000, int frameSize = 8)
        {
            // Connect to DLN server
            p_Connection = Library.Connect("localhost", Connection.DefaultPort);

            // Open DLN device
            if (Device.Count().Equals(0)) throw new Exception("No DLN-series adapters have been detected.");

            // Create p_Device and p_SPI objects and store them in memory
            p_Device = Device.Open();
            p_SPI = p_Device.SpiMaster.Ports[0];

            // Configure SPI by setting object properties
            p_SPI.Enabled = false; // Disable SPI while configuring
            p_SPI.Frequency = frequency;
            p_SPI.FrameSize = frameSize;
            p_SPI.Enabled = true; // Enable SPI

            // Retrieve SPI Flash device ID
            p_ID = GetFlashID();
        }
        #endregion
        #region Destructor (Not exposed)
        ~SPIFlash()
        {
            // Disable SPI
            if (p_SPI != null) p_SPI.Enabled = false;

            Library.Disconnect(p_Connection);
        }
        #endregion
        #region Public Functions (Exposed)
        /// <summary>Erases all data from the current SPI Flash device (Note: This may take a very long time).</summary>
        public void EraseChip()
        {
            if (!Enabled) throw new InvalidOperationException("SPI Flash device is not enabled.");

            byte[] writeEnable = new byte[1] { (byte)CommandID.WriteEnable };
            byte[] readStatus = new byte[2] { (byte)CommandID.ReadStatusRegister, 0 };
            byte[] eraseChip = new byte[1] { (byte)CommandID.BulkErase };

            // Write enable
            p_SPI.ReadWrite(writeEnable);
            // Erase chip
            p_SPI.ReadWrite(eraseChip);

            // Poll for erase finish (1200 * 100ms = 120s max)
            for (int i = 0; i < 1200; i++)
            {
                System.Threading.Thread.Sleep(100);

                byte[] rxbuffer = p_SPI.ReadWrite(readStatus);
                byte status = rxbuffer[1];
                // Bit 0 = 0 => ready
                //       = 1 => busy
                if ((status & 0x01) == 0) break;
            }
        }

        /// <summary>Erases a single sector from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        public void EraseSector(int address)
        {
            if (!Enabled) throw new InvalidOperationException("SPI Flash device is not enabled.");

            byte[] writeEnable = new byte[1] { (byte)CommandID.WriteEnable };
            byte[] readStatus = new byte[2] { (byte)CommandID.ReadStatusRegister, 0 };
            byte[] eraseSector = new byte[4];

            // Prepare command
            eraseSector[0] = (byte)CommandID.SectorErase;
            eraseSector[1] = (byte)(address >> 16);
            eraseSector[2] = (byte)(address >> 8);
            eraseSector[3] = (byte)(address >> 0);

            // Write enable
            p_SPI.ReadWrite(writeEnable);
            // Erase sector
            p_SPI.ReadWrite(eraseSector);

            // Poll for erase finish (30 * 100ms = 3s max)
            for (int i = 0; i < 30; i++)
            {
                System.Threading.Thread.Sleep(100);

                byte[] rxbuffer = p_SPI.ReadWrite(readStatus);
                byte status = rxbuffer[1];
                // Bit 0 = 0 => ready
                //       = 1 => busy
                if ((status & 0x01) == 0) break;
            }
        }

        /// <summary>Reads an array of bytes from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <param name="length">System.Int32 value specifying the number of bytes to read.</param>
        /// <returns>System.Byte array containing the data retrieved.</returns>
        public byte[] Read(int address, int length)
        {
            if (!Enabled) throw new InvalidOperationException("SPI Flash device is not enabled.");
            if (length.Equals(0)) throw new ArgumentOutOfRangeException("length", "Length must be greater than zero.");

            int size = length;
            byte[] result = new byte[size];
            int offset = 0;

            while (size > 0)
            {
                int n = Math.Min(size, 256);

                ReadPage(address, result, offset, n);

                address += n;
                offset += n;
                size -= n;
            }

            return result;
        }

        /// <summary>Reads a boolean value (True/False) from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.Boolean value containing the data retrieved.</returns>
        public bool ReadBoolean(int address)
        {
            byte[] result = Read(address, 1);

            return BitConverter.ToBoolean(result, 0);
        }

        /// <summary>Reads a single byte from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.Byte value containing the data retrieved.</returns>
        public byte ReadByte(int address)
        {
            byte[] result = Read(address, 1);

            return result[0];
        }

        /// <summary>Reads a single character from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.Char value containing the data retrieved.</returns>
        public char ReadChar(int address)
        {
            byte[] result = Read(address, 2);

            return BitConverter.ToChar(result, 0);
        }

        /// <summary>Reads a double-precision, floating-point number from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.Double value containing the data retrieved.</returns>
        public double ReadDouble(int address)
        {
            byte[] result = Read(address, 8);

            return BitConverter.ToDouble(result, 0);
        }

        /// <summary>Reads a 16-bit signed integer from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.Int16 value containing the data retrieved.</returns>
        public short ReadInt16(int address)
        {
            byte[] result = Read(address, 2);

            return BitConverter.ToInt16(result, 0);
        }

        /// <summary>Reads a 32-bit signed integer from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.Int32 value containing the data retrieved.</returns>
        public int ReadInt32(int address)
        {
            byte[] result = Read(address, 4);

            return BitConverter.ToInt32(result, 0);
        }

        /// <summary>Reads a 64-bit signed integer from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.Int64 value containing the data retrieved.</returns>
        public long ReadInt64(int address)
        {
            byte[] result = Read(address, 8);

            return BitConverter.ToInt64(result, 0);
        }

        /// <summary>Reads a single-precision, floating-point number from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.Single value containing the data retrieved.</returns>
        public float ReadSingle(int address)
        {
            byte[] result = Read(address, 4);

            return BitConverter.ToSingle(result, 0);
        }

        /// <summary>Reads an encoded string value from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <param name="encoding">System.Text.Encoding object specifying the text encoding.</param>
        /// <param name="length">System.Int32 value specifying the number of bytes to read.</param>
        /// <returns>System.String value containing the data retrieved.</returns>
        /// <remarks>In order to read a string correctly, you must know the text encoding and length of the string beforehand.</remarks>
        public string ReadString(int address, Encoding encoding, int length)
        {
            if (encoding == null) throw new ArgumentNullException("encoding");

            byte[] result = Read(address, length);

            return encoding.GetString(result);
        }

        /// <summary>Reads a 16-bit unsigned integer from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.UInt16 value containing the data retrieved.</returns>
        public ushort ReadUInt16(int address)
        {
            byte[] result = Read(address, 2);

            return BitConverter.ToUInt16(result, 0);
        }

        /// <summary>Reads a 32-bit signed integer from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.UInt32 value containing the data retrieved.</returns>
        public uint ReadUInt32(int address)
        {
            byte[] result = Read(address, 4);

            return BitConverter.ToUInt32(result, 0);
        }

        /// <summary>Reads a 64-bit signed integer from the current SPI Flash device at the specified memory address.</summary>
        /// <param name="address">System.Int32 value specifying the memory address.</param>
        /// <returns>System.UInt64 value containing the data retrieved.</returns>
        public ulong ReadUInt64(int address)
        {
            byte[] result = Read(address, 8);

            return BitConverter.ToUInt64(result, 0);
        }

        /// <summary>Writes an array of bytes to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Byte array containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void Write(byte[] data, int address = 0)
        {
            if (!Enabled) throw new InvalidOperationException("SPI Flash device is not enabled.");

            int offset = 0;
            int size = data.Length;

            while (size > 0)
            {
                int n = 256 - address % 256; // if programming starts not from page start
                if (n > size) n = size;

                WritePage(address, data, offset, n);

                address += n;
                offset += n;
                size -= n;
            }
        }

        /// <summary>Writes a boolean value (True/False) to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Boolean value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteBoolean(bool data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes a single byte to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Byte value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteByte(byte data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Write a single character to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Char value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteChar(char data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes a double-precision, floating-point number to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Double value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteDouble(double data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes a 16-bit signed integer to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Int16 value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteInt16(short data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes a 32-bit signed integer to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">Syste.Int32 value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteInt32(int data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes a 64-bit signed integer to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Int64 value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteInt64(long data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes a single-precision, floating-point number to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Single value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteSingle(float data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes an encoded string value to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.String value containing the data to be written.</param>
        /// <param name="encoding">System.Text.Encoding object specifying the text encoding.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteString(string data, Encoding encoding, int address = 0)
        {
            if (encoding == null) throw new ArgumentNullException("encoding");

            Write(encoding.GetBytes(data), address);
        }

        /// <summary>Writes a 16-bit unsigned integer to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.UInt16 value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteUInt16(ushort data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes a 32-bit unsigned integer to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.UInt32 value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteUInt32(uint data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }

        /// <summary>Writes a 64-bit unsigned integer to the current SPI Flash device at the specified memory address.</summary>
        /// <param name="data">System.Int64 value containing the data to be written.</param>
        /// <param name="address">System.Int32 value specifying the memory address (Defaults to 0).</param>
        public void WriteUInt64(ulong data, int address = 0)
        {
            Write(BitConverter.GetBytes(data), address);
        }
        #endregion
        #region Private Functions (Not exposed)
        /// <summary>For internal use only.</summary>
        private uint GetFlashID()
        {
            byte[] txbuffer = new byte[3];
            byte[] rxbuffer;

            // Prepare command
            txbuffer[0] = (byte)CommandID.ReadFlashID;

            // Send command
            rxbuffer = p_SPI.ReadWrite(txbuffer);

            // rxbuffer[1] = manufacturerID
            // rxbuffer[2] = deviceID
            return ((uint)rxbuffer[1] << 8) | rxbuffer[2];
        }

        /// <summary>For internal use only.</summary>
        private void ReadPage(int address, byte[] buffer, int offset, int count)
        {
            byte[] txbuffer = new byte[4 + count];
            byte[] rxbuffer;

            // Prepare command
            txbuffer[0] = (byte)CommandID.ReadArray;
            txbuffer[1] = (byte)(address >> 16);
            txbuffer[2] = (byte)(address >> 8);
            txbuffer[3] = (byte)(address >> 0);

            // Send command
            rxbuffer = p_SPI.ReadWrite(txbuffer);

            Array.Copy(rxbuffer, 4, buffer, offset, count);
        }

        /// <summary>For internal use only.</summary>
        private void WritePage(int address, byte[] buffer, int offset, int count)
        {
            byte[] writeEnable = new byte[1] { (byte)CommandID.WriteEnable };
            byte[] readStatus = new byte[2] { (byte)CommandID.ReadStatusRegister, 0 };
            byte[] txbuffer = new byte[4 + count];

            // Prepare command
            txbuffer[0] = (byte)CommandID.PageProgram;
            txbuffer[1] = (byte)(address >> 16);
            txbuffer[2] = (byte)(address >> 8);
            txbuffer[3] = (byte)(address >> 0);
            Array.Copy(buffer, offset, txbuffer, 4, count);

            // Write enable
            p_SPI.ReadWrite(writeEnable);

            // Send command
            p_SPI.ReadWrite(txbuffer);

            // Wait for write finish
            while (true)
            {
                byte[] rxbuffer = p_SPI.ReadWrite(readStatus);
                byte status = rxbuffer[1];
                // Bit 0 = 0 => ready
                //       = 1 => busy
                if ((status & 0x01) == 0) break;
            }
        }
        #endregion
    }
    #endregion
}