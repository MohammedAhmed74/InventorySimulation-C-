using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryModels;
using InventoryTesting;

namespace InventorySimulation
{
    public partial class Form1 : Form
    {
        public string[] lines = File.ReadAllLines(@"TestCase1.txt");
        SimulationSystem s = new SimulationSystem();
        DataTable table = new DataTable();


        public Form1()
        {
            InitializeComponent();
            int tmp = 1;
            int tmp2 = 1;
            int leadCount = 2;
            bool order = false;
            int orderQ = 0;
            decimal sAvg = 0;
            decimal eAvg = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "OrderUpTo")
                {

                    s.OrderUpTo = int.Parse(lines[i + 1]);
                }
                else if (lines[i] == "ReviewPeriod")
                {
                    s.ReviewPeriod = int.Parse(lines[i + 1]);
                }
                else if (lines[i] == "StartInventoryQuantity")
                {

                    s.StartInventoryQuantity = int.Parse(lines[i + 1]);

                }
                else if (lines[i] == "StartLeadDays")
                {
                    s.StartLeadDays = int.Parse(lines[i + 1]);

                }
                else if (lines[i] == "StartOrderQuantity")
                {
                    s.StartOrderQuantity = int.Parse(lines[i + 1]);
                    orderQ = s.StartOrderQuantity;
                }
                else if (lines[i] == "NumberOfDays")
                {
                    s.NumberOfDays = int.Parse(lines[i + 1]);
                }
                else if (lines[i] == "DemandDistribution")
                {

                    for (int j = 0; j < 5; j++)
                    {
                        string[] demandDist = lines[i + j + 1].Split(',', (char)StringSplitOptions.RemoveEmptyEntries);
                        Distribution d = new Distribution();
                        d.Value = int.Parse(demandDist[0]);
                        d.Probability = decimal.Parse(demandDist[1]);
                        s.DemandDistribution.Add(d);


                    }
                    for (int j = 0; j < s.DemandDistribution.Count; j++)
                    {
                        if (j == 0)
                        {
                            s.DemandDistribution[j].CummProbability = s.DemandDistribution[j].Probability;
                            s.DemandDistribution[j].MinRange = 1;

                        }
                        else
                        {

                            s.DemandDistribution[j].CummProbability =
                              s.DemandDistribution[j - 1].CummProbability + s.DemandDistribution[j].Probability;
                            s.DemandDistribution[j].MinRange = s.DemandDistribution[j].MaxRange + 1;


                        }
                        decimal v = (s.DemandDistribution[j].CummProbability) * 100;

                        s.DemandDistribution[j].MaxRange = (int)v;

                    }
                    foreach (Distribution obj in s.DemandDistribution)
                    {

                        //MessageBox.Show(obj.Value+" "+ obj.Probability);


                    }

                }
                else if (lines[i] == "LeadDaysDistribution")
                {

                    for (int j = 0; j < 3; j++)
                    {
                        string[] leadDist = lines[i + j + 1].Split(',', (char)StringSplitOptions.RemoveEmptyEntries);
                        Distribution d = new Distribution();
                        d.Value = int.Parse(leadDist[0]);
                        d.Probability = decimal.Parse(leadDist[1]);
                        s.LeadDaysDistribution.Add(d);


                    }

                    for (int k = 0; k < s.LeadDaysDistribution.Count; k++)
                    {
                        if (k == 0)
                        {
                            s.LeadDaysDistribution[k].CummProbability = s.LeadDaysDistribution[k].Probability;
                            s.LeadDaysDistribution[k].MinRange = 1;

                        }
                        else
                        {

                            s.LeadDaysDistribution[k].CummProbability =
                              s.LeadDaysDistribution[k - 1].CummProbability + s.LeadDaysDistribution[k].Probability;
                            s.LeadDaysDistribution[k].MinRange = s.LeadDaysDistribution[k].MaxRange + 1;


                        }
                        decimal v = (s.LeadDaysDistribution[k].CummProbability) * 100;

                        s.LeadDaysDistribution[k].MaxRange = (int)v;

                    }
                    foreach (Distribution obj in s.LeadDaysDistribution)
                    {

                        //MessageBox.Show(obj.Value + " " + obj.Probability);


                    }




                }


            }

            for (int i = 0; i < s.NumberOfDays; i++)
            {

                SimulationCase scase = new SimulationCase();
                scase.Cycle = tmp2;
                Random r = new Random();
                Random r2 = new Random();
                scase.RandomDemand = r.Next(1, 100);
                leadCount--;

                // day within cycle
                if (tmp <= 5)
                {
                    scase.DayWithinCycle = tmp;
                    tmp++;
                }
                else
                {
                    tmp = 1;
                    scase.DayWithinCycle = tmp;
                    tmp++;
                }
                for (int j = 0; j < s.DemandDistribution.Count; j++)
                {

                    if (scase.RandomDemand >= s.DemandDistribution[j].MinRange && scase.RandomDemand <= s.DemandDistribution[j].MaxRange)
                    {
                        scase.Demand = s.DemandDistribution[j].Value;

                        break;
                    }
                }
                //Console.WriteLine(orderQ);
                // Console.WriteLine(scase.DayWithinCycle);

                //beginning inventory
                if (i == 0)
                {
                    scase.BeginningInventory = s.StartInventoryQuantity;
                    scase.OrderQuantity = s.StartOrderQuantity;

                }
                else
                {
                    if (leadCount == -1)
                    {

                        scase.BeginningInventory = s.SimulationCases[s.SimulationCases.Count - 1].EndingInventory + orderQ;
                        order = false;

                    }
                    else
                        scase.BeginningInventory = s.SimulationCases[s.SimulationCases.Count - 1].EndingInventory;

                }

                // Shortage and ending inventory
                if (scase.BeginningInventory - scase.Demand < 0)
                {
                    scase.EndingInventory = 0;
                    if (i != 0)
                        scase.ShortageQuantity = Math.Abs(scase.BeginningInventory - scase.Demand) + s.SimulationCases[s.SimulationCases.Count - 1].ShortageQuantity;
                    else
                        scase.ShortageQuantity = Math.Abs(scase.BeginningInventory - scase.Demand);
                }
                else if (scase.BeginningInventory - scase.Demand == 0)
                {
                    if (i == 0)
                        scase.ShortageQuantity = 0;
                    else
                        scase.ShortageQuantity = s.SimulationCases[s.SimulationCases.Count - 1].ShortageQuantity;

                    scase.EndingInventory = 0;
                }
                else 
                {
                    if (i == 0)
                    {
                        scase.ShortageQuantity = 0;
                        scase.EndingInventory = scase.BeginningInventory - scase.Demand;
                    }
                    else
                    {
                        if (s.SimulationCases[s.SimulationCases.Count - 1].ShortageQuantity == 0)
                        {
                            if (i == 0)
                                scase.ShortageQuantity = 0;
                            else
                                scase.ShortageQuantity = s.SimulationCases[s.SimulationCases.Count - 1].ShortageQuantity;

                            scase.EndingInventory = scase.BeginningInventory - scase.Demand;
                        }
                        else
                        {

                            if (scase.BeginningInventory - scase.Demand > s.SimulationCases[s.SimulationCases.Count - 1].ShortageQuantity)
                            {
                                scase.EndingInventory = scase.BeginningInventory - scase.Demand - s.SimulationCases[s.SimulationCases.Count - 1].ShortageQuantity;
                                scase.ShortageQuantity = 0;
                            }
                            else if (scase.BeginningInventory - scase.Demand == s.SimulationCases[s.SimulationCases.Count - 1].ShortageQuantity)
                            {
                                scase.EndingInventory = 0;
                                scase.ShortageQuantity = 0;
                            }
                            else
                            {
                                scase.EndingInventory = 0;
                                scase.ShortageQuantity = s.SimulationCases[s.SimulationCases.Count - 1].ShortageQuantity - (scase.BeginningInventory - scase.Demand);
                            }

                        }
                    }
                }

                //lead days and order
                if (scase.DayWithinCycle % 5 == 0)
                {
                    // MessageBox.Show("I'M HERE");
                    
                    scase.RandomLeadDays = r2.Next(1, 100);
                    for (int j = 0; j < s.LeadDaysDistribution.Count; j++)
                    {
                        if (scase.RandomLeadDays >= s.LeadDaysDistribution[j].MinRange && scase.RandomLeadDays <= s.LeadDaysDistribution[j].MaxRange)
                        {
                            scase.LeadDays = s.LeadDaysDistribution[j].Value;
                            leadCount = scase.LeadDays;
                            break;
                        }


                    }
                    scase.OrderQuantity = s.OrderUpTo - scase.EndingInventory + scase.ShortageQuantity;
                    tmp2++;
                    order = true;
                    orderQ = scase.OrderQuantity;
                }
                else
                {

                    scase.RandomLeadDays = 0;
                    scase.LeadDays = 0;
                    scase.OrderQuantity = 0;
                }


                //Console.WriteLine(scase.EndingInventory);

                s.SimulationCases.Add(scase);



            }











            for (int i = 0; i < s.NumberOfDays; i++)
            {
                eAvg += s.SimulationCases[i].EndingInventory;
                sAvg += s.SimulationCases[i].ShortageQuantity;
            }
            eAvg = eAvg / s.NumberOfDays;
            sAvg = sAvg / s.NumberOfDays;
            s.PerformanceMeasures.ShortageQuantityAverage = sAvg;
            s.PerformanceMeasures.EndingInventoryAverage = eAvg;

            for (int i = 0; i < s.NumberOfDays; i++)
            {
                //Console.WriteLine("Beginning "+ s.SimulationCases[i].BeginningInventory+"   ENDING " +s.SimulationCases[i].EndingInventory);
                eAvg += s.SimulationCases[i].EndingInventory;
                sAvg += s.SimulationCases[i].ShortageQuantity;
            }



            String testingResult = TestingManager.Test(s, Constants.FileNames.TestCase1);

            MessageBox.Show(testingResult);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            table.Columns.Add("Day");
            table.Columns.Add("Cycle");
            table.Columns.Add("DayWithCycle");
            table.Columns.Add("BeginingInventory");
            table.Columns.Add("RandomDigitForDemand");
            table.Columns.Add("Demand");
            table.Columns.Add("EndingInventory");
            table.Columns.Add("ShotageQuantity");
            table.Columns.Add("OrderQuantity");
            table.Columns.Add("Random Lead time");
            table.Columns.Add("LeadTime");

            dataGridView.DataSource = table;

            for (int i = 0; i < s.NumberOfDays; i++)
            {
                table.Rows.Add(Convert.ToString(s.SimulationCases[i].Day), Convert.ToString(s.SimulationCases[i].Cycle), Convert.ToString(s.SimulationCases[i].DayWithinCycle),
                    Convert.ToString(s.SimulationCases[i].BeginningInventory), Convert.ToString(s.SimulationCases[i].RandomDemand), Convert.ToString(s.SimulationCases[i].Demand),
                    Convert.ToString(s.SimulationCases[i].EndingInventory), Convert.ToString(s.SimulationCases[i].ShortageQuantity),
                    s.SimulationCases[i].OrderQuantity, Convert.ToString(s.SimulationCases[i].RandomLeadDays), Convert.ToString(s.SimulationCases[i].LeadDays));

            }

        }
    }

      
    
}
