namespace MIGAZ
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
            this.lblTenantID = new System.Windows.Forms.Label();
            this.btnGetToken = new System.Windows.Forms.Button();
            this.lblToken = new System.Windows.Forms.Label();
            this.gridVirtualNetworks = new System.Windows.Forms.DataGridView();
            this.colVirtualNetwork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridStorageAccounts = new System.Windows.Forms.DataGridView();
            this.colStorageAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridVirtualMachines = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmbSubscriptions = new System.Windows.Forms.ComboBox();
            this.lblSubscriptions = new System.Windows.Forms.Label();
            this.txtTenantID = new System.Windows.Forms.TextBox();
            this.btnOptions = new System.Windows.Forms.Button();
            this.colCloudService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVirtualMachine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeploymentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVirtualNetworkName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoadBalancerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridVirtualNetworks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridStorageAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridVirtualMachines)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.colVirtualNetworkName,
            this.colLoadBalancerName});
            this.gridVirtualMachines.Location = new System.Drawing.Point(500, 79);
            this.gridVirtualMachines.Name = "gridVirtualMachines";
            this.gridVirtualMachines.ReadOnly = true;
            this.gridVirtualMachines.RowHeadersVisible = false;
            this.gridVirtualMachines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridVirtualMachines.Size = new System.Drawing.Size(446, 419);
            this.gridVirtualMachines.TabIndex = 15;
            this.gridVirtualMachines.SelectionChanged += new System.EventHandler(this.gridVirtualMachines_SelectionChanged);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 553);
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
            // txtTenantID
            // 
            this.txtTenantID.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MIGAZ.app.Default, "TenantId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtTenantID.Location = new System.Drawing.Point(135, 21);
            this.txtTenantID.Name = "txtTenantID";
            this.txtTenantID.Size = new System.Drawing.Size(213, 20);
            this.txtTenantID.TabIndex = 0;
            this.txtTenantID.Text = global::MIGAZ.app.Default.TenantId;
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(24, 50);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(129, 23);
            this.btnOptions.TabIndex = 34;
            this.btnOptions.Text = "Options...";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
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
            // colLoadBalancerName
            // 
            this.colLoadBalancerName.HeaderText = "LoadBalancerName";
            this.colLoadBalancerName.Name = "colLoadBalancerName";
            this.colLoadBalancerName.ReadOnly = true;
            this.colLoadBalancerName.Visible = false;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 575);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.lblSubscriptions);
            this.Controls.Add(this.cmbSubscriptions);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.txtDestinationFolder);
            this.Controls.Add(this.lblOutputFolder);
            this.Controls.Add(this.gridVirtualMachines);
            this.Controls.Add(this.gridStorageAccounts);
            this.Controls.Add(this.gridVirtualNetworks);
            this.Controls.Add(this.lblToken);
            this.Controls.Add(this.btnGetToken);
            this.Controls.Add(this.lblTenantID);
            this.Controls.Add(this.txtTenantID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "migAz";
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
        private System.Windows.Forms.ComboBox cmbSubscriptions;
        private System.Windows.Forms.Label lblSubscriptions;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCloudService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVirtualMachine;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeploymentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVirtualNetworkName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoadBalancerName;
    }
}

