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
            this.SuspendLayout();
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(12, 18);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(93, 13);
            this.lblSuffix.TabIndex = 37;
            this.lblSuffix.Text = "Uniqueness suffix:";
            // 
            // txtSuffix
            // 
            this.txtSuffix.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtSuffix.Location = new System.Drawing.Point(129, 15);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(30, 20);
            this.txtSuffix.TabIndex = 36;
            this.txtSuffix.Text = "v2";
            this.txtSuffix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSuffix_KeyPress);
            // 
            // chkAllowTelemetry
            // 
            this.chkAllowTelemetry.AutoSize = true;
            this.chkAllowTelemetry.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAllowTelemetry.Checked = true;
            this.chkAllowTelemetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowTelemetry.Location = new System.Drawing.Point(15, 94);
            this.chkAllowTelemetry.Name = "chkAllowTelemetry";
            this.chkAllowTelemetry.Size = new System.Drawing.Size(144, 17);
            this.chkAllowTelemetry.TabIndex = 32;
            this.chkAllowTelemetry.Text = "Allow telemetry collection";
            this.chkAllowTelemetry.UseVisualStyleBackColor = true;
            this.chkAllowTelemetry.CheckedChanged += new System.EventHandler(this.chkAllowTelemetry_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(84, 130);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 38;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkBuildEmpty
            // 
            this.chkBuildEmpty.AutoSize = true;
            this.chkBuildEmpty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBuildEmpty.Checked = true;
            this.chkBuildEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBuildEmpty.Location = new System.Drawing.Point(18, 56);
            this.chkBuildEmpty.Name = "chkBuildEmpty";
            this.chkBuildEmpty.Size = new System.Drawing.Size(141, 17);
            this.chkBuildEmpty.TabIndex = 39;
            this.chkBuildEmpty.Text = "Build empty environment";
            this.chkBuildEmpty.UseVisualStyleBackColor = true;
            // 
            // formOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 165);
            this.Controls.Add(this.chkBuildEmpty);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblSuffix);
            this.Controls.Add(this.txtSuffix);
            this.Controls.Add(this.chkAllowTelemetry);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}