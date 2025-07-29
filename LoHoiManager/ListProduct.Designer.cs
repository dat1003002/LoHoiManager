namespace LoHoiManager
{
    partial class ListProduct
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
            textsearch = new TextBox();
            btnclose = new Button();
            btndelete = new Button();
            btnedit = new Button();
            btnadd = new Button();
            label3 = new Label();
            textname = new TextBox();
            label2 = new Label();
            dataProductype = new DataGridView();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataProductype).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Crimson;
            panel1.Controls.Add(label1);
            panel1.Location = new Point(-2, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1282, 91);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(438, 21);
            label1.Name = "label1";
            label1.Size = new Size(411, 42);
            label1.TabIndex = 0;
            label1.Text = "Danh Sách Các Loại Củi";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textsearch);
            groupBox1.Controls.Add(btnclose);
            groupBox1.Controls.Add(btndelete);
            groupBox1.Controls.Add(btnedit);
            groupBox1.Controls.Add(btnadd);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(textname);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = SystemColors.ActiveCaptionText;
            groupBox1.Location = new Point(12, 111);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1256, 127);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông Tin Sản Phẩm";
            // 
            // textsearch
            // 
            textsearch.Location = new Point(932, 51);
            textsearch.Name = "textsearch";
            textsearch.Size = new Size(297, 34);
            textsearch.TabIndex = 8;
            textsearch.TextChanged += textsearch_TextChanged;
            // 
            // btnclose
            // 
            btnclose.BackColor = Color.DodgerBlue;
            btnclose.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnclose.ForeColor = SystemColors.ButtonHighlight;
            btnclose.Location = new Point(689, 66);
            btnclose.Name = "btnclose";
            btnclose.Size = new Size(104, 38);
            btnclose.TabIndex = 7;
            btnclose.Text = "Đóng";
            btnclose.UseVisualStyleBackColor = false;
            btnclose.Click += btnclose_Click;
            // 
            // btndelete
            // 
            btndelete.BackColor = Color.Crimson;
            btndelete.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btndelete.ForeColor = SystemColors.ButtonHighlight;
            btndelete.Location = new Point(689, 22);
            btndelete.Name = "btndelete";
            btndelete.Size = new Size(104, 38);
            btndelete.TabIndex = 6;
            btndelete.Text = "Xóa";
            btndelete.UseVisualStyleBackColor = false;
            btndelete.Click += btndelete_Click;
            // 
            // btnedit
            // 
            btnedit.BackColor = Color.FromArgb(39, 185, 154);
            btnedit.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnedit.ForeColor = SystemColors.ButtonHighlight;
            btnedit.Location = new Point(550, 66);
            btnedit.Name = "btnedit";
            btnedit.Size = new Size(122, 38);
            btnedit.TabIndex = 5;
            btnedit.Text = "Chỉnh Sửa";
            btnedit.UseVisualStyleBackColor = false;
            btnedit.Click += btnedit_Click;
            // 
            // btnadd
            // 
            btnadd.BackColor = Color.FromArgb(39, 185, 154);
            btnadd.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnadd.ForeColor = SystemColors.ButtonHighlight;
            btnadd.Location = new Point(550, 22);
            btnadd.Name = "btnadd";
            btnadd.Size = new Size(122, 38);
            btnadd.TabIndex = 4;
            btnadd.Text = "Thêm";
            btnadd.UseVisualStyleBackColor = false;
            btnadd.Click += btnadd_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(813, 54);
            label3.Name = "label3";
            label3.Size = new Size(113, 28);
            label3.TabIndex = 3;
            label3.Text = "Tìm Kiếm :";
            // 
            // textname
            // 
            textname.Location = new Point(186, 48);
            textname.Name = "textname";
            textname.Size = new Size(342, 34);
            textname.TabIndex = 1;
            textname.TextChanged += textname_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(31, 48);
            label2.Name = "label2";
            label2.Size = new Size(149, 28);
            label2.TabIndex = 0;
            label2.Text = "Nhập Tên Củi :";
            // 
            // dataProductype
            // 
            dataProductype.BackgroundColor = SystemColors.ActiveCaption;
            dataProductype.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataProductype.Location = new Point(12, 268);
            dataProductype.Name = "dataProductype";
            dataProductype.RowHeadersWidth = 51;
            dataProductype.Size = new Size(1256, 546);
            dataProductype.TabIndex = 2;
            dataProductype.CellContentClick += dataProductype_CellContentClick;
            // 
            // ListProduct
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 826);
            Controls.Add(dataProductype);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "ListProduct";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ListProduct";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataProductype).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private GroupBox groupBox1;
        private DataGridView dataProductype;
        private Label label2;
        private Label label3;
        private TextBox textname;
        private Button btnedit;
        private Button btnadd;
        private TextBox textsearch;
        private Button btnclose;
        private Button btndelete;
    }
}