namespace msn2.net.Pictures.Controls.UserControls
{
    partial class DateTimePicker
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.hourPicker = new System.Windows.Forms.NumericUpDown();
            this.minutePicker = new System.Windows.Forms.DomainUpDown();
            this.ampmPicker = new System.Windows.Forms.DomainUpDown();
            this.dateLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.hourPicker)).BeginInit();
            this.SuspendLayout();
// 
// datePicker
// 
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker.Location = new System.Drawing.Point(0, 0);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(95, 20);
            this.datePicker.TabIndex = 0;
// 
// hourPicker
// 
            this.hourPicker.Location = new System.Drawing.Point(102, 1);
            this.hourPicker.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.hourPicker.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hourPicker.Name = "hourPicker";
            this.hourPicker.Size = new System.Drawing.Size(38, 20);
            this.hourPicker.TabIndex = 1;
            this.hourPicker.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hourPicker.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
// 
// minutePicker
// 
            this.minutePicker.Location = new System.Drawing.Point(147, 1);
            this.minutePicker.Name = "minutePicker";
            this.minutePicker.Size = new System.Drawing.Size(40, 20);
            this.minutePicker.TabIndex = 2;
            this.minutePicker.Text = "00";
            this.minutePicker.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
// 
// ampmPicker
// 
            this.ampmPicker.Items.Add("am");
            this.ampmPicker.Items.Add("pm");
            this.ampmPicker.Location = new System.Drawing.Point(194, 0);
            this.ampmPicker.Name = "ampmPicker";
            this.ampmPicker.Size = new System.Drawing.Size(49, 20);
            this.ampmPicker.TabIndex = 3;
            this.ampmPicker.Text = "am";
            this.ampmPicker.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
// 
// dateLabel
// 
            this.dateLabel.Location = new System.Drawing.Point(4, 30);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(240, 18);
            this.dateLabel.TabIndex = 4;
            this.dateLabel.Text = "[ Date ]";
            this.dateLabel.Click += new System.EventHandler(this.dateLabel_Click);
// 
// DateTimePicker
// 
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.ampmPicker);
            this.Controls.Add(this.minutePicker);
            this.Controls.Add(this.hourPicker);
            this.Controls.Add(this.datePicker);
            this.Name = "DateTimePicker";
            this.Size = new System.Drawing.Size(247, 48);
            this.Resize += new System.EventHandler(this.DateTimePicker_Resize);
            this.Enter += new System.EventHandler(this.DateTimePicker_Enter);
            this.Leave += new System.EventHandler(this.DateTimePicker_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.hourPicker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.NumericUpDown hourPicker;
        private System.Windows.Forms.DomainUpDown minutePicker;
        private System.Windows.Forms.DomainUpDown ampmPicker;
        private System.Windows.Forms.Label dateLabel;
    }
}
