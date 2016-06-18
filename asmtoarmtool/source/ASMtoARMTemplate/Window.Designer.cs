namespace ASMtoARMTemplate
{
    partial class Window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.txtTenantID = new System.Windows.Forms.TextBox();
            this.lblTenantID = new System.Windows.Forms.Label();
            this.btnGetToken = new System.Windows.Forms.Button();
            this.lblClientID = new System.Windows.Forms.Label();
            this.txtClientID = new System.Windows.Forms.TextBox();
            this.lblReturnURLs = new System.Windows.Forms.Label();
            this.txtReturnURLs = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.gridVirtualNetworks = new System.Windows.Forms.DataGridView();
            this.colVirtualNetwork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridStorageAccounts = new System.Windows.Forms.DataGridView();
            this.colStorageAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridVirtualMachines = new System.Windows.Forms.DataGridView();
            this.colCloudService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVirtualMachine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeploymentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVirtualNetworkName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.chkAllowTelemetry = new System.Windows.Forms.CheckBox();
            this.cmbSubscriptions = new System.Windows.Forms.ComboBox();
            this.lblSubscriptions = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridVirtualNetworks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridStorageAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridVirtualMachines)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTenantID
            // 
            this.txtTenantID.Location = new System.Drawing.Point(135, 21);
            this.txtTenantID.Name = "txtTenantID";
            this.txtTenantID.Size = new System.Drawing.Size(213, 20);
            this.txtTenantID.TabIndex = 0;
            this.txtTenantID.Text = "<domain>.onmicrosoft.com";
            // 
            // lblTenantID
            // 
            this.lblTenantID.AutoSize = true;
            this.lblTenantID.Location = new System.Drawing.Point(21, 24);
            this.lblTenantID.Name = "lblTenantID";
            this.lblTenantID.Size = new System.Drawing.Size(108, 13);
            this.lblTenantID.TabIndex = 1;
            this.lblTenantID.Text = "Tenant (Id or domain)";
            // 
            // btnGetToken
            // 
            this.btnGetToken.Location = new System.Drawing.Point(219, 50);
            this.btnGetToken.Name = "btnGetToken";
            this.btnGetToken.Size = new System.Drawing.Size(129, 23);
            this.btnGetToken.TabIndex = 2;
            this.btnGetToken.Text = "Get Subscriptions";
            this.btnGetToken.UseVisualStyleBackColor = true;
            this.btnGetToken.Click += new System.EventHandler(this.btnGetToken_Click);
            // 
            // lblClientID
            // 
            this.lblClientID.AutoSize = true;
            this.lblClientID.Location = new System.Drawing.Point(643, 47);
            this.lblClientID.Name = "lblClientID";
            this.lblClientID.Size = new System.Drawing.Size(47, 13);
            this.lblClientID.TabIndex = 4;
            this.lblClientID.Text = "Client ID";
            this.lblClientID.Visible = false;
            // 
            // txtClientID
            // 
            this.txtClientID.Location = new System.Drawing.Point(757, 44);
            this.txtClientID.Name = "txtClientID";
            this.txtClientID.Size = new System.Drawing.Size(213, 20);
            this.txtClientID.TabIndex = 3;
            this.txtClientID.Text = "1950a258-227b-4e31-a9cf-717495945fc2";
            this.txtClientID.Visible = false;
            // 
            // lblReturnURLs
            // 
            this.lblReturnURLs.AutoSize = true;
            this.lblReturnURLs.Location = new System.Drawing.Point(643, 60);
            this.lblReturnURLs.Name = "lblReturnURLs";
            this.lblReturnURLs.Size = new System.Drawing.Size(69, 13);
            this.lblReturnURLs.TabIndex = 6;
            this.lblReturnURLs.Text = "Return URLs";
            this.lblReturnURLs.Visible = false;
            // 
            // txtReturnURLs
            // 
            this.txtReturnURLs.Location = new System.Drawing.Point(757, 57);
            this.txtReturnURLs.Name = "txtReturnURLs";
            this.txtReturnURLs.Size = new System.Drawing.Size(213, 20);
            this.txtReturnURLs.TabIndex = 5;
            this.txtReturnURLs.Text = "urn:ietf:wg:oauth:2.0:oob";
            this.txtReturnURLs.Visible = false;
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(292, 5);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(34, 13);
            this.lblToken.TabIndex = 7;
            this.lblToken.Text = "token";
            this.lblToken.Visible = false;
            // 
            // gridVirtualNetworks
            // 
            this.gridVirtualNetworks.AllowUserToAddRows = false;
            this.gridVirtualNetworks.AllowUserToDeleteRows = false;
            this.gridVirtualNetworks.AllowUserToResizeRows = false;
            this.gridVirtualNetworks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridVirtualNetworks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridVirtualNetworks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVirtualNetwork});
            this.gridVirtualNetworks.Location = new System.Drawing.Point(24, 79);
            this.gridVirtualNetworks.Name = "gridVirtualNetworks";
            this.gridVirtualNetworks.ReadOnly = true;
            this.gridVirtualNetworks.RowHeadersVisible = false;
            this.gridVirtualNetworks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridVirtualNetworks.Size = new System.Drawing.Size(232, 419);
            this.gridVirtualNetworks.TabIndex = 13;
            this.gridVirtualNetworks.SelectionChanged += new System.EventHandler(this.gridVirtualNetworks_SelectionChanged);
            // 
            // colVirtualNetwork
            // 
            this.colVirtualNetwork.FillWeight = 213F;
            this.colVirtualNetwork.HeaderText = "Virtual Networks";
            this.colVirtualNetwork.Name = "colVirtualNetwork";
            this.colVirtualNetwork.ReadOnly = true;
            this.colVirtualNetwork.Width = 213;
            // 
            // gridStorageAccounts
            // 
            this.gridStorageAccounts.AllowUserToAddRows = false;
            this.gridStorageAccounts.AllowUserToDeleteRows = false;
            this.gridStorageAccounts.AllowUserToResizeRows = false;
            this.gridStorageAccounts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridStorageAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStorageAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStorageAccount});
            this.gridStorageAccounts.Location = new System.Drawing.Point(262, 79);
            this.gridStorageAccounts.Name = "gridStorageAccounts";
            this.gridStorageAccounts.ReadOnly = true;
            this.gridStorageAccounts.RowHeadersVisible = false;
            this.gridStorageAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStorageAccounts.Size = new System.Drawing.Size(232, 419);
            this.gridStorageAccounts.TabIndex = 14;
            this.gridStorageAccounts.SelectionChanged += new System.EventHandler(this.gridStorageAccounts_SelectionChanged);
            // 
            // colStorageAccount
            // 
            this.colStorageAccount.FillWeight = 213F;
            this.colStorageAccount.HeaderText = "Storage Accounts";
            this.colStorageAccount.Name = "colStorageAccount";
            this.colStorageAccount.ReadOnly = true;
            this.colStorageAccount.Width = 213;
            // 
            // gridVirtualMachines
            // 
            this.gridVirtualMachines.AllowUserToAddRows = false;
            this.gridVirtualMachines.AllowUserToDeleteRows = false;
            this.gridVirtualMachines.AllowUserToResizeRows = false;
            this.gridVirtualMachines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridVirtualMachines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridVirtualMachines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCloudService,
            this.colVirtualMachine,
            this.colDeploymentName,
            this.colVirtualNetworkName});
            this.gridVirtualMachines.Location = new System.Drawing.Point(500, 79);
            this.gridVirtualMachines.Name = "gridVirtualMachines";
            this.gridVirtualMachines.ReadOnly = true;
            this.gridVirtualMachines.RowHeadersVisible = false;
            this.gridVirtualMachines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridVirtualMachines.Size = new System.Drawing.Size(446, 419);
            this.gridVirtualMachines.TabIndex = 15;
            this.gridVirtualMachines.SelectionChanged += new System.EventHandler(this.gridVirtualMachines_SelectionChanged);
            // 
            // colCloudService
            // 
            this.colCloudService.FillWeight = 213F;
            this.colCloudService.HeaderText = "Cloud Service";
            this.colCloudService.Name = "colCloudService";
            this.colCloudService.ReadOnly = true;
            this.colCloudService.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCloudService.Width = 213;
            // 
            // colVirtualMachine
            // 
            this.colVirtualMachine.FillWeight = 213F;
            this.colVirtualMachine.HeaderText = "Virtual Machines";
            this.colVirtualMachine.Name = "colVirtualMachine";
            this.colVirtualMachine.ReadOnly = true;
            this.colVirtualMachine.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colVirtualMachine.Width = 213;
            // 
            // colDeploymentName
            // 
            this.colDeploymentName.HeaderText = "Deployment Name";
            this.colDeploymentName.Name = "colDeploymentName";
            this.colDeploymentName.ReadOnly = true;
            this.colDeploymentName.Visible = false;
            // 
            // colVirtualNetworkName
            // 
            this.colVirtualNetworkName.HeaderText = "Virtual Network Name";
            this.colVirtualNetworkName.Name = "colVirtualNetworkName";
            this.colVirtualNetworkName.ReadOnly = true;
            this.colVirtualNetworkName.Visible = false;
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(618, 514);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(328, 23);
            this.btnExport.TabIndex = 25;
            this.btnExport.Text = "Export 0 objects";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Enabled = false;
            this.btnChoosePath.Location = new System.Drawing.Point(443, 514);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(29, 23);
            this.btnChoosePath.TabIndex = 28;
            this.btnChoosePath.Text = "...";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.btnChoosePath_Click);
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Enabled = false;
            this.txtDestinationFolder.Location = new System.Drawing.Point(101, 516);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(336, 20);
            this.txtDestinationFolder.TabIndex = 27;
            this.txtDestinationFolder.TextChanged += new System.EventHandler(this.txtDestinationFolder_TextChanged);
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(21, 519);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(74, 13);
            this.lblOutputFolder.TabIndex = 26;
            this.lblOutputFolder.Text = "Output Folder:";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 565);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(970, 22);
            this.statusStrip1.TabIndex = 29;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // chkAllowTelemetry
            // 
            this.chkAllowTelemetry.AutoSize = true;
            this.chkAllowTelemetry.Checked = true;
            this.chkAllowTelemetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowTelemetry.Location = new System.Drawing.Point(101, 542);
            this.chkAllowTelemetry.Name = "chkAllowTelemetry";
            this.chkAllowTelemetry.Size = new System.Drawing.Size(144, 17);
            this.chkAllowTelemetry.TabIndex = 31;
            this.chkAllowTelemetry.Text = "Allow telemetry collection";
            this.chkAllowTelemetry.UseVisualStyleBackColor = true;
            this.chkAllowTelemetry.CheckedChanged += new System.EventHandler(this.chkAllowTelemetry_CheckedChanged);
            // 
            // cmbSubscriptions
            // 
            this.cmbSubscriptions.Enabled = false;
            this.cmbSubscriptions.FormattingEnabled = true;
            this.cmbSubscriptions.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmbSubscriptions.Location = new System.Drawing.Point(480, 20);
            this.cmbSubscriptions.MaxDropDownItems = 15;
            this.cmbSubscriptions.Name = "cmbSubscriptions";
            this.cmbSubscriptions.Size = new System.Drawing.Size(466, 21);
            this.cmbSubscriptions.TabIndex = 32;
            this.cmbSubscriptions.SelectedIndexChanged += new System.EventHandler(this.cmbSubscriptions_SelectedIndexChanged);
            // 
            // lblSubscriptions
            // 
            this.lblSubscriptions.AutoSize = true;
            this.lblSubscriptions.Location = new System.Drawing.Point(404, 23);
            this.lblSubscriptions.Name = "lblSubscriptions";
            this.lblSubscriptions.Size = new System.Drawing.Size(70, 13);
            this.lblSubscriptions.TabIndex = 33;
            this.lblSubscriptions.Text = "Subscriptions";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 587);
            this.Controls.Add(this.lblSubscriptions);
            this.Controls.Add(this.cmbSubscriptions);
            this.Controls.Add(this.chkAllowTelemetry);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.txtDestinationFolder);
            this.Controls.Add(this.lblOutputFolder);
            this.Controls.Add(this.gridVirtualMachines);
            this.Controls.Add(this.gridStorageAccounts);
            this.Controls.Add(this.gridVirtualNetworks);
            this.Controls.Add(this.lblToken);
            this.Controls.Add(this.lblReturnURLs);
            this.Controls.Add(this.txtReturnURLs);
            this.Controls.Add(this.lblClientID);
            this.Controls.Add(this.txtClientID);
            this.Controls.Add(this.btnGetToken);
            this.Controls.Add(this.lblTenantID);
            this.Controls.Add(this.txtTenantID);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Window";
            this.Text = "ASM to ARM Tool";
            this.Load += new System.EventHandler(this.Window_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridVirtualNetworks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridStorageAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridVirtualMachines)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTenantID;
        private System.Windows.Forms.Label lblTenantID;
        private System.Windows.Forms.Button btnGetToken;
        private System.Windows.Forms.Label lblClientID;
        private System.Windows.Forms.TextBox txtClientID;
        private System.Windows.Forms.Label lblReturnURLs;
        private System.Windows.Forms.TextBox txtReturnURLs;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.DataGridView gridVirtualNetworks;
        private System.Windows.Forms.DataGridView gridStorageAccounts;
        private System.Windows.Forms.DataGridView gridVirtualMachines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVirtualNetwork;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStorageAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCloudService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVirtualMachine;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeploymentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVirtualNetworkName;
        private System.Windows.Forms.CheckBox chkAllowTelemetry;
        private System.Windows.Forms.ComboBox cmbSubscriptions;
        private System.Windows.Forms.Label lblSubscriptions;
    }
}

