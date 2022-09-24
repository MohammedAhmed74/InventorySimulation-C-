using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InventorySimulation
{
    public partial class outputForm : Form
    {
        public outputForm()
        {
            InitializeComponent();
        }

        private void outputForm_Load(object sender, EventArgs e)
        {
            Random r = new Random();

            output_grid.ColumnCount = 12;
            output_grid.Columns[0].Name = "Day";
            output_grid.Columns[1].Name = "Cycle";
            output_grid.Columns[2].Name = "Day within Cycle";
            output_grid.Columns[3].Name = "Beginning inventory";
            output_grid.Columns[4].Name = "Random Digit for demand";
            output_grid.Columns[5].Name = "Demand";


            output_grid.Columns[6].Name = "Ending inventory";
            output_grid.Columns[7].Name = "Shortage quantity";
            output_grid.Columns[8].Name = "Order quantity";


            output_grid.Columns[9].Name = "Random Digit for lead time";
            output_grid.Columns[10].Name = "Lead Time";
            output_grid.Columns[11].Name = "Days until order arrives";


            for (int i = 0; i < 100; i++)
            {

                //random servie


               
                output_grid.Rows.Add();
                
                //output_grid[0, i].Value = syscasee.CustomerNumber.ToString();
                //syscasee.RandomService = r.Next(1, 100);
                //output_grid[4, i].Value = syscasee.RandomService.ToString();
            }
        }
    }
}
