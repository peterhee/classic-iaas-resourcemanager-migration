namespace MIGAZ.Forms
{
    partial class formOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formOptions));
            this.lblSuffix = new System.Windows.Forms.Label();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.chkAllowTelemetry = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkBuildEmpty = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboAzureEnvironment = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(33, 76);
            this.lblSuffix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(139, 20);
            this.lblSuffix.TabIndex = 2;
            this.lblSuffix.Text = "Uniqueness suffix:";
            // 
            // txtSuffix
            // 
            this.txtSuffix.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtSuffix.Location = new System.Drawing.Point(200, 73);
            this.txtSuffix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtSuffix.TabIndex = 3;
            this.txtSuffix.Text = "v2";
            this.txtSuffix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSuffix_KeyPress);
            // 
            // chkAllowTelemetry
            // 
            this.chkAllowTelemetry.AutoSize = true;
            this.chkAllowTelemetry.Checked = true;
            this.chkAllowTelemetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowTelemetry.Location = new System.Drawing.Point(37, 172);
            this.chkAllowTelemetry.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAllowTelemetry.Name = "chkAllowTelemetry";
            this.chkAllowTelemetry.Size = new System.Drawing.Size(211, 24);
            this.chkAllowTelemetry.TabIndex = 5;
            this.chkAllowTelemetry.Text = "Allow telemetry collection";
            this.chkAllowTelemetry.UseVisualStyleBackColor = true;
            this.chkAllowTelemetry.CheckedChanged += new System.EventHandler(this.chkAllowTelemetry_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(200, 224);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(112, 35);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkBuildEmpty
            // 
            this.chkBuildEmpty.AutoSize = true;
            this.chkBuildEmpty.Checked = true;
            this.chkBuildEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBuildEmpty.Location = new System.Drawing.Point(37, 128);
            this.chkBuildEmpty.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkBuildEmpty.Name = "chkBuildEmpty";
            this.chkBuildEmpty.Size = new System.Drawing.Size(208, 24);
            this.chkBuildEmpty.TabIndex = 4;
            this.chkBuildEmpty.Text = "Build empty environment";
            this.chkBuildEmpty.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(331, 224);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Azure Environment:";
            // 
            // cboAzureEnvironment
            // 
            this.cboAzureEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAzureEnvironment.FormattingEnabled = true;
            this.cboAzureEnvironment.Items.AddRange(new object[] {
            "Azure Cloud",
            "Azure China Cloud",
            "Azure Germany Cloud",
            "Azure US Government Cloud"});
            this.cboAzureEnvironment.Location = new System.Drawing.Point(200, 30);
            this.cboAzureEnvironment.Name = "cboAzureEnvironment";
            this.cboAzureEnvironment.Size = new System.Drawing.Size(243, 28);
            this.cboAzureEnvironment.TabIndex = 1;
            // 
            // formOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(468, 287);
            this.Controls.Add(this.cboAzureEnvironment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkBuildEmpty);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblSuffix);
            this.Controls.Add(this.txtSuffix);
            this.Controls.Add(this.chkAllowTelemetry);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formOptions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.formOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkAllowTelemetry;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkBuildEmpty;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboAzureEnvironment;
    }
}