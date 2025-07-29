namespace LoHoiManager
{
    partial class ListPallet
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
            btnclose = new Button();
            btndelete = new Button();
            inputsearch = new TextBox();
            label4 = new Label();
            btnupdate = new Button();
            btnadd = new Button();
            trongluongpallet = new TextBox();
            label3 = new Label();
            label2 = new Label();
            namePallet = new TextBox();
            groupBox2 = new GroupBox();
            dataPallet = new DataGridView();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataPallet).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Crimson;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1280, 85);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(501, 20);
            label1.Name = "label1";
            label1.Size = new Size(291, 42);
            label1.TabIndex = 0;
            label1.Text = "Danh Sách Pallet";
            // 
            // btnclose
            // 
            btnclose.BackColor = SystemColors.HotTrack;
            btnclose.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnclose.ForeColor = SystemColors.ButtonHighlight;
            btnclose.Location = new Point(814, 83);
            btnclose.Name = "btnclose";
            btnclose.Size = new Size(129, 41);
            btnclose.TabIndex = 9;
            btnclose.Text = "Đóng";
            btnclose.UseVisualStyleBackColor = false;
            btnclose.Click += btnclose_Click;
            // 
            // btndelete
            // 
            btndelete.BackColor = Color.Crimson;
            btndelete.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btndelete.ForeColor = SystemColors.ButtonHighlight;
            btndelete.Location = new Point(673, 87);
            btndelete.Name = "btndelete";
            btndelete.Size = new Size(107, 39);
            btndelete.TabIndex = 8;
            btndelete.Text = "Xóa";
            btndelete.UseVisualStyleBackColor = false;
            btndelete.Click += btndelete_Click;
            // 
            // inputsearch
            // 
            inputsearch.Location = new Point(814, 33);
            inputsearch.Name = "inputsearch";
            inputsearch.Size = new Size(373, 34);
            inputsearch.TabIndex = 7;
            inputsearch.TextChanged += inputsearch_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(673, 41);
            label4.Name = "label4";
            label4.Size = new Size(125, 25);
            label4.TabIndex = 6;
            label4.Text = "Tìm Kiếm :";
            // 
            // btnupdate
            // 
            btnupdate.BackColor = Color.FromArgb(39, 185, 154);
            btnupdate.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnupdate.ForeColor = SystemColors.ButtonHighlight;
            btnupdate.Location = new Point(508, 86);
            btnupdate.Name = "btnupdate";
            btnupdate.Size = new Size(129, 40);
            btnupdate.TabIndex = 5;
            btnupdate.Text = "Chỉnh Sửa";
            btnupdate.UseVisualStyleBackColor = false;
            btnupdate.Click += btnupdate_Click;
            // 
            // btnadd
            // 
            btnadd.BackColor = Color.FromArgb(39, 185, 154);
            btnadd.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnadd.ForeColor = SystemColors.ButtonHighlight;
            btnadd.Location = new Point(508, 38);
            btnadd.Name = "btnadd";
            btnadd.Size = new Size(129, 38);
            btnadd.TabIndex = 4;
            btnadd.Text = "Thêm Mới";
            btnadd.UseVisualStyleBackColor = false;
            btnadd.Click += btnadd_Click;
            // 
            // trongluongpallet
            // 
            trongluongpallet.Location = new Point(192, 86);
            trongluongpallet.Name = "trongluongpallet";
            trongluongpallet.Size = new Size(284, 34);
            trongluongpallet.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 95);
            label3.Name = "label3";
            label3.Size = new Size(145, 25);
            label3.TabIndex = 2;
            label3.Text = "Trọng Lượng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 41);
            label2.Name = "label2";
            label2.Size = new Size(126, 25);
            label2.TabIndex = 1;
            label2.Text = "Tên Pallet :";
            // 
            // namePallet
            // 
            namePallet.Location = new Point(192, 38);
            namePallet.Name = "namePallet";
            namePallet.Size = new Size(284, 34);
            namePallet.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnclose);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(inputsearch);
            groupBox2.Controls.Add(btndelete);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(namePallet);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(trongluongpallet);
            groupBox2.Controls.Add(btnupdate);
            groupBox2.Controls.Add(btnadd);
            groupBox2.Font = new Font("Times New Roman", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(12, 110);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1256, 146);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Danh Sách Chức Năng  Pallet";
            // 
            // dataPallet
            // 
            dataPallet.BackgroundColor = SystemColors.ActiveCaption;
            dataPallet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataPallet.Location = new Point(12, 310);
            dataPallet.Name = "dataPallet";
            dataPallet.RowHeadersWidth = 51;
            dataPallet.Size = new Size(1256, 504);
            dataPallet.TabIndex = 3;
            // 
            // ListPallet
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 826);
            Controls.Add(dataPallet);
            Controls.Add(groupBox2);
            Controls.Add(panel1);
            Name = "ListPallet";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ListPallet";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataPallet).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Label label2;
        private TextBox namePallet;
        private Button btnupdate;
        private Button btnadd;
        private TextBox trongluongpallet;
        private Label label3;
        private TextBox inputsearch;
        private Label label4;
        private Button btnclose;
        private Button btndelete;
        private GroupBox groupBox2;
        private DataGridView dataPallet;
    }
}