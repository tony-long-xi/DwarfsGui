namespace DwarfsGui
{
    partial class FrmDwarfs
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDwarfs));
            tabControl = new TabControl();
            tabCreate = new TabPage();
            label15 = new Label();
            btnBrowseWinFsp = new Button();
            txtWinFspPath = new TextBox();
            label14 = new Label();
            lblCreateProgressStatus = new Label();
            label1 = new Label();
            progressCreate = new ProgressBar();
            btnCreate = new Button();
            cmbCompressionAlgo = new ComboBox();
            label16 = new Label();
            chkDisableDedup = new CheckBox();
            cmbCompressionLevel = new ComboBox();
            chkForce = new CheckBox();
            nudWorkers = new NumericUpDown();
            label4 = new Label();
            label3 = new Label();
            btnBrowseCreateOutput = new Button();
            btnBrowseCreateInput = new Button();
            txtCreateOutput = new TextBox();
            txtCreateInput = new TextBox();
            label2 = new Label();
            tabMount = new TabPage();
            label21 = new Label();
            textMount = new TextBox();
            lblMountProgressStatus = new Label();
            progressMount = new ProgressBar();
            txtReadAhead = new TextBox();
            label9 = new Label();
            btnBrowseMountPoint = new Button();
            btnBrowseMountImage = new Button();
            btnMount = new Button();
            txtCacheSize = new TextBox();
            label8 = new Label();
            nudMountWorkers = new NumericUpDown();
            label5 = new Label();
            txtMountPoint = new TextBox();
            txtMountImage = new TextBox();
            label6 = new Label();
            label7 = new Label();
            btnOpenMountPoint = new Button();
            btnUnmount = new Button();
            lvMounted = new ListView();
            col1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            tabExtract = new TabPage();
            txtExtractCacheSize = new TextBox();
            label13 = new Label();
            lblExtractProgressStatus = new Label();
            progressExtract = new ProgressBar();
            btnExtract = new Button();
            chkContinueOnError = new CheckBox();
            nudExtractWorkers = new NumericUpDown();
            label10 = new Label();
            btnBrowseExtractOutput = new Button();
            btnBrowseExtractInput = new Button();
            txtExtractOutput = new TextBox();
            txtExtractInput = new TextBox();
            label11 = new Label();
            label12 = new Label();
            tabReg = new TabPage();
            linkLabel3 = new LinkLabel();
            label20 = new Label();
            linkLabel2 = new LinkLabel();
            linkLabel1 = new LinkLabel();
            label19 = new Label();
            label18 = new Label();
            label17 = new Label();
            lblContextMenuStatus = new Label();
            btnUnregisterContextMenu = new Button();
            btnRegisterContextMenu = new Button();
            btnLog = new Button();
            splitContainer1 = new SplitContainer();
            lstLog = new RichTextBox();
            contextMenuLog = new ContextMenuStrip(components);
            copyLogItem = new ToolStripMenuItem();
            tabControl.SuspendLayout();
            tabCreate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudWorkers).BeginInit();
            tabMount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMountWorkers).BeginInit();
            tabExtract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudExtractWorkers).BeginInit();
            tabReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuLog.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabCreate);
            tabControl.Controls.Add(tabMount);
            tabControl.Controls.Add(tabExtract);
            tabControl.Controls.Add(tabReg);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1048, 414);
            tabControl.TabIndex = 0;
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            // 
            // tabCreate
            // 
            tabCreate.Controls.Add(label15);
            tabCreate.Controls.Add(btnBrowseWinFsp);
            tabCreate.Controls.Add(txtWinFspPath);
            tabCreate.Controls.Add(label14);
            tabCreate.Controls.Add(lblCreateProgressStatus);
            tabCreate.Controls.Add(label1);
            tabCreate.Controls.Add(progressCreate);
            tabCreate.Controls.Add(btnCreate);
            tabCreate.Controls.Add(cmbCompressionAlgo);
            tabCreate.Controls.Add(label16);
            tabCreate.Controls.Add(chkDisableDedup);
            tabCreate.Controls.Add(cmbCompressionLevel);
            tabCreate.Controls.Add(chkForce);
            tabCreate.Controls.Add(nudWorkers);
            tabCreate.Controls.Add(label4);
            tabCreate.Controls.Add(label3);
            tabCreate.Controls.Add(btnBrowseCreateOutput);
            tabCreate.Controls.Add(btnBrowseCreateInput);
            tabCreate.Controls.Add(txtCreateOutput);
            tabCreate.Controls.Add(txtCreateInput);
            tabCreate.Controls.Add(label2);
            tabCreate.Location = new Point(4, 33);
            tabCreate.Name = "tabCreate";
            tabCreate.Padding = new Padding(3);
            tabCreate.Size = new Size(1040, 377);
            tabCreate.TabIndex = 0;
            tabCreate.Text = "制作镜像";
            tabCreate.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(25, 298);
            label15.Name = "label15";
            label15.Size = new Size(466, 24);
            label15.TabIndex = 16;
            label15.Text = "WinFsp必须提前安装，安装包在程序目录的DwarfsBin中";
            // 
            // btnBrowseWinFsp
            // 
            btnBrowseWinFsp.Location = new Point(884, 244);
            btnBrowseWinFsp.Name = "btnBrowseWinFsp";
            btnBrowseWinFsp.Size = new Size(112, 34);
            btnBrowseWinFsp.TabIndex = 15;
            btnBrowseWinFsp.Text = "浏览...";
            btnBrowseWinFsp.UseVisualStyleBackColor = true;
            btnBrowseWinFsp.Click += btnBrowseWinFsp_Click;
            // 
            // txtWinFspPath
            // 
            txtWinFspPath.Location = new Point(158, 246);
            txtWinFspPath.Name = "txtWinFspPath";
            txtWinFspPath.Size = new Size(697, 30);
            txtWinFspPath.TabIndex = 14;
            txtWinFspPath.Text = "C:\\Program Files (x86)\\WinFsp\\bin";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(24, 249);
            label14.Name = "label14";
            label14.Size = new Size(110, 24);
            label14.TabIndex = 13;
            label14.Text = "WinFsp目录";
            // 
            // lblCreateProgressStatus
            // 
            lblCreateProgressStatus.AutoSize = true;
            lblCreateProgressStatus.ForeColor = Color.DimGray;
            lblCreateProgressStatus.Location = new Point(24, 182);
            lblCreateProgressStatus.Name = "lblCreateProgressStatus";
            lblCreateProgressStatus.Size = new Size(0, 24);
            lblCreateProgressStatus.TabIndex = 17;
            lblCreateProgressStatus.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 19);
            label1.Name = "label1";
            label1.Size = new Size(82, 24);
            label1.TabIndex = 0;
            label1.Text = "文件路径";
            // 
            // progressCreate
            // 
            progressCreate.Location = new Point(24, 212);
            progressCreate.Name = "progressCreate";
            progressCreate.Size = new Size(972, 20);
            progressCreate.TabIndex = 0;
            progressCreate.Visible = false;
            // 
            // btnCreate
            // 
            btnCreate.BackColor = Color.FromArgb(39, 174, 96);
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.ForeColor = Color.White;
            btnCreate.Location = new Point(884, 103);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(112, 34);
            btnCreate.TabIndex = 12;
            btnCreate.Text = "制作镜像";
            btnCreate.UseVisualStyleBackColor = false;
            btnCreate.Click += btnCreate_Click;
            // 
            // cmbCompressionAlgo
            // 
            cmbCompressionAlgo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCompressionAlgo.FormattingEnabled = true;
            cmbCompressionAlgo.Items.AddRange(new object[] { "(默认)", "lz4", "lz4hc:level=9", "null", "lzma:level=9" });
            cmbCompressionAlgo.Location = new Point(119, 138);
            cmbCompressionAlgo.Name = "cmbCompressionAlgo";
            cmbCompressionAlgo.Size = new Size(200, 32);
            cmbCompressionAlgo.TabIndex = 16;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(24, 141);
            label16.Name = "label16";
            label16.Size = new Size(82, 24);
            label16.TabIndex = 15;
            label16.Text = "压缩算法";
            // 
            // chkDisableDedup
            // 
            chkDisableDedup.AutoSize = true;
            chkDisableDedup.Location = new Point(545, 139);
            chkDisableDedup.Name = "chkDisableDedup";
            chkDisableDedup.Size = new Size(108, 28);
            chkDisableDedup.TabIndex = 17;
            chkDisableDedup.Text = "禁用去重";
            chkDisableDedup.UseVisualStyleBackColor = true;
            // 
            // cmbCompressionLevel
            // 
            cmbCompressionLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCompressionLevel.FormattingEnabled = true;
            cmbCompressionLevel.Items.AddRange(new object[] { "0 - 极速临时打包", "1 - 快速备份", "2 - 日常使用", "3 - 兼顾速度与压缩", "4 - 较好压缩率", "5 - 代码/小文件专用", "6 - 高压缩归档", "7 - 极致压缩(默认)" });
            cmbCompressionLevel.Location = new Point(119, 100);
            cmbCompressionLevel.Name = "cmbCompressionLevel";
            cmbCompressionLevel.Size = new Size(200, 32);
            cmbCompressionLevel.TabIndex = 7;
            // 
            // chkForce
            // 
            chkForce.AutoSize = true;
            chkForce.Location = new Point(545, 101);
            chkForce.Name = "chkForce";
            chkForce.Size = new Size(144, 28);
            chkForce.TabIndex = 11;
            chkForce.Text = "覆盖已有镜像";
            chkForce.UseVisualStyleBackColor = true;
            // 
            // nudWorkers
            // 
            nudWorkers.Location = new Point(440, 100);
            nudWorkers.Name = "nudWorkers";
            nudWorkers.Size = new Size(71, 30);
            nudWorkers.TabIndex = 9;
            nudWorkers.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(352, 103);
            label4.Name = "label4";
            label4.Size = new Size(82, 24);
            label4.TabIndex = 8;
            label4.Text = "工作线程";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 103);
            label3.Name = "label3";
            label3.Size = new Size(82, 24);
            label3.TabIndex = 6;
            label3.Text = "压缩级别";
            // 
            // btnBrowseCreateOutput
            // 
            btnBrowseCreateOutput.Location = new Point(884, 56);
            btnBrowseCreateOutput.Name = "btnBrowseCreateOutput";
            btnBrowseCreateOutput.Size = new Size(112, 34);
            btnBrowseCreateOutput.TabIndex = 5;
            btnBrowseCreateOutput.Text = "浏览...";
            btnBrowseCreateOutput.UseVisualStyleBackColor = true;
            btnBrowseCreateOutput.Click += btnBrowseCreateOutput_Click;
            // 
            // btnBrowseCreateInput
            // 
            btnBrowseCreateInput.Location = new Point(884, 14);
            btnBrowseCreateInput.Name = "btnBrowseCreateInput";
            btnBrowseCreateInput.Size = new Size(112, 34);
            btnBrowseCreateInput.TabIndex = 2;
            btnBrowseCreateInput.Text = "浏览...";
            btnBrowseCreateInput.UseVisualStyleBackColor = true;
            btnBrowseCreateInput.Click += btnBrowseCreateInput_Click;
            // 
            // txtCreateOutput
            // 
            txtCreateOutput.Location = new Point(119, 58);
            txtCreateOutput.Name = "txtCreateOutput";
            txtCreateOutput.Size = new Size(736, 30);
            txtCreateOutput.TabIndex = 4;
            // 
            // txtCreateInput
            // 
            txtCreateInput.Location = new Point(119, 16);
            txtCreateInput.Name = "txtCreateInput";
            txtCreateInput.Size = new Size(736, 30);
            txtCreateInput.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 61);
            label2.Name = "label2";
            label2.Size = new Size(82, 24);
            label2.TabIndex = 3;
            label2.Text = "镜像保存";
            // 
            // tabMount
            // 
            tabMount.Controls.Add(label21);
            tabMount.Controls.Add(textMount);
            tabMount.Controls.Add(lblMountProgressStatus);
            tabMount.Controls.Add(progressMount);
            tabMount.Controls.Add(txtReadAhead);
            tabMount.Controls.Add(label9);
            tabMount.Controls.Add(btnBrowseMountPoint);
            tabMount.Controls.Add(btnBrowseMountImage);
            tabMount.Controls.Add(btnMount);
            tabMount.Controls.Add(txtCacheSize);
            tabMount.Controls.Add(label8);
            tabMount.Controls.Add(nudMountWorkers);
            tabMount.Controls.Add(label5);
            tabMount.Controls.Add(txtMountPoint);
            tabMount.Controls.Add(txtMountImage);
            tabMount.Controls.Add(label6);
            tabMount.Controls.Add(label7);
            tabMount.Controls.Add(btnOpenMountPoint);
            tabMount.Controls.Add(btnUnmount);
            tabMount.Controls.Add(lvMounted);
            tabMount.Location = new Point(4, 33);
            tabMount.Name = "tabMount";
            tabMount.Padding = new Padding(3);
            tabMount.Size = new Size(1040, 377);
            tabMount.TabIndex = 1;
            tabMount.Text = "挂载镜像";
            tabMount.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(651, 259);
            label21.Name = "label21";
            label21.Size = new Size(100, 24);
            label21.TabIndex = 19;
            label21.Text = "虚拟目录名";
            // 
            // textMount
            // 
            textMount.Location = new Point(757, 256);
            textMount.Name = "textMount";
            textMount.Size = new Size(98, 30);
            textMount.TabIndex = 5;
            textMount.Text = "mount";
            // 
            // lblMountProgressStatus
            // 
            lblMountProgressStatus.AutoSize = true;
            lblMountProgressStatus.ForeColor = Color.DimGray;
            lblMountProgressStatus.Location = new Point(24, 313);
            lblMountProgressStatus.Name = "lblMountProgressStatus";
            lblMountProgressStatus.Size = new Size(0, 24);
            lblMountProgressStatus.TabIndex = 17;
            lblMountProgressStatus.Visible = false;
            // 
            // progressMount
            // 
            progressMount.Location = new Point(24, 344);
            progressMount.Name = "progressMount";
            progressMount.Size = new Size(972, 20);
            progressMount.TabIndex = 16;
            progressMount.Visible = false;
            // 
            // txtReadAhead
            // 
            txtReadAhead.Location = new Point(511, 298);
            txtReadAhead.Name = "txtReadAhead";
            txtReadAhead.Size = new Size(93, 30);
            txtReadAhead.TabIndex = 12;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(423, 301);
            label9.Name = "label9";
            label9.Size = new Size(82, 24);
            label9.TabIndex = 11;
            label9.Text = "预读大小";
            // 
            // btnBrowseMountPoint
            // 
            btnBrowseMountPoint.Location = new Point(881, 254);
            btnBrowseMountPoint.Name = "btnBrowseMountPoint";
            btnBrowseMountPoint.Size = new Size(112, 34);
            btnBrowseMountPoint.TabIndex = 6;
            btnBrowseMountPoint.Text = "浏览...";
            btnBrowseMountPoint.UseVisualStyleBackColor = true;
            btnBrowseMountPoint.Click += btnBrowseMountPoint_Click;
            // 
            // btnBrowseMountImage
            // 
            btnBrowseMountImage.Location = new Point(884, 212);
            btnBrowseMountImage.Name = "btnBrowseMountImage";
            btnBrowseMountImage.Size = new Size(112, 34);
            btnBrowseMountImage.TabIndex = 3;
            btnBrowseMountImage.Text = "浏览...";
            btnBrowseMountImage.UseVisualStyleBackColor = true;
            btnBrowseMountImage.Click += btnBrowseMountImage_Click;
            // 
            // btnMount
            // 
            btnMount.BackColor = Color.FromArgb(41, 128, 185);
            btnMount.FlatAppearance.BorderSize = 0;
            btnMount.FlatStyle = FlatStyle.Flat;
            btnMount.ForeColor = Color.White;
            btnMount.Location = new Point(881, 296);
            btnMount.Name = "btnMount";
            btnMount.Size = new Size(112, 34);
            btnMount.TabIndex = 13;
            btnMount.Text = "挂载镜像";
            btnMount.UseVisualStyleBackColor = false;
            btnMount.Click += btnMount_Click;
            // 
            // txtCacheSize
            // 
            txtCacheSize.Location = new Point(304, 298);
            txtCacheSize.Name = "txtCacheSize";
            txtCacheSize.Size = new Size(93, 30);
            txtCacheSize.TabIndex = 10;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(216, 301);
            label8.Name = "label8";
            label8.Size = new Size(82, 24);
            label8.TabIndex = 9;
            label8.Text = "缓存大小";
            // 
            // nudMountWorkers
            // 
            nudMountWorkers.Location = new Point(119, 298);
            nudMountWorkers.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
            nudMountWorkers.Name = "nudMountWorkers";
            nudMountWorkers.Size = new Size(71, 30);
            nudMountWorkers.TabIndex = 8;
            nudMountWorkers.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(24, 301);
            label5.Name = "label5";
            label5.Size = new Size(82, 24);
            label5.TabIndex = 7;
            label5.Text = "工作线程";
            // 
            // txtMountPoint
            // 
            txtMountPoint.Location = new Point(119, 256);
            txtMountPoint.Name = "txtMountPoint";
            txtMountPoint.PlaceholderText = "留空自动挂载";
            txtMountPoint.Size = new Size(524, 30);
            txtMountPoint.TabIndex = 4;
            // 
            // txtMountImage
            // 
            txtMountImage.Location = new Point(119, 214);
            txtMountImage.Name = "txtMountImage";
            txtMountImage.Size = new Size(736, 30);
            txtMountImage.TabIndex = 2;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(24, 259);
            label6.Name = "label6";
            label6.Size = new Size(64, 24);
            label6.TabIndex = 6;
            label6.Text = "挂载点";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(24, 217);
            label7.Name = "label7";
            label7.Size = new Size(82, 24);
            label7.TabIndex = 3;
            label7.Text = "镜像文件";
            // 
            // btnOpenMountPoint
            // 
            btnOpenMountPoint.BackColor = Color.FromArgb(52, 152, 219);
            btnOpenMountPoint.FlatAppearance.BorderSize = 0;
            btnOpenMountPoint.FlatStyle = FlatStyle.Flat;
            btnOpenMountPoint.ForeColor = Color.White;
            btnOpenMountPoint.Location = new Point(155, 169);
            btnOpenMountPoint.Name = "btnOpenMountPoint";
            btnOpenMountPoint.Size = new Size(112, 34);
            btnOpenMountPoint.TabIndex = 1;
            btnOpenMountPoint.Text = "打开挂载点";
            btnOpenMountPoint.UseVisualStyleBackColor = false;
            btnOpenMountPoint.Click += btnOpenMountPoint_Click;
            // 
            // btnUnmount
            // 
            btnUnmount.BackColor = Color.FromArgb(231, 76, 60);
            btnUnmount.FlatAppearance.BorderSize = 0;
            btnUnmount.FlatStyle = FlatStyle.Flat;
            btnUnmount.ForeColor = Color.White;
            btnUnmount.Location = new Point(24, 169);
            btnUnmount.Name = "btnUnmount";
            btnUnmount.Size = new Size(112, 34);
            btnUnmount.TabIndex = 0;
            btnUnmount.Text = "卸载镜像";
            btnUnmount.UseVisualStyleBackColor = false;
            btnUnmount.Click += btnUnmount_Click;
            // 
            // lvMounted
            // 
            lvMounted.Columns.AddRange(new ColumnHeader[] { col1, columnHeader2, columnHeader3 });
            lvMounted.FullRowSelect = true;
            lvMounted.GridLines = true;
            lvMounted.Location = new Point(24, 14);
            lvMounted.Name = "lvMounted";
            lvMounted.Size = new Size(983, 149);
            lvMounted.TabIndex = 0;
            lvMounted.UseCompatibleStateImageBehavior = false;
            lvMounted.View = View.Details;
            // 
            // col1
            // 
            col1.Text = "镜像路径";
            col1.Width = 500;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "挂载点";
            columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "挂载时间";
            columnHeader3.Width = 255;
            // 
            // tabExtract
            // 
            tabExtract.Controls.Add(txtExtractCacheSize);
            tabExtract.Controls.Add(label13);
            tabExtract.Controls.Add(lblExtractProgressStatus);
            tabExtract.Controls.Add(progressExtract);
            tabExtract.Controls.Add(btnExtract);
            tabExtract.Controls.Add(chkContinueOnError);
            tabExtract.Controls.Add(nudExtractWorkers);
            tabExtract.Controls.Add(label10);
            tabExtract.Controls.Add(btnBrowseExtractOutput);
            tabExtract.Controls.Add(btnBrowseExtractInput);
            tabExtract.Controls.Add(txtExtractOutput);
            tabExtract.Controls.Add(txtExtractInput);
            tabExtract.Controls.Add(label11);
            tabExtract.Controls.Add(label12);
            tabExtract.Location = new Point(4, 33);
            tabExtract.Name = "tabExtract";
            tabExtract.Size = new Size(1040, 377);
            tabExtract.TabIndex = 2;
            tabExtract.Text = "提取镜像";
            tabExtract.UseVisualStyleBackColor = true;
            // 
            // txtExtractCacheSize
            // 
            txtExtractCacheSize.Location = new Point(321, 100);
            txtExtractCacheSize.Name = "txtExtractCacheSize";
            txtExtractCacheSize.Size = new Size(93, 30);
            txtExtractCacheSize.TabIndex = 14;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(233, 103);
            label13.Name = "label13";
            label13.Size = new Size(82, 24);
            label13.TabIndex = 13;
            label13.Text = "缓存大小";
            // 
            // lblExtractProgressStatus
            // 
            lblExtractProgressStatus.AutoSize = true;
            lblExtractProgressStatus.ForeColor = Color.DimGray;
            lblExtractProgressStatus.Location = new Point(24, 113);
            lblExtractProgressStatus.Name = "lblExtractProgressStatus";
            lblExtractProgressStatus.Size = new Size(0, 24);
            lblExtractProgressStatus.TabIndex = 15;
            lblExtractProgressStatus.Visible = false;
            // 
            // progressExtract
            // 
            progressExtract.Location = new Point(24, 143);
            progressExtract.Name = "progressExtract";
            progressExtract.Size = new Size(972, 30);
            progressExtract.TabIndex = 10;
            progressExtract.Visible = false;
            // 
            // btnExtract
            // 
            btnExtract.BackColor = Color.FromArgb(142, 68, 173);
            btnExtract.FlatAppearance.BorderSize = 0;
            btnExtract.FlatStyle = FlatStyle.Flat;
            btnExtract.ForeColor = Color.White;
            btnExtract.Location = new Point(884, 98);
            btnExtract.Name = "btnExtract";
            btnExtract.Size = new Size(112, 34);
            btnExtract.TabIndex = 0;
            btnExtract.Text = "提取镜像";
            btnExtract.UseVisualStyleBackColor = false;
            btnExtract.Click += btnExtract_Click;
            // 
            // chkContinueOnError
            // 
            chkContinueOnError.AutoSize = true;
            chkContinueOnError.Location = new Point(464, 101);
            chkContinueOnError.Name = "chkContinueOnError";
            chkContinueOnError.Size = new Size(144, 28);
            chkContinueOnError.TabIndex = 9;
            chkContinueOnError.Text = "遇到错误继续";
            chkContinueOnError.UseVisualStyleBackColor = true;
            // 
            // nudExtractWorkers
            // 
            nudExtractWorkers.Location = new Point(119, 100);
            nudExtractWorkers.Name = "nudExtractWorkers";
            nudExtractWorkers.Size = new Size(71, 30);
            nudExtractWorkers.TabIndex = 7;
            nudExtractWorkers.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(24, 103);
            label10.Name = "label10";
            label10.Size = new Size(82, 24);
            label10.TabIndex = 6;
            label10.Text = "工作线程";
            // 
            // btnBrowseExtractOutput
            // 
            btnBrowseExtractOutput.Location = new Point(884, 56);
            btnBrowseExtractOutput.Name = "btnBrowseExtractOutput";
            btnBrowseExtractOutput.Size = new Size(112, 34);
            btnBrowseExtractOutput.TabIndex = 5;
            btnBrowseExtractOutput.Text = "浏览...";
            btnBrowseExtractOutput.UseVisualStyleBackColor = true;
            btnBrowseExtractOutput.Click += btnBrowseExtractOutput_Click;
            // 
            // btnBrowseExtractInput
            // 
            btnBrowseExtractInput.Location = new Point(884, 14);
            btnBrowseExtractInput.Name = "btnBrowseExtractInput";
            btnBrowseExtractInput.Size = new Size(112, 34);
            btnBrowseExtractInput.TabIndex = 2;
            btnBrowseExtractInput.Text = "浏览...";
            btnBrowseExtractInput.UseVisualStyleBackColor = true;
            btnBrowseExtractInput.Click += btnBrowseExtractInput_Click;
            // 
            // txtExtractOutput
            // 
            txtExtractOutput.Location = new Point(119, 58);
            txtExtractOutput.Name = "txtExtractOutput";
            txtExtractOutput.Size = new Size(736, 30);
            txtExtractOutput.TabIndex = 4;
            // 
            // txtExtractInput
            // 
            txtExtractInput.Location = new Point(119, 16);
            txtExtractInput.Name = "txtExtractInput";
            txtExtractInput.Size = new Size(736, 30);
            txtExtractInput.TabIndex = 1;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(24, 61);
            label11.Name = "label11";
            label11.Size = new Size(82, 24);
            label11.TabIndex = 3;
            label11.Text = "镜像保存";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(24, 19);
            label12.Name = "label12";
            label12.Size = new Size(82, 24);
            label12.TabIndex = 0;
            label12.Text = "文件路径";
            // 
            // tabReg
            // 
            tabReg.Controls.Add(linkLabel3);
            tabReg.Controls.Add(label20);
            tabReg.Controls.Add(linkLabel2);
            tabReg.Controls.Add(linkLabel1);
            tabReg.Controls.Add(label19);
            tabReg.Controls.Add(label18);
            tabReg.Controls.Add(label17);
            tabReg.Controls.Add(lblContextMenuStatus);
            tabReg.Controls.Add(btnUnregisterContextMenu);
            tabReg.Controls.Add(btnRegisterContextMenu);
            tabReg.Location = new Point(4, 33);
            tabReg.Name = "tabReg";
            tabReg.Size = new Size(1040, 377);
            tabReg.TabIndex = 3;
            tabReg.Text = "右键菜单";
            tabReg.UseVisualStyleBackColor = true;
            // 
            // linkLabel3
            // 
            linkLabel3.AutoSize = true;
            linkLabel3.Location = new Point(139, 264);
            linkLabel3.Name = "linkLabel3";
            linkLabel3.Size = new Size(386, 24);
            linkLabel3.TabIndex = 9;
            linkLabel3.TabStop = true;
            linkLabel3.Text = "https://github.com/tony-long-xi/DwarfsGui";
            linkLabel3.LinkClicked += linkLabel_LinkClicked;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(35, 264);
            label20.Name = "label20";
            label20.Size = new Size(100, 24);
            label20.TabIndex = 8;
            label20.Text = "当前软件：";
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Location = new Point(139, 217);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(302, 24);
            linkLabel2.TabIndex = 7;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "https://github.com/winfsp/winfsp";
            linkLabel2.LinkClicked += linkLabel_LinkClicked;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(139, 170);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(284, 24);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "https://github.com/mhx/dwarfs";
            linkLabel1.LinkClicked += linkLabel_LinkClicked;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(35, 217);
            label19.Name = "label19";
            label19.Size = new Size(92, 24);
            label19.TabIndex = 5;
            label19.Text = "WinFsp：";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(35, 170);
            label18.Name = "label18";
            label18.Size = new Size(85, 24);
            label18.TabIndex = 4;
            label18.Text = "dwarfs：";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(35, 123);
            label17.Name = "label17";
            label17.Size = new Size(280, 24);
            label17.TabIndex = 3;
            label17.Text = "免费软件       Made By Tony.Tian";
            // 
            // lblContextMenuStatus
            // 
            lblContextMenuStatus.AutoSize = true;
            lblContextMenuStatus.ForeColor = Color.DimGray;
            lblContextMenuStatus.Location = new Point(24, 76);
            lblContextMenuStatus.Name = "lblContextMenuStatus";
            lblContextMenuStatus.Size = new Size(0, 24);
            lblContextMenuStatus.TabIndex = 2;
            // 
            // btnUnregisterContextMenu
            // 
            btnUnregisterContextMenu.BackColor = Color.FromArgb(231, 76, 60);
            btnUnregisterContextMenu.FlatAppearance.BorderSize = 0;
            btnUnregisterContextMenu.FlatStyle = FlatStyle.Flat;
            btnUnregisterContextMenu.Location = new Point(159, 19);
            btnUnregisterContextMenu.Name = "btnUnregisterContextMenu";
            btnUnregisterContextMenu.Size = new Size(112, 34);
            btnUnregisterContextMenu.TabIndex = 1;
            btnUnregisterContextMenu.Text = "取消右键";
            btnUnregisterContextMenu.UseVisualStyleBackColor = false;
            btnUnregisterContextMenu.Click += btnUnregisterContextMenu_Click;
            // 
            // btnRegisterContextMenu
            // 
            btnRegisterContextMenu.BackColor = Color.FromArgb(46, 204, 113);
            btnRegisterContextMenu.FlatAppearance.BorderSize = 0;
            btnRegisterContextMenu.FlatStyle = FlatStyle.Flat;
            btnRegisterContextMenu.ForeColor = Color.White;
            btnRegisterContextMenu.Location = new Point(24, 19);
            btnRegisterContextMenu.Name = "btnRegisterContextMenu";
            btnRegisterContextMenu.Size = new Size(112, 34);
            btnRegisterContextMenu.TabIndex = 0;
            btnRegisterContextMenu.Text = "注册右键";
            btnRegisterContextMenu.UseVisualStyleBackColor = false;
            btnRegisterContextMenu.Click += btnRegisterContextMenu_Click;
            // 
            // btnLog
            // 
            btnLog.Location = new Point(4, 4);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(112, 30);
            btnLog.TabIndex = 1;
            btnLog.Text = "清除日志";
            btnLog.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tabControl);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(lstLog);
            splitContainer1.Panel2.Controls.Add(btnLog);
            splitContainer1.Size = new Size(1048, 700);
            splitContainer1.SplitterDistance = 414;
            splitContainer1.TabIndex = 2;
            // 
            // lstLog
            // 
            lstLog.ContextMenuStrip = contextMenuLog;
            lstLog.Dock = DockStyle.Fill;
            lstLog.Font = new Font("Microsoft YaHei UI", 9F);
            lstLog.Location = new Point(0, 0);
            lstLog.Name = "lstLog";
            lstLog.ReadOnly = true;
            lstLog.ScrollBars = RichTextBoxScrollBars.Vertical;
            lstLog.Size = new Size(1048, 282);
            lstLog.TabIndex = 2;
            lstLog.Text = "";
            // 
            // contextMenuLog
            // 
            contextMenuLog.ImageScalingSize = new Size(24, 24);
            contextMenuLog.Items.AddRange(new ToolStripItem[] { copyLogItem });
            contextMenuLog.Name = "contextMenuLog";
            contextMenuLog.Size = new Size(117, 34);
            // 
            // copyLogItem
            // 
            copyLogItem.Name = "copyLogItem";
            copyLogItem.Size = new Size(116, 30);
            copyLogItem.Text = "复制";
            copyLogItem.Click += copyLogItem_Click;
            // 
            // FrmDwarfs
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1048, 700);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDwarfs";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dwarfs镜像管理器 V1.3";
            tabControl.ResumeLayout(false);
            tabCreate.ResumeLayout(false);
            tabCreate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudWorkers).EndInit();
            tabMount.ResumeLayout(false);
            tabMount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMountWorkers).EndInit();
            tabExtract.ResumeLayout(false);
            tabExtract.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudExtractWorkers).EndInit();
            tabReg.ResumeLayout(false);
            tabReg.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuLog.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabCreate;
        private TabPage tabMount;
        private TabPage tabExtract;
        private TabPage tabReg;
        private Label label2;
        private Label label1;
        private Button btnLog;
        private SplitContainer splitContainer1;
        private Button btnBrowseCreateOutput;
        private Button btnBrowseCreateInput;
        private TextBox txtCreateOutput;
        private TextBox txtCreateInput;
        private NumericUpDown nudWorkers;
        private Label label4;
        private Label label3;
        private RichTextBox lstLog;
        private ContextMenuStrip contextMenuLog;
        private CheckBox chkForce;
        private ComboBox cmbCompressionLevel;
        private ComboBox cmbCompressionAlgo;
        private Label label16;
        private CheckBox chkDisableDedup;
        private Button btnCreate;
        private ProgressBar progressCreate;
        private Label lblCreateProgressStatus;
        private ListView lvMounted;
        private ColumnHeader col1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Button btnOpenMountPoint;
        private Button btnUnmount;
        private Button btnBrowseMountPoint;
        private Button btnBrowseMountImage;
        private Button btnMount;
        private TextBox txtCacheSize;
        private Label label8;
        private NumericUpDown nudMountWorkers;
        private Label label5;
        private TextBox txtMountPoint;
        private TextBox txtMountImage;
        private Label label6;
        private Label label7;
        private TextBox txtReadAhead;
        private Label label9;
        private Button btnExtract;
        private CheckBox chkContinueOnError;
        private NumericUpDown nudExtractWorkers;
        private Label label10;
        private Button btnBrowseExtractOutput;
        private Button btnBrowseExtractInput;
        private TextBox txtExtractOutput;
        private TextBox txtExtractInput;
        private Label label11;
        private Label label12;
        private Button btnUnregisterContextMenu;
        private Button btnRegisterContextMenu;
        private ProgressBar progressExtract;
        private Label lblExtractProgressStatus;
        private TextBox txtExtractCacheSize;
        private Label label13;
        private Label lblContextMenuStatus;
        private ProgressBar progressMount;
        private Label lblMountProgressStatus;
        private Button btnBrowseWinFsp;
        private TextBox txtWinFspPath;
        private Label label14;
        private Label label15;
        private Label label20;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel1;
        private Label label19;
        private Label label18;
        private Label label17;
        private LinkLabel linkLabel3;
        private ToolStripMenuItem copyLogItem;
        private Label label21;
        private TextBox textMount;
    }
}
