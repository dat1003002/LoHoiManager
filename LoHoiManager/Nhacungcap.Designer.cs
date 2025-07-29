namespace LoHoiManager
{
    partial class Nhacungcap
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
            header = new Panel();
            lbnhacc = new Label();
            groupheader = new GroupBox();
            label3 = new Label();
            bntedit = new Button();
            btndelete = new Button();
            btnclose = new Button();
            textsearch = new TextBox();
            btnadd = new Button();
            label2 = new Label();
            label1 = new Label();
            txtmanhacc = new TextBox();
            texttennhacc = new TextBox();
            datanhacc = new DataGridView();
            header.SuspendLayout();
            groupheader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)datanhacc).BeginInit();
            SuspendLayout();
            // 
            // header
            // 
            header.BackColor = Color.Crimson;
            header.Controls.Add(lbnhacc);
            header.Dock = DockStyle.Top;
            header.Location = new Point(0, 0);
            header.Name = "header";
            header.Size = new Size(1280, 82);
            header.TabIndex = 0;
            // 
            // lbnhacc
            // 
            lbnhacc.AutoSize = true;
            lbnhacc.Font = new Font("Times New Roman", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbnhacc.ForeColor = SystemColors.ButtonFace;
            lbnhacc.Location = new Point(394, 22);
            lbnhacc.Name = "lbnhacc";
            lbnhacc.Size = new Size(538, 42);
            lbnhacc.TabIndex = 0;
            lbnhacc.Text = "DANH SÁCH NHÀ CUNG CẤP";
            lbnhacc.TextAlign = ContentAlignment.TopCenter;
            // 
            // groupheader
            // 
            groupheader.Controls.Add(label3);
            groupheader.Controls.Add(bntedit);
            groupheader.Controls.Add(btndelete);
            groupheader.Controls.Add(btnclose);
            groupheader.Controls.Add(textsearch);
            groupheader.Controls.Add(btnadd);
            groupheader.Controls.Add(label2);
            groupheader.Controls.Add(label1);
            groupheader.Controls.Add(txtmanhacc);
            groupheader.Controls.Add(texttennhacc);
            groupheader.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupheader.Location = new Point(12, 102);
            groupheader.Name = "groupheader";
            groupheader.Size = new Size(1256, 169);
            groupheader.TabIndex = 1;
            groupheader.TabStop = false;
            groupheader.Text = "Thông Tin Nhà Cung Cấp";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(28, 109);
            label3.Name = "label3";
            label3.Size = new Size(237, 25);
            label3.TabIndex = 9;
            label3.Text = "Nhập Thông Tin Tìm Kiếm :";
            // 
            // bntedit
            // 
            bntedit.BackColor = Color.FromArgb(39, 185, 154);
            bntedit.ForeColor = SystemColors.ButtonHighlight;
            bntedit.Location = new Point(788, 102);
            bntedit.Name = "bntedit";
            bntedit.Size = new Size(118, 38);
            bntedit.TabIndex = 8;
            bntedit.Text = "Chỉnh Sửa";
            bntedit.UseVisualStyleBackColor = false;
            bntedit.Click += bntedit_Click;
            // 
            // btndelete
            // 
            btndelete.BackColor = Color.Crimson;
            btndelete.ForeColor = SystemColors.ButtonHighlight;
            btndelete.Location = new Point(939, 102);
            btndelete.Name = "btndelete";
            btndelete.Size = new Size(113, 38);
            btndelete.TabIndex = 7;
            btndelete.Text = "Xóa";
            btndelete.UseVisualStyleBackColor = false;
            btndelete.Click += btndelete_Click;
            // 
            // btnclose
            // 
            btnclose.BackColor = SystemColors.HotTrack;
            btnclose.ForeColor = Color.White;
            btnclose.Location = new Point(1095, 102);
            btnclose.Name = "btnclose";
            btnclose.Size = new Size(118, 38);
            btnclose.TabIndex = 6;
            btnclose.Text = "Đóng";
            btnclose.UseVisualStyleBackColor = false;
            btnclose.Click += btnclose_Click;
            // 
            // textsearch
            // 
            textsearch.Location = new Point(271, 106);
            textsearch.Name = "textsearch";
            textsearch.Size = new Size(469, 31);
            textsearch.TabIndex = 5;
            textsearch.TextChanged += textsearch_TextChanged;
            // 
            // btnadd
            // 
            btnadd.BackColor = Color.FromArgb(39, 185, 154);
            btnadd.ForeColor = SystemColors.ButtonHighlight;
            btnadd.Location = new Point(1095, 30);
            btnadd.Name = "btnadd";
            btnadd.Size = new Size(118, 37);
            btnadd.TabIndex = 4;
            btnadd.Text = "Thêm Mới";
            btnadd.UseVisualStyleBackColor = false;
            btnadd.Click += btnadd_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(558, 33);
            label2.Name = "label2";
            label2.Size = new Size(105, 25);
            label2.TabIndex = 3;
            label2.Text = "Tên Nhà CC";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 33);
            label1.Name = "label1";
            label1.Size = new Size(102, 25);
            label1.TabIndex = 2;
            label1.Text = "Mã Nhà CC";
            // 
            // txtmanhacc
            // 
            txtmanhacc.Location = new Point(136, 30);
            txtmanhacc.Name = "txtmanhacc";
            txtmanhacc.Size = new Size(373, 31);
            txtmanhacc.TabIndex = 1;
            // 
            // texttennhacc
            // 
            texttennhacc.Location = new Point(669, 33);
            texttennhacc.Multiline = true;
            texttennhacc.Name = "texttennhacc";
            texttennhacc.Size = new Size(373, 31);
            texttennhacc.TabIndex = 0;
            // 
            // datanhacc
            // 
            datanhacc.BackgroundColor = SystemColors.ActiveCaption;
            datanhacc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            datanhacc.Location = new Point(12, 326);
            datanhacc.Name = "datanhacc";
            datanhacc.RowHeadersWidth = 51;
            datanhacc.Size = new Size(1256, 499);
            datanhacc.TabIndex = 2;
            datanhacc.CellContentClick += datanhacc_CellContentClick;
            // 
            // Nhacungcap
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 826);
            Controls.Add(datanhacc);
            Controls.Add(header);
            Controls.Add(groupheader);
            Name = "Nhacungcap";
            Text = "Nhacungcap";
            header.ResumeLayout(false);
            header.PerformLayout();
            groupheader.ResumeLayout(false);
            groupheader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)datanhacc).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel header;
        private Label lbnhacc;
        private DataGridView datanhacungcap;
        private GroupBox groupheader;
        private Button bntedit;
        private Button btndelete;
        private Button btnclose;
        private TextBox textsearch;
        private Button btnadd;
        private Label label2;
        private Label label1;
        private TextBox txtmanhacc;
        private TextBox texttennhacc;
        private DataGridView dataGridView1;
        private DataGridView datanhacc;
        private Label label3;
    }
}