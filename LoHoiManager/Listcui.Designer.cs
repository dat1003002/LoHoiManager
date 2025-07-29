namespace LoHoiManager
{
    partial class Listcui
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
            groupBox1 = new GroupBox();
            btnExport = new Button();
            btnTonKho = new Button();
            cbtimkiem = new ComboBox();
            btnClose = new Button();
            btnsearch = new Button();
            btnExecl = new Button();
            label3 = new Label();
            datestop = new DateTimePicker();
            label4 = new Label();
            datestart = new DateTimePicker();
            panelTong = new Panel();
            panel2 = new Panel();
            label2 = new Label();
            dataproduct = new DataGridView();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataproduct).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Crimson;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1280, 79);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(338, 19);
            label1.Name = "label1";
            label1.Size = new Size(537, 42);
            label1.TabIndex = 0;
            label1.Text = "Danh Sách Sản Phẩm Nhập Kho";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnExport);
            groupBox1.Controls.Add(btnTonKho);
            groupBox1.Controls.Add(cbtimkiem);
            groupBox1.Controls.Add(btnClose);
            groupBox1.Controls.Add(btnsearch);
            groupBox1.Controls.Add(btnExecl);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(datestop);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(datestart);
            groupBox1.Controls.Add(panelTong);
            groupBox1.Controls.Add(panel2);
            groupBox1.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = SystemColors.ActiveCaptionText;
            groupBox1.Location = new Point(12, 112);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1256, 152);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tổng sản phẩm";
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.ForestGreen;
            btnExport.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            btnExport.ForeColor = SystemColors.ButtonHighlight;
            btnExport.Location = new Point(369, 105);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(98, 36);
            btnExport.TabIndex = 12;
            btnExport.Text = "Xuất Kho";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnTonKho
            // 
            btnTonKho.BackColor = Color.DarkCyan;
            btnTonKho.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold);
            btnTonKho.ForeColor = SystemColors.ButtonHighlight;
            btnTonKho.Location = new Point(262, 105);
            btnTonKho.Name = "btnTonKho";
            btnTonKho.Size = new Size(101, 36);
            btnTonKho.TabIndex = 11;
            btnTonKho.Text = "Tồn Kho";
            btnTonKho.UseVisualStyleBackColor = false;
            btnTonKho.Click += btnTonKho_Click;
            // 
            // cbtimkiem
            // 
            cbtimkiem.FormattingEnabled = true;
            cbtimkiem.Location = new Point(16, 108);
            cbtimkiem.Name = "cbtimkiem";
            cbtimkiem.Size = new Size(205, 33);
            cbtimkiem.TabIndex = 10;
            cbtimkiem.SelectedIndexChanged += cbtimkiem_SelectedIndexChanged;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Crimson;
            btnClose.Font = new Font("Times New Roman", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = SystemColors.ButtonHighlight;
            btnClose.Location = new Point(539, 108);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(102, 38);
            btnClose.TabIndex = 9;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnsearch
            // 
            btnsearch.BackColor = SystemColors.MenuHighlight;
            btnsearch.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnsearch.ForeColor = SystemColors.ButtonHighlight;
            btnsearch.Location = new Point(539, 68);
            btnsearch.Name = "btnsearch";
            btnsearch.Size = new Size(102, 34);
            btnsearch.TabIndex = 8;
            btnsearch.Text = "Lọc";
            btnsearch.UseVisualStyleBackColor = false;
            btnsearch.Click += btnsearch_Click;
            // 
            // btnExecl
            // 
            btnExecl.BackColor = Color.FromArgb(39, 185, 154);
            btnExecl.Font = new Font("Times New Roman", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExecl.ForeColor = SystemColors.ButtonHighlight;
            btnExecl.Location = new Point(539, 26);
            btnExecl.Name = "btnExecl";
            btnExecl.Size = new Size(102, 36);
            btnExecl.TabIndex = 7;
            btnExecl.Text = "Xuất Execl";
            btnExecl.UseVisualStyleBackColor = false;
            btnExecl.Click += btnExecl_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(311, 30);
            label3.Name = "label3";
            label3.Size = new Size(125, 25);
            label3.TabIndex = 5;
            label3.Text = "Đến Ngày :";
            // 
            // datestop
            // 
            datestop.CustomFormat = "dd/MM/yyyy";
            datestop.Format = DateTimePickerFormat.Custom;
            datestop.Location = new Point(262, 58);
            datestop.Name = "datestop";
            datestop.Size = new Size(205, 34);
            datestop.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(55, 30);
            label4.Name = "label4";
            label4.Size = new Size(113, 25);
            label4.TabIndex = 3;
            label4.Text = "Từ Ngày :";
            // 
            // datestart
            // 
            datestart.CustomFormat = "dd/MM/yyyy";
            datestart.Format = DateTimePickerFormat.Custom;
            datestart.Location = new Point(16, 58);
            datestart.Name = "datestart";
            datestart.Size = new Size(205, 34);
            datestart.TabIndex = 2;
            // 
            // panelTong
            // 
            panelTong.BackColor = Color.Crimson;
            panelTong.Location = new Point(831, 14);
            panelTong.Name = "panelTong";
            panelTong.Size = new Size(425, 138);
            panelTong.TabIndex = 1;
            panelTong.Paint += panelTong_Paint;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(39, 185, 154);
            panel2.Controls.Add(label2);
            panel2.Location = new Point(659, 14);
            panel2.Name = "panel2";
            panel2.Size = new Size(176, 138);
            panel2.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(35, 49);
            label2.Name = "label2";
            label2.Size = new Size(107, 45);
            label2.TabIndex = 0;
            label2.Text = "Tổng";
            // 
            // dataproduct
            // 
            dataproduct.BackgroundColor = SystemColors.ActiveCaption;
            dataproduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataproduct.Location = new Point(0, 344);
            dataproduct.Name = "dataproduct";
            dataproduct.RowHeadersWidth = 51;
            dataproduct.Size = new Size(1280, 481);
            dataproduct.TabIndex = 2;
            dataproduct.CellContentClick += dataproduct_CellContentClick;
            // 
            // Listcui
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 826);
            Controls.Add(dataproduct);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            ForeColor = SystemColors.ActiveCaptionText;
            Name = "Listcui";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Listcui";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataproduct).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private GroupBox groupBox1;
        private DataGridView dataproduct;
        private Panel panel2;
        private Label label2;
        private Panel panelTong;
        private Label label3;
        private DateTimePicker datestart;
        private Label label4;
        private DateTimePicker datestop;
        private Button btnExecl;
        private Button btnsearch;
        private Button btnClose;
        private ComboBox cbtimkiem;
        private Button btnExport;
        private Button btnTonKho;
    }
}