namespace InventorySimulation
{
    partial class outputForm
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
            this.output_grid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.output_grid)).BeginInit();
            this.SuspendLayout();
            // 
            // output_grid
            // 
            this.output_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.output_grid.Location = new System.Drawing.Point(-211, -245);
            this.output_grid.Margin = new System.Windows.Forms.Padding(4);
            this.output_grid.Name = "output_grid";
            this.output_grid.RowTemplate.Height = 26;
            this.output_grid.Size = new System.Drawing.Size(1625, 941);
            this.output_grid.TabIndex = 1;
            // 
            // outputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1430, 707);
            this.Controls.Add(this.output_grid);
            this.Name = "outputForm";
            this.Text = "outputForm";
            this.Load += new System.EventHandler(this.outputForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.output_grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView output_grid;
    }
}