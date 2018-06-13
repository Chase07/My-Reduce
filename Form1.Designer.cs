namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.gDataView = new System.Windows.Forms.DataGridView();
            this.Out = new System.Windows.Forms.RichTextBox();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.Open = new System.Windows.Forms.Button();
            this.Normalize = new System.Windows.Forms.Button();
            this.NRReduce = new System.Windows.Forms.Button();
            this.MRMRReduce = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gDataView)).BeginInit();
            this.SuspendLayout();
            // 
            // gDataView
            // 
            this.gDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gDataView.Location = new System.Drawing.Point(12, 12);
            this.gDataView.Name = "gDataView";
            this.gDataView.RowTemplate.Height = 24;
            this.gDataView.Size = new System.Drawing.Size(1344, 231);
            this.gDataView.TabIndex = 0;
            this.gDataView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Out
            // 
            this.Out.Location = new System.Drawing.Point(12, 262);
            this.Out.Name = "Out";
            this.Out.Size = new System.Drawing.Size(1344, 215);
            this.Out.TabIndex = 1;
            this.Out.Text = "";
            this.Out.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = "Data Source=CHASE07\\CHASE07;Initial Catalog=UCIdata;Integrated Security=True";
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            this.sqlConnection1.StatisticsEnabled = true;
            this.sqlConnection1.InfoMessage += new System.Data.SqlClient.SqlInfoMessageEventHandler(this.sqlConnection1_InfoMessage);
            // 
            // Open
            // 
            this.Open.Location = new System.Drawing.Point(35, 491);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(112, 23);
            this.Open.TabIndex = 2;
            this.Open.Text = "Open";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.button1_Click);
            // 
            // Normalize
            // 
            this.Normalize.Location = new System.Drawing.Point(179, 491);
            this.Normalize.Name = "Normalize";
            this.Normalize.Size = new System.Drawing.Size(119, 23);
            this.Normalize.TabIndex = 3;
            this.Normalize.Text = "Normalize";
            this.Normalize.UseVisualStyleBackColor = true;
            this.Normalize.Click += new System.EventHandler(this.button2_Click);
            // 
            // NRReduce
            // 
            this.NRReduce.Location = new System.Drawing.Point(382, 491);
            this.NRReduce.Name = "NRReduce";
            this.NRReduce.Size = new System.Drawing.Size(193, 23);
            this.NRReduce.TabIndex = 4;
            this.NRReduce.Text = "Neighbor Reduce";
            this.NRReduce.UseVisualStyleBackColor = true;
            this.NRReduce.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // MRMRReduce
            // 
            this.MRMRReduce.Location = new System.Drawing.Point(640, 491);
            this.MRMRReduce.Name = "MRMRReduce";
            this.MRMRReduce.Size = new System.Drawing.Size(165, 23);
            this.MRMRReduce.TabIndex = 5;
            this.MRMRReduce.Text = "MRMR Reduce";
            this.MRMRReduce.UseVisualStyleBackColor = true;
            this.MRMRReduce.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 636);
            this.Controls.Add(this.MRMRReduce);
            this.Controls.Add(this.NRReduce);
            this.Controls.Add(this.Normalize);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.Out);
            this.Controls.Add(this.gDataView);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gDataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gDataView;
        private System.Windows.Forms.RichTextBox Out;
        private System.Data.SqlClient.SqlConnection sqlConnection1;
        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Button Normalize;
        private System.Windows.Forms.Button NRReduce;
        private System.Windows.Forms.Button MRMRReduce;
    }
}

