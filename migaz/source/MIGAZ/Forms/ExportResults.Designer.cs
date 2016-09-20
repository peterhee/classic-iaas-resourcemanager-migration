namespace MIGAZ.Forms
{
    partial class ExportResults
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnViewTemplate = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnGenerateInstructions = new System.Windows.Forms.Button();
            this.txtRGName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboRGLocation = new System.Windows.Forms.ComboBox();
            this.cboSubscription = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(720, 421);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnViewTemplate);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.txtMessages);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(712, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Status";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnViewTemplate
            // 
            this.btnViewTemplate.Location = new System.Drawing.Point(27, 326);
            this.btnViewTemplate.Name = "btnViewTemplate";
            this.btnViewTemplate.Size = new System.Drawing.Size(169, 44);
            this.btnViewTemplate.TabIndex = 3;
            this.btnViewTemplate.Text = "View Template";
            this.btnViewTemplate.UseVisualStyleBackColor = true;
            this.btnViewTemplate.Click += new System.EventHandler(this.btnViewTemplate_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MIGAZ.Properties.Resources.Resource_group;
            this.pictureBox1.Location = new System.Drawing.Point(17, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 108);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // txtMessages
            // 
            this.txtMessages.Location = new System.Drawing.Point(52, 187);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.Size = new System.Drawing.Size(539, 112);
            this.txtMessages.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(525, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Please note the following messages arising from the template generation:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(137, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(556, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "Click the Next Steps tab to learn how to deploy it to Azure Resource Manager.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(357, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Your template has been generated successfully!  ";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabPage2.Controls.Add(this.btnGenerateInstructions);
            this.tabPage2.Controls.Add(this.txtRGName);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.cboRGLocation);
            this.tabPage2.Controls.Add(this.cboSubscription);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(712, 388);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Next Steps";
            // 
            // btnGenerateInstructions
            // 
            this.btnGenerateInstructions.Location = new System.Drawing.Point(197, 218);
            this.btnGenerateInstructions.Name = "btnGenerateInstructions";
            this.btnGenerateInstructions.Size = new System.Drawing.Size(200, 47);
            this.btnGenerateInstructions.TabIndex = 5;
            this.btnGenerateInstructions.Text = "Show Instructions";
            this.btnGenerateInstructions.UseVisualStyleBackColor = true;
            this.btnGenerateInstructions.Click += new System.EventHandler(this.btnGenerateInstructions_Click);
            // 
            // txtRGName
            // 
            this.txtRGName.Location = new System.Drawing.Point(197, 103);
            this.txtRGName.Name = "txtRGName";
            this.txtRGName.Size = new System.Drawing.Size(293, 26);
            this.txtRGName.TabIndex = 4;
            this.txtRGName.Text = "MigratedResources";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 3;
            this.label6.Text = "Location:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "New Resource Group:";
            // 
            // cboRGLocation
            // 
            this.cboRGLocation.FormattingEnabled = true;
            this.cboRGLocation.Items.AddRange(new object[] {
            "East US",
            "East US 2",
            "West US",
            "Central US",
            "North Central US",
            "South Central US",
            "North Europe",
            "West Europe",
            "East Asia",
            "Southeast Asia",
            "Japan East",
            "Japan West",
            "Australia East",
            "Australia Southeast",
            "Brazil South",
            "South India",
            "Central India",
            "West India",
            "Canada Central",
            "Canada East",
            "West US 2",
            "West Central US",
            "UK South",
            "UK West"});
            this.cboRGLocation.Location = new System.Drawing.Point(197, 153);
            this.cboRGLocation.Name = "cboRGLocation";
            this.cboRGLocation.Size = new System.Drawing.Size(293, 28);
            this.cboRGLocation.TabIndex = 2;
            // 
            // cboSubscription
            // 
            this.cboSubscription.DisplayMember = "SubscriptionName";
            this.cboSubscription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubscription.FormattingEnabled = true;
            this.cboSubscription.Location = new System.Drawing.Point(197, 55);
            this.cboSubscription.Name = "cboSubscription";
            this.cboSubscription.Size = new System.Drawing.Size(438, 28);
            this.cboSubscription.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Subscription:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(470, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Now let’s get this template deployed to Azure Resource Manager!\r\n";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(591, 450);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(141, 41);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ExportResults
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 510);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportResults";
            this.Text = "Export Complete";
            this.Load += new System.EventHandler(this.ExportResults_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRGName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboSubscription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboRGLocation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnViewTemplate;
        private System.Windows.Forms.Button btnGenerateInstructions;
    }
}