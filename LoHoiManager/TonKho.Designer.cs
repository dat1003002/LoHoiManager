namespace LoHoiManager
{
    partial class TonKho
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
            dataTonKho = new DataGridView();
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            groupBox1 = new GroupBox();
            btnClose = new Button();
            btnLoc = new Button();
            btnExecl = new Button();
            panelTinhTong = new Panel();
            panelTong = new Panel();
            btnIn = new Button();
            cbFactory = new ComboBox();
            label4 = new Label();
            dateStop = new DateTimePicker();
            dateStart = new DateTimePicker();
            label3 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataTonKho).BeginInit();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dataTonKho
            // 
            dataTonKho.BackgroundColor = SystemColors.ActiveCaption;
            dataTonKho.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataTonKho.Location = new Point(3, 306);
            dataTonKho.Name = "dataTonKho";
            dataTonKho.RowHeadersWidth = 51;
            dataTonKho.Size = new Size(1277, 516);
            dataTonKho.TabIndex = 0;
            dataTonKho.CellContentClick += dataTonKho_CellContentClick;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Crimson;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1280, 91);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 24F, FontStyle.Bold);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(440, 19);
            label1.Name = "label1";
            label1.Size = new Size(359, 45);
            label1.TabIndex = 0;
            label1.Text = "Danh Sách Tồn Kho";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Crimson;
            panel2.Location = new Point(0, 97);
            panel2.Name = "panel2";
            panel2.Size = new Size(1280, 10);
            panel2.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnClose);
            groupBox1.Controls.Add(btnLoc);
            groupBox1.Controls.Add(btnExecl);
            groupBox1.Controls.Add(panelTinhTong);
            groupBox1.Controls.Add(panelTong);
            groupBox1.Controls.Add(btnIn);
            groupBox1.Controls.Add(cbFactory);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(dateStop);
            groupBox1.Controls.Add(dateStart);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            groupBox1.Location = new Point(12, 113);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1256, 143);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chức Năng Tồn Kho";
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Crimson;
            btnClose.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            btnClose.ForeColor = SystemColors.ButtonHighlight;
            btnClose.Location = new Point(650, 97);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 40);
            btnClose.TabIndex = 10;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.DarkCyan;
            btnLoc.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            btnLoc.ForeColor = SystemColors.ButtonHighlight;
            btnLoc.Location = new Point(650, 54);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(94, 40);
            btnLoc.TabIndex = 9;
            btnLoc.Text = "Lọc";
            btnLoc.UseVisualStyleBackColor = false;
            btnLoc.Click += btnLoc_Click;
            // 
            // btnExecl
            // 
            btnExecl.BackColor = SystemColors.HotTrack;
            btnExecl.Font = new Font("Times New Roman", 9F, FontStyle.Bold);
            btnExecl.ForeColor = SystemColors.ButtonHighlight;
            btnExecl.Location = new Point(650, 13);
            btnExecl.Name = "btnExecl";
            btnExecl.Size = new Size(94, 40);
            btnExecl.TabIndex = 0;
            btnExecl.Text = "Xuất Execl";
            btnExecl.UseVisualStyleBackColor = false;
            btnExecl.Click += btnExecl_Click;
            // 
            // panelTinhTong
            // 
            panelTinhTong.BackColor = Color.FromArgb(39, 185, 154);
            panelTinhTong.Location = new Point(915, 13);
            panelTinhTong.Name = "panelTinhTong";
            panelTinhTong.Size = new Size(341, 127);
            panelTinhTong.TabIndex = 8;
            panelTinhTong.Paint += panelTinhTong_Paint;
            // 
            // panelTong
            // 
            panelTong.BackColor = Color.Crimson;
            panelTong.Location = new Point(762, 13);
            panelTong.Name = "panelTong";
            panelTong.Size = new Size(157, 127);
            panelTong.TabIndex = 7;
            panelTong.Paint += panelTong_Paint;
            // 
            // btnIn
            // 
            btnIn.BackColor = Color.DarkCyan;
            btnIn.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            btnIn.ForeColor = SystemColors.ButtonHighlight;
            btnIn.Location = new Point(514, 97);
            btnIn.Name = "btnIn";
            btnIn.Size = new Size(94, 40);
            btnIn.TabIndex = 6;
            btnIn.Text = "In Lại";
            btnIn.UseVisualStyleBackColor = false;
            btnIn.Click += btnIn_Click;
            // 
            // cbFactory
            // 
            cbFactory.FormattingEnabled = true;
            cbFactory.Location = new Point(152, 101);
            cbFactory.Name = "cbFactory";
            cbFactory.Size = new Size(268, 31);
            cbFactory.TabIndex = 5;
            cbFactory.SelectedIndexChanged += cbFactory_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            label4.Location = new Point(19, 107);
            label4.Name = "label4";
            label4.Size = new Size(127, 19);
            label4.TabIndex = 4;
            label4.Text = "Chọn Nhà Máy :";
            // 
            // dateStop
            // 
            dateStop.CustomFormat = "dd/MM/yyyy";
            dateStop.Format = DateTimePickerFormat.Custom;
            dateStop.Location = new Point(426, 45);
            dateStop.Name = "dateStop";
            dateStop.Size = new Size(182, 30);
            dateStop.TabIndex = 3;
            dateStop.ValueChanged += dateStop_ValueChanged;
            // 
            // dateStart
            // 
            dateStart.CustomFormat = "dd/MM/yyyy";
            dateStart.Format = DateTimePickerFormat.Custom;
            dateStart.Location = new Point(105, 45);
            dateStart.Name = "dateStart";
            dateStart.Size = new Size(182, 30);
            dateStart.TabIndex = 2;
            dateStart.ValueChanged += dateStart_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            label3.Location = new Point(331, 54);
            label3.Name = "label3";
            label3.Size = new Size(89, 19);
            label3.TabIndex = 1;
            label3.Text = "Đến Ngày :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            label2.Location = new Point(19, 54);
            label2.Name = "label2";
            label2.Size = new Size(80, 19);
            label2.TabIndex = 0;
            label2.Text = "Từ Ngày :";
            // 
            // TonKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 826);
            Controls.Add(groupBox1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(dataTonKho);
            Name = "TonKho";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TonKho";
            ((System.ComponentModel.ISupportInitialize)dataTonKho).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataTonKho;
        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private GroupBox groupBox1;
        private DateTimePicker dateStart;
        private Label label3;
        private Label label2;
        private Button btnClose;
        private Button btnLoc;
        private Button btnExecl;
        private Panel panelTinhTong;
        private Panel panelTong;
        private Button btnIn;
        private ComboBox cbFactory;
        private Label label4;
        private DateTimePicker dateStop;
    }
}