namespace lilexcel

{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            
            InitializeComponent();
            
        }

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        public void AddDataToGridView(string data1, string data2, string data3, string data4, string data5)
        {
            dataGridView1.Rows.Add(data1, data2, data3, data4, data5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.Show();
            
        }

        private void LoadCsvData(string filePath)
        {
            
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                
                string[] headers = streamReader.ReadLine().Split(',');

                
                foreach (string header in headers)
                {
                    dataGridView1.Columns.Add(header, header);
                }

                
                while (!streamReader.EndOfStream)
                {
                    string[] rowValues = streamReader.ReadLine().Split(',');

                    
                    int rowIndex = dataGridView1.Rows.Add();
                    for (int i = 0; i < rowValues.Length; i++)
                    {
                        dataGridView1.Rows[rowIndex].Cells[i].Value = rowValues[i];
                    }
                }
            }

            MessageBox.Show("File loaded successfully.", "Load File", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                   
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        streamWriter.Write(column.HeaderText + ",");
                    }
                    streamWriter.WriteLine();

                    
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            
                            string cellValue = cell.Value?.ToString().Replace(",", " ") ?? "";
                            streamWriter.Write(cellValue + ",");
                        }
                        streamWriter.WriteLine();
                    }
                }

                MessageBox.Show("File saved successfully.", "Save File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                LoadCsvData(filePath);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow item in dataGridView1.SelectedRows)
            {
                if (item.Selected)
                {
                    dataGridView1.Rows.RemoveAt(item.Index);
                }
            }    
        }
    }
}