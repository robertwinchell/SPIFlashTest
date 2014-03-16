namespace SPIFlash
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpRead = new System.Windows.Forms.GroupBox();
            this.grpWrite = new System.Windows.Forms.GroupBox();
            this.btnWriteBoolean = new System.Windows.Forms.Button();
            this.numAddress = new System.Windows.Forms.NumericUpDown();
            this.lblAddress = new System.Windows.Forms.Label();
            this.grpErase = new System.Windows.Forms.GroupBox();
            this.btnEraseSector = new System.Windows.Forms.Button();
            this.btnEraseChip = new System.Windows.Forms.Button();
            this.btnWriteChar = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.btnWriteString = new System.Windows.Forms.Button();
            this.btnWriteInteger = new System.Windows.Forms.Button();
            this.btnReadString = new System.Windows.Forms.Button();
            this.btnReadInteger = new System.Windows.Forms.Button();
            this.btnReadChar = new System.Windows.Forms.Button();
            this.btnReadBoolean = new System.Windows.Forms.Button();
            this.grpRead.SuspendLayout();
            this.grpWrite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAddress)).BeginInit();
            this.grpErase.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpRead
            // 
            this.grpRead.Controls.Add(this.btnReadString);
            this.grpRead.Controls.Add(this.btnReadInteger);
            this.grpRead.Controls.Add(this.btnReadChar);
            this.grpRead.Controls.Add(this.btnReadBoolean);
            this.grpRead.Enabled = false;
            this.grpRead.Location = new System.Drawing.Point(12, 38);
            this.grpRead.Name = "grpRead";
            this.grpRead.Size = new System.Drawing.Size(260, 84);
            this.grpRead.TabIndex = 0;
            this.grpRead.TabStop = false;
            this.grpRead.Text = "Read Memory";
            // 
            // grpWrite
            // 
            this.grpWrite.Controls.Add(this.btnWriteString);
            this.grpWrite.Controls.Add(this.btnWriteInteger);
            this.grpWrite.Controls.Add(this.btnWriteChar);
            this.grpWrite.Controls.Add(this.btnWriteBoolean);
            this.grpWrite.Enabled = false;
            this.grpWrite.Location = new System.Drawing.Point(12, 128);
            this.grpWrite.Name = "grpWrite";
            this.grpWrite.Size = new System.Drawing.Size(260, 84);
            this.grpWrite.TabIndex = 1;
            this.grpWrite.TabStop = false;
            this.grpWrite.Text = "Write Memory";
            // 
            // btnWriteBoolean
            // 
            this.btnWriteBoolean.Location = new System.Drawing.Point(6, 19);
            this.btnWriteBoolean.Name = "btnWriteBoolean";
            this.btnWriteBoolean.Size = new System.Drawing.Size(121, 23);
            this.btnWriteBoolean.TabIndex = 8;
            this.btnWriteBoolean.Text = "Write Boolean";
            this.btnWriteBoolean.UseVisualStyleBackColor = true;
            this.btnWriteBoolean.Click += new System.EventHandler(this.btnWriteBoolean_Click);
            // 
            // numAddress
            // 
            this.numAddress.Enabled = false;
            this.numAddress.Location = new System.Drawing.Point(145, 12);
            this.numAddress.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numAddress.Name = "numAddress";
            this.numAddress.Size = new System.Drawing.Size(127, 20);
            this.numAddress.TabIndex = 5;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Enabled = false;
            this.lblAddress.Location = new System.Drawing.Point(12, 14);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(125, 13);
            this.lblAddress.TabIndex = 4;
            this.lblAddress.Text = "Memory Address (Offset):";
            // 
            // grpErase
            // 
            this.grpErase.Controls.Add(this.btnEraseChip);
            this.grpErase.Controls.Add(this.btnEraseSector);
            this.grpErase.Enabled = false;
            this.grpErase.Location = new System.Drawing.Point(12, 218);
            this.grpErase.Name = "grpErase";
            this.grpErase.Size = new System.Drawing.Size(260, 56);
            this.grpErase.TabIndex = 6;
            this.grpErase.TabStop = false;
            this.grpErase.Text = "Erase Memory";
            // 
            // btnEraseSector
            // 
            this.btnEraseSector.Location = new System.Drawing.Point(6, 19);
            this.btnEraseSector.Name = "btnEraseSector";
            this.btnEraseSector.Size = new System.Drawing.Size(121, 23);
            this.btnEraseSector.TabIndex = 8;
            this.btnEraseSector.Text = "Erase Sector";
            this.btnEraseSector.UseVisualStyleBackColor = true;
            this.btnEraseSector.Click += new System.EventHandler(this.btnEraseSector_Click);
            // 
            // btnEraseChip
            // 
            this.btnEraseChip.Location = new System.Drawing.Point(133, 19);
            this.btnEraseChip.Name = "btnEraseChip";
            this.btnEraseChip.Size = new System.Drawing.Size(121, 23);
            this.btnEraseChip.TabIndex = 9;
            this.btnEraseChip.Text = "Erase Chip";
            this.btnEraseChip.UseVisualStyleBackColor = true;
            this.btnEraseChip.Click += new System.EventHandler(this.btnEraseChip_Click);
            // 
            // btnWriteChar
            // 
            this.btnWriteChar.Location = new System.Drawing.Point(133, 19);
            this.btnWriteChar.Name = "btnWriteChar";
            this.btnWriteChar.Size = new System.Drawing.Size(121, 23);
            this.btnWriteChar.TabIndex = 9;
            this.btnWriteChar.Text = "Write Char";
            this.btnWriteChar.UseVisualStyleBackColor = true;
            this.btnWriteChar.Click += new System.EventHandler(this.btnWriteChar_Click);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 288);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(284, 22);
            this.statusBar.TabIndex = 7;
            this.statusBar.Text = "Not connected";
            // 
            // btnWriteString
            // 
            this.btnWriteString.Location = new System.Drawing.Point(133, 48);
            this.btnWriteString.Name = "btnWriteString";
            this.btnWriteString.Size = new System.Drawing.Size(121, 23);
            this.btnWriteString.TabIndex = 11;
            this.btnWriteString.Text = "Write String";
            this.btnWriteString.UseVisualStyleBackColor = true;
            this.btnWriteString.Click += new System.EventHandler(this.btnWriteString_Click);
            // 
            // btnWriteInteger
            // 
            this.btnWriteInteger.Location = new System.Drawing.Point(6, 48);
            this.btnWriteInteger.Name = "btnWriteInteger";
            this.btnWriteInteger.Size = new System.Drawing.Size(121, 23);
            this.btnWriteInteger.TabIndex = 10;
            this.btnWriteInteger.Text = "Write Integer";
            this.btnWriteInteger.UseVisualStyleBackColor = true;
            this.btnWriteInteger.Click += new System.EventHandler(this.btnWriteInteger_Click);
            // 
            // btnReadString
            // 
            this.btnReadString.Location = new System.Drawing.Point(133, 48);
            this.btnReadString.Name = "btnReadString";
            this.btnReadString.Size = new System.Drawing.Size(121, 23);
            this.btnReadString.TabIndex = 15;
            this.btnReadString.Text = "Read String";
            this.btnReadString.UseVisualStyleBackColor = true;
            this.btnReadString.Click += new System.EventHandler(this.btnReadString_Click);
            // 
            // btnReadInteger
            // 
            this.btnReadInteger.Location = new System.Drawing.Point(6, 48);
            this.btnReadInteger.Name = "btnReadInteger";
            this.btnReadInteger.Size = new System.Drawing.Size(121, 23);
            this.btnReadInteger.TabIndex = 14;
            this.btnReadInteger.Text = "Read Integer";
            this.btnReadInteger.UseVisualStyleBackColor = true;
            this.btnReadInteger.Click += new System.EventHandler(this.btnReadInteger_Click);
            // 
            // btnReadChar
            // 
            this.btnReadChar.Location = new System.Drawing.Point(133, 19);
            this.btnReadChar.Name = "btnReadChar";
            this.btnReadChar.Size = new System.Drawing.Size(121, 23);
            this.btnReadChar.TabIndex = 13;
            this.btnReadChar.Text = "Read Char";
            this.btnReadChar.UseVisualStyleBackColor = true;
            this.btnReadChar.Click += new System.EventHandler(this.btnReadChar_Click);
            // 
            // btnReadBoolean
            // 
            this.btnReadBoolean.Location = new System.Drawing.Point(6, 19);
            this.btnReadBoolean.Name = "btnReadBoolean";
            this.btnReadBoolean.Size = new System.Drawing.Size(121, 23);
            this.btnReadBoolean.TabIndex = 12;
            this.btnReadBoolean.Text = "Read Boolean";
            this.btnReadBoolean.UseVisualStyleBackColor = true;
            this.btnReadBoolean.Click += new System.EventHandler(this.btnReadBoolean_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 310);
            this.Controls.Add(this.grpErase);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.grpWrite);
            this.Controls.Add(this.grpRead);
            this.Controls.Add(this.numAddress);
            this.Controls.Add(this.lblAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SPIFlash Test Application";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.grpRead.ResumeLayout(false);
            this.grpWrite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numAddress)).EndInit();
            this.grpErase.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpRead;
        private System.Windows.Forms.GroupBox grpWrite;
        private System.Windows.Forms.Button btnWriteBoolean;
        private System.Windows.Forms.NumericUpDown numAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.GroupBox grpErase;
        private System.Windows.Forms.Button btnEraseSector;
        private System.Windows.Forms.Button btnEraseChip;
        private System.Windows.Forms.Button btnWriteChar;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.Button btnWriteString;
        private System.Windows.Forms.Button btnWriteInteger;
        private System.Windows.Forms.Button btnReadString;
        private System.Windows.Forms.Button btnReadBoolean;
        private System.Windows.Forms.Button btnReadInteger;
        private System.Windows.Forms.Button btnReadChar;

    }
}