namespace LoHoiManager
{
    partial class XuatKho
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
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            groupBox1 = new GroupBox();
            panelTinhTong = new Panel();
            paneTong = new Panel();
            btnClose = new Button();
            btnLoc = new Button();
            btnExecl = new Button();
            cbFactory = new ComboBox();
            label4 = new Label();
            dateStop = new DateTimePicker();
            dateStart = new DateTimePicker();
            label3 = new Label();
            label2 = new Label();
            dataExport = new DataGridView();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataExport).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Crimson;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1280, 90);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 24F, FontStyle.Bold);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(455, 21);
            label1.Name = "label1";
            label1.Size = new Size(376, 45);
            label1.TabIndex = 0;
            label1.Text = "Danh Sách Xuất Kho";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Crimson;
            panel2.Location = new Point(0, 96);
            panel2.Name = "panel2";
            panel2.Size = new Size(1280, 12);
            panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(panelTinhTong);
            groupBox1.Controls.Add(paneTong);
            groupBox1.Controls.Add(btnClose);
            groupBox1.Controls.Add(btnLoc);
            groupBox1.Controls.Add(btnExecl);
            groupBox1.Controls.Add(cbFactory);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(dateStop);
            groupBox1.Controls.Add(dateStart);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            groupBox1.Location = new Point(12, 124);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1256, 142);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chức Năng Xuất Kho";
            // 
            // panelTinhTong
            // 
            panelTinhTong.BackColor = Color.FromArgb(39, 185, 154);
            panelTinhTong.Location = new Point(901, 15);
            panelTinhTong.Name = "panelTinhTong";
            panelTinhTong.Size = new Size(353, 127);
            panelTinhTong.TabIndex = 10;
            panelTinhTong.Paint += panelTinhTong_Paint;
            // 
            // paneTong
            // 
            paneTong.BackColor = Color.Crimson;
            paneTong.Location = new Point(771, 15);
            paneTong.Name = "paneTong";
            paneTong.Size = new Size(130, 127);
            paneTong.TabIndex = 9;
            paneTong.Paint += paneTong_Paint;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Crimson;
            btnClose.Font = new Font("Times New Roman", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = SystemColors.ButtonHighlight;
            btnClose.Location = new Point(645, 99);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 36);
            btnClose.TabIndex = 8;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.DodgerBlue;
            btnLoc.Font = new Font("Times New Roman", 9F, FontStyle.Bold);
            btnLoc.ForeColor = SystemColors.ButtonHighlight;
            btnLoc.Location = new Point(645, 15);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(94, 36);
            btnLoc.TabIndex = 7;
            btnLoc.Text = "Lọc";
            btnLoc.UseVisualStyleBackColor = false;
            btnLoc.Click += btnLoc_Click;
            // 
            // btnExecl
            // 
            btnExecl.BackColor = Color.FromArgb(39, 185, 154);
            btnExecl.Font = new Font("Times New Roman", 9F, FontStyle.Bold);
            btnExecl.ForeColor = SystemColors.ButtonHighlight;
            btnExecl.Location = new Point(645, 57);
            btnExecl.Name = "btnExecl";
            btnExecl.Size = new Size(94, 36);
            btnExecl.TabIndex = 6;
            btnExecl.Text = "Xuất Execl";
            btnExecl.UseVisualStyleBackColor = false;
            btnExecl.Click += btnExecl_Click;
            // 
            // cbFactory
            // 
            cbFactory.FormattingEnabled = true;
            cbFactory.Location = new Point(152, 82);
            cbFactory.Name = "cbFactory";
            cbFactory.Size = new Size(279, 31);
            cbFactory.TabIndex = 5;
            cbFactory.SelectedIndexChanged += cbFactory_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            label4.Location = new Point(25, 88);
            label4.Name = "label4";
            label4.Size = new Size(127, 19);
            label4.TabIndex = 4;
            label4.Text = "Chọn Nhà Máy :";
            // 
            // dateStop
            // 
            dateStop.CustomFormat = "dd/MM/yyyy";
            dateStop.Format = DateTimePickerFormat.Custom;
            dateStop.Location = new Point(410, 37);
            dateStop.Name = "dateStop";
            dateStop.Size = new Size(172, 30);
            dateStop.TabIndex = 3;
            // 
            // dateStart
            // 
            dateStart.CustomFormat = "dd/MM/yyyy";
            dateStart.Format = DateTimePickerFormat.Custom;
            dateStart.Location = new Point(111, 37);
            dateStart.Name = "dateStart";
            dateStart.Size = new Size(173, 30);
            dateStart.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            label3.Location = new Point(315, 44);
            label3.Name = "label3";
            label3.Size = new Size(89, 19);
            label3.TabIndex = 1;
            label3.Text = "Đến Ngày :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            label2.Location = new Point(25, 46);
            label2.Name = "label2";
            label2.Size = new Size(80, 19);
            label2.TabIndex = 0;
            label2.Text = "Từ Ngày :";
            // 
            // dataExport
            // 
            dataExport.BackgroundColor = SystemColors.ActiveCaption;
            dataExport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataExport.Location = new Point(0, 315);
            dataExport.Name = "dataExport";
            dataExport.RowHeadersWidth = 51;
            dataExport.Size = new Size(1280, 499);
            dataExport.TabIndex = 3;
            dataExport.CellContentClick += dataExport_CellContentClick;
            // 
            // XuatKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 826);
            Controls.Add(dataExport);
            Controls.Add(groupBox1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "XuatKho";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "XuatKho";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataExport).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private GroupBox groupBox1;
        private DataGridView dataExport;
        private Label label3;
        private Label label2;
        private ComboBox cbFactory;
        private Label label4;
        private DateTimePicker dateStop;
        private DateTimePicker dateStart;
        private Button btnClose;
        private Button btnLoc;
        private Button btnExecl;
        private Panel panelTinhTong;
        private Panel paneTong;
    }
}