namespace LockoutCreator
{
    partial class LockoutInputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LockoutInputForm));
            this.lockoutIDLabel = new System.Windows.Forms.Label();
            this.lockoutDateLabel = new System.Windows.Forms.Label();
            this.lockoutTimeLabel = new System.Windows.Forms.Label();
            this.unlockTimeLabel = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lockoutIDComboBox = new System.Windows.Forms.ComboBox();
            this.lockoutDatePicker = new System.Windows.Forms.DateTimePicker();
            this.lockTimePicker = new System.Windows.Forms.DateTimePicker();
            this.unlockTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // lockoutIDLabel
            // 
            this.lockoutIDLabel.AutoSize = true;
            this.lockoutIDLabel.Location = new System.Drawing.Point(18, 16);
            this.lockoutIDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lockoutIDLabel.Name = "lockoutIDLabel";
            this.lockoutIDLabel.Size = new System.Drawing.Size(77, 16);
            this.lockoutIDLabel.TabIndex = 0;
            this.lockoutIDLabel.Text = "Lockout ID: ";
            // 
            // lockoutDateLabel
            // 
            this.lockoutDateLabel.AutoSize = true;
            this.lockoutDateLabel.Location = new System.Drawing.Point(18, 51);
            this.lockoutDateLabel.Name = "lockoutDateLabel";
            this.lockoutDateLabel.Size = new System.Drawing.Size(90, 16);
            this.lockoutDateLabel.TabIndex = 2;
            this.lockoutDateLabel.Text = "Lockout Date:";
            // 
            // lockoutTimeLabel
            // 
            this.lockoutTimeLabel.AutoSize = true;
            this.lockoutTimeLabel.Location = new System.Drawing.Point(18, 85);
            this.lockoutTimeLabel.Name = "lockoutTimeLabel";
            this.lockoutTimeLabel.Size = new System.Drawing.Size(74, 16);
            this.lockoutTimeLabel.TabIndex = 4;
            this.lockoutTimeLabel.Text = "Lock Time:";
            // 
            // unlockTimeLabel
            // 
            this.unlockTimeLabel.AutoSize = true;
            this.unlockTimeLabel.Location = new System.Drawing.Point(18, 118);
            this.unlockTimeLabel.Name = "unlockTimeLabel";
            this.unlockTimeLabel.Size = new System.Drawing.Size(87, 16);
            this.unlockTimeLabel.TabIndex = 6;
            this.unlockTimeLabel.Text = "Unlock Time:";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(21, 166);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // pBar1
            // 
            this.pBar1.Location = new System.Drawing.Point(115, 166);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(132, 23);
            this.pBar1.TabIndex = 9;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(252, 170);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 10;
            // 
            // lockoutIDComboBox
            // 
            this.lockoutIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lockoutIDComboBox.FormattingEnabled = true;
            this.lockoutIDComboBox.Location = new System.Drawing.Point(115, 16);
            this.lockoutIDComboBox.Name = "lockoutIDComboBox";
            this.lockoutIDComboBox.Size = new System.Drawing.Size(163, 24);
            this.lockoutIDComboBox.Sorted = true;
            this.lockoutIDComboBox.TabIndex = 1;
            // 
            // lockoutDatePicker
            // 
            this.lockoutDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.lockoutDatePicker.Location = new System.Drawing.Point(114, 51);
            this.lockoutDatePicker.Name = "lockoutDatePicker";
            this.lockoutDatePicker.Size = new System.Drawing.Size(164, 22);
            this.lockoutDatePicker.TabIndex = 2;
            this.lockoutDatePicker.Value = new System.DateTime(2019, 9, 11, 0, 0, 0, 0);
            // 
            // lockTimePicker
            // 
            this.lockTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.lockTimePicker.Location = new System.Drawing.Point(115, 85);
            this.lockTimePicker.Name = "lockTimePicker";
            this.lockTimePicker.Size = new System.Drawing.Size(163, 22);
            this.lockTimePicker.TabIndex = 3;
            // 
            // unlockTimePicker
            // 
            this.unlockTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.unlockTimePicker.Location = new System.Drawing.Point(115, 118);
            this.unlockTimePicker.Name = "unlockTimePicker";
            this.unlockTimePicker.Size = new System.Drawing.Size(163, 22);
            this.unlockTimePicker.TabIndex = 4;
            // 
            // LockoutInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 216);
            this.Controls.Add(this.unlockTimePicker);
            this.Controls.Add(this.lockTimePicker);
            this.Controls.Add(this.lockoutDatePicker);
            this.Controls.Add(this.lockoutIDComboBox);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pBar1);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.unlockTimeLabel);
            this.Controls.Add(this.lockoutTimeLabel);
            this.Controls.Add(this.lockoutDateLabel);
            this.Controls.Add(this.lockoutIDLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "LockoutInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lockout Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LockoutInputForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lockoutIDLabel;
        private System.Windows.Forms.Label lockoutDateLabel;
        private System.Windows.Forms.Label lockoutTimeLabel;
        private System.Windows.Forms.Label unlockTimeLabel;
        private System.Windows.Forms.Button submitButton;
        public System.Windows.Forms.ProgressBar pBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox lockoutIDComboBox;
        private System.Windows.Forms.DateTimePicker lockoutDatePicker;
        private System.Windows.Forms.DateTimePicker lockTimePicker;
        private System.Windows.Forms.DateTimePicker unlockTimePicker;
    }
}