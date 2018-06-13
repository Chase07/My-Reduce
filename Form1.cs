using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
//using System.Collections;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        SqlCommand WCommand;
        SqlDataAdapter WDataAdapter;
        SqlDataReader WDataReader;

        DataSet MDataSet = new DataSet();
        DataView DV;

        // 决策表的记录总数 
        public int ObjectCounts;
        string table_name;
        string normalized_table_name;




        public Form1()
        {
            InitializeComponent();

            // Disable those buttons
            Normalize.Enabled = false;
            NRReduce.Enabled = false;
            MRMRReduce.Enabled = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void sqlConnection1_InfoMessage(object sender, System.Data.SqlClient.SqlInfoMessageEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)// Open button
        {
            /*
             * 后期再优化打开按键的操作 2017-12-28-16:56
             */
            // Disable those buttons
            Open.Enabled = false;

            WCommand = sqlConnection1.CreateCommand();
            sqlConnection1.Open();// Set sqlConnection1 in an open status
            WCommand.Connection = sqlConnection1;// Get an available sqlconnection

            // Get SQL statements
            WCommand.CommandText = "SELECT * FROM glass;"; table_name = "glass";
            //WCommand.CommandText = "SELECT * FROM wine;"; table_name = "wine";
            //WCommand.CommandText = "SELECT * FROM zoo;"; table_name = "zoo";
            
            WDataAdapter = new SqlDataAdapter(WCommand);// New a SqlDataAdapter by SQL statements           
            WDataAdapter.Fill(MDataSet, table_name);// Fill the MDataSet with the result of Sql statements       
            DV = new DataView(MDataSet.Tables[0]);// New DataView with the first Sql statement
            gDataView.DataSource = DV;// Demonstrate on gDataView                       

            Normalize.Enabled = true;
        }
        private void normalizing(List<double> source_data, List<double> result_data)
        {
            double max_of_col = source_data.Max();// Store the maximum value
            double mix_of_col = source_data.Min();// Store the minimum value            
            foreach (double curr in source_data)
            {
                double rescaling_value = (curr - mix_of_col) / (max_of_col - mix_of_col);
                result_data.Add(rescaling_value);
            }
        }
        private void insert_table(List<List<double>> source_data, string table_name)
        {
            // Insert those condition classes
            for (int curr_obj = 0; curr_obj <= DV.Table.Rows.Count - 1; ++curr_obj)
            {
                WCommand.CommandText = "INSERT INTO " + table_name + "(";
                for (int i = 0; i <= DV.Table.Columns.Count - 1; i++)
                {
                    if (i < DV.Table.Columns.Count - 1)
                    { WCommand.CommandText += DV.Table.Columns[i].ColumnName + ", "; }
                    else
                    { WCommand.CommandText += DV.Table.Columns[i].ColumnName + ") VALUES("; }
                }
                for (int curr_cond = 0; curr_cond <= DV.Table.Columns.Count - 1; ++curr_cond)
                {
                    if (curr_cond < DV.Table.Columns.Count - 1)
                    { WCommand.CommandText += source_data[curr_cond][curr_obj] + ", "; }
                    else
                    { WCommand.CommandText += source_data[curr_cond][curr_obj] + ");"; }
                }
                WCommand.ExecuteNonQuery();
            }            
        }
        private void button2_Click(object sender, EventArgs e)// Normalize button
        {
            Normalize.Enabled = false;

            normalized_table_name = table_name + "_normalized";
            WCommand.CommandText = "SELECT * FROM sysobjects WHERE NAME = '" + normalized_table_name + "';";
            WDataReader = WCommand.ExecuteReader();

            if (WDataReader.HasRows == false)
            {// Now normalized table is inexistent

                // Create a new table with the structure of old table
                WDataReader.Close();// Close it first
                WCommand.CommandText = "SELECT * INTO " + normalized_table_name + " FROM " + table_name + " WHERE 1 = 0;";
                WCommand.ExecuteNonQuery();
              
                List<List<double>> normalized_data = new List<List<double>>();
                // Normalizing all data and store in normalized_data                
                for (int curr_col = 0; curr_col <= DV.Table.Columns.Count - 1; ++curr_col)// Include the decision column
                {
                    List<double> col_data = new List<double>();
                    List<double> normalized_col_data = new List<double>();
                    // Store each column in a col_data
                    for (int curr_row = 0; curr_row <= DV.Table.Rows.Count - 1; ++curr_row)
                    {
                        col_data.Add(Double.Parse(DV[curr_row][curr_col].ToString()));
                    }
                    if (curr_col < DV.Table.Columns.Count - 1)
                    {
                        normalizing(col_data, normalized_col_data); // Normalizing data in one column
                        normalized_data.Add(normalized_col_data);// Add one column normalized data in it each time
                    }
                    else { normalized_data.Add(col_data); }// Decision column doesn't need to normalize

                    //调试使用
                    //if (curr_col == DV.Table.Columns.Count - 1)
                    //{
                    //    int j = 0;
                    //    foreach (List<double> curr_col_data in normalized_data)
                    //    {
                    //        int i = 0;
                    //        Out.AppendText("c" + (++j) + "\n");
                    //        foreach (double curr in curr_col_data)
                    //        {
                    //            Out.AppendText(++i + ": " + curr + "\n");
                    //        }
                    //        Out.AppendText("\n\n");
                    //    }
                    //}
                }
                insert_table(normalized_data, normalized_table_name);
                //调试使用
                //WCommand.CommandText = "DROP TABLE normalized_table_name;";
                //WCommand.ExecuteNonQuery();
            }

            WDataReader.Close();// Close it first            
            WCommand.CommandText = "SELECT * FROM " + normalized_table_name + ";";                   
            WDataAdapter.Fill(MDataSet, normalized_table_name);// Fill the MDataSet with the normalized table
            DV = new DataView(MDataSet.Tables[1]);// New DataView with the the normalized table
            gDataView.DataSource = DV;// Demonstrate on gDataView

            NRReduce.Enabled = true;
            MRMRReduce.Enabled = true;
        }           

   
        private double euclidean_distance(List<double> Xi, List<double> Xj)
        {
            double result = 0.0;
            if(Xi.Count == Xj.Count)
            {
                for (int index = 0; index < Xi.Count; ++index)
                {
                    double tempResult = (Xi[index] >= Xj[index] ? Xi[index] - Xj[index] : Xj[index] - Xi[index]);
                    tempResult *= tempResult;
                    result += tempResult;
                }
            }
            else { Console.WriteLine("Xi.Count != Xj.Count!\n"); }
            return result;
        }
        private void retrieveOneRow(List<double> X, List<string> curr_RED, int curr_row, int curr_col = -1)
        {
            X.Clear();
            if(curr_col != -1)
            { X.Add(Double.Parse(DV[curr_row][curr_col].ToString())); }// Add in the new condition

            // Add in the old condition
            if (curr_RED != null && curr_RED.Count > 0)
            {
                for (int temp_col = 0; temp_col <= DV.Table.Columns.Count - 2; ++temp_col)
                {
                    string tempColumnName = DV.Table.Columns[temp_col].ColumnName;
                    if (curr_RED.Contains(tempColumnName))
                    {
                        X.Add(Double.Parse(DV[curr_row][temp_col].ToString()));
                    }
                }
            }
            
        }
        private void copyTo(List<string> RED, ref string bestRED)
        {
            bestRED = "";
            foreach (string curr in RED)
            {
                bestRED = (bestRED == "" ? curr : bestRED + ", " + curr);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)// Neighbor Reduce button
        {         
            NRReduce.Enabled = false;
            MRMRReduce.Enabled = false;

            List<double> Xi = new List<double>();
            List<double> Xj = new List<double>();
            List<string> RED = new List<string>(), tempRED = new List<string>();

            string bestRED = "";
            string colName = "";
            double radius = 0.01;// threshold
            long neighborDAmount = 0;
            long neighborAmount = 0;
            double NU = 0.0;
            double Pos = 0.0;
            double NR = 0.0, tempNR = 0.0, bestNR = 0.0;
            bool flag = false;
            int conditionAmount = 1;// Counting the times do while loop
            do
            {
              
                for (int curr_col = 0; curr_col <= DV.Table.Columns.Count - 2; ++curr_col)//Except the dicision column
                {
                    Pos = 0.0;
                    colName = DV.Table.Columns[curr_col].ColumnName;
                    if (RED.Contains(colName)) { continue; }                    
                    for (int selected_row = 0; selected_row <= DV.Table.Rows.Count - 1; ++selected_row)
                    {                     
                        neighborAmount = neighborDAmount = 0;
                        retrieveOneRow(Xi, RED, selected_row, curr_col);
                        for (int contrast_row = 0; contrast_row <= DV.Table.Rows.Count - 1; ++contrast_row)
                        {
                            retrieveOneRow(Xj, RED, contrast_row, curr_col);
                            if (euclidean_distance(Xi, Xj) <= radius * radius)
                            {// current instance is a neighbor
                                ++neighborAmount;                                 
                                string decisionValue = DV[contrast_row][DV.Table.Columns.Count - 1].ToString();
                                if (decisionValue == DV[selected_row][DV.Table.Columns.Count - 1].ToString())// Two rows have same decision value                                
                                { ++neighborDAmount; }
                            }
                        }
                        NU = (neighborDAmount * 1.0) / neighborAmount;
                        Pos += NU;
                    }
                    tempNR = Pos / DV.Count;
                    if (tempNR > NR)
                    {
                        NR = tempNR;
                        if(tempRED.Count > conditionAmount - 1)// Make sure save the old optimal RED in the last do while loop
                        { tempRED.RemoveAt(tempRED.Count - 1); }// Drop the rear element of old optimal RED in the previous for(curr_col) loop 
                        tempRED.Add(colName);// Update this RED now
                        
                    }
                }
                RED = tempRED;
                if (NR - bestNR > 0.001)
                {
                    bestNR = NR;                   
                    copyTo(RED, ref bestRED);
                    Out.AppendText(bestRED + "\n");
                    ++conditionAmount;
                    flag = (RED.Count < DV.Table.Columns.Count - 1 ? false : true);// Set true while all conditions are contained in RED
                }
                else
                {
                    flag = true;
                }

            } while (flag == false);
            Out.AppendText("bestRED: " + bestRED + "\n");       

        }
        private void button1_Click_2(object sender, EventArgs e)// MRMR Reduce button
        {
            NRReduce.Enabled = false;
            MRMRReduce.Enabled = false;

            List<double> Xi = new List<double>();
            List<double> Xj = new List<double>();
            List<string> RED = new List<string>(), tempRED = new List<string>();
            Dictionary<string, double> allNR = new Dictionary<string, double>();// Store the neighbor relevance to decision value of all conditions

            string bestRED = "";
            string colName = "";
            double radius = 0.05;// threshold
            long neighborDAmount = 0, neighborAmount = 0;
            double NU = 0.0;
            double Pos = 0.0;
            double tempMrmrValue = 0.0, mrmrValue = 0.0, bestMrmrValue = 0.0;
            bool flag = false;
            int conditionAmount = 1;// Counting the times do while loop

            do
            {

                for (int curr_col = 0; curr_col <= DV.Table.Columns.Count - 2; ++curr_col)//Except the dicision column
                {
                    Pos = 0.0;
                    colName = DV.Table.Columns[curr_col].ColumnName;
                    if (RED.Contains(colName)) { continue; }

                    if (conditionAmount == 1)// Same as Neighbor Reduce
                    {
                        // Only computing a single condition
                        for (int selected_row = 0; selected_row <= DV.Table.Rows.Count - 1; ++selected_row)
                        {
                            neighborAmount = neighborDAmount = 0;
                            retrieveOneRow(Xi, RED, selected_row, curr_col);
                            for (int contrast_row = 0; contrast_row <= DV.Table.Rows.Count - 1; ++contrast_row)
                            {
                                retrieveOneRow(Xj, RED, contrast_row, curr_col);
                                if (euclidean_distance(Xi, Xj) <= radius * radius)
                                {// current instance is a neighbor
                                    ++neighborAmount;
                                    string decisionValue = DV[contrast_row][DV.Table.Columns.Count - 1].ToString();
                                    if (decisionValue == DV[selected_row][DV.Table.Columns.Count - 1].ToString())// Two rows have same decision value                                
                                    { ++neighborDAmount; }
                                }
                            }
                            NU = (neighborDAmount * 1.0) / neighborAmount;
                            Pos += NU;
                        }

                        double tempNR = Pos / DV.Count;
                        allNR.Add(colName, tempNR);// Collect the NR of all condition
                        if (tempNR > allNR[tempRED[0]])// tempRED only have one single element
                        {
                            if (tempRED.Count > conditionAmount - 1)
                            { tempRED.RemoveAt(tempRED.Count - 1); }// Drop the rear element of old optimal RED in the previous for(curr_col) loop
                            tempRED.Add(colName);// 第一个被收录的应该是c7
                        }
                    }

                    //if (conditionAmount > 1)
                    //{
                    //    tempMrmrValue = MRMR(allNR, tempRED, colName, radius);///////////////////////////////
                    //    if (tempMrmrValue > mrmrValue)
                    //    {
                    //        mrmrValue = tempMrmrValue;
                    //        if (tempRED.Count > conditionAmount - 1)
                    //        { tempRED.RemoveAt(tempRED.Count - 1); }// Drop the last element of previous optimal RED
                    //        tempRED.Add(colName);// Update this RED now
                    //    }
                    //}

                }
                RED = tempRED;
                if (mrmrValue - bestMrmrValue > 0.001 || conditionAmount == 1)
                {
                    bestMrmrValue = mrmrValue;
                    copyTo(RED, ref bestRED);
                    Out.AppendText(bestRED + "\n");
                    ++conditionAmount;
                    flag = (RED.Count < DV.Table.Columns.Count - 1 ? false : true);// Set true while all conditions are contained in RED
                }
                else
                {
                    flag = true;
                }

            } while (flag == false);
            Out.AppendText("bestRED: " + bestRED + "\n");

        }
        private double Relevance(Dictionary<string, double> allNR, List<string> tempRED, string colName)
        {
            double result = 0.0, sumOfNR = 0.0;

            foreach (string Ci in tempRED)
            {
                sumOfNR += allNR[Ci];
            }
            sumOfNR += allNR[colName];
            result = sumOfNR / tempRED.Count + 1;
            return result;
        }
        private double Redundancy(List<string> tempRED, string currColName, double radius)
        {
            double result = 0.0;
            Dictionary<string, Dictionary<string, double>> allNHM = new Dictionary<string, Dictionary<string, double>>();

            updateNHM(allNHM, tempRED[tempRED.Count - 1], currColName, radius);
            /////////////////////////////////////////

            return result;
        }
        private void updateNHM(Dictionary<string, Dictionary<string, double>> allNHM, string lastColNameofRED, string currColName, double radius)
        {
            List<double> Xi = new List<double>();
            List<double> Xj = new List<double>();
            long nebIntersectionAmount = 0, nebUnionAmount = 0;
            double NU = 0.0, sumOfNU = 0.0;
            double NHM = 0.0;

            List<string> conditionSet = new List<string> { lastColNameofRED, currColName };

            for (int selected_row = 0; selected_row <= DV.Table.Rows.Count - 1; ++selected_row)
            {
                retrieveOneRow(Xi, conditionSet, selected_row);
                for (int contrast_row = 0; contrast_row <= DV.Table.Rows.Count - 1; ++contrast_row)
                {
                    retrieveOneRow(Xj, conditionSet, contrast_row);
                    if (euclidean_distance(Xi, Xj) <= radius * radius)
                    {// current instance is a neighbor
                        ++nebUnionAmount;
                        string decisionValue = DV[contrast_row][DV.Table.Columns.Count - 1].ToString();
                        if (decisionValue == DV[selected_row][DV.Table.Columns.Count - 1].ToString())// Two rows have same decision value                                
                        { ++nebIntersectionAmount; }
                    }
                }
                NU = (nebIntersectionAmount * 1.0) / nebUnionAmount;
                sumOfNU += NU;
            }

            NHM = sumOfNU / DV.Count;
            //allNHM.Add(new Dictionary<string, Dictionary<string, double>>(lastColNameofRED, new Dictionary<string, double>(currColName, NHM)));
        }
        private double MRMR(Dictionary<string, double> allNR, List<string> tempRED, string currColName, double radius)
        {
            double result = 0.0;
            double RelValue = 0.0, RedValue = 0.0;

            RelValue = Relevance(allNR, tempRED, currColName);
            RedValue = Redundancy(tempRED, currColName, radius);
            result = RelValue - RedValue;
            return result;

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
