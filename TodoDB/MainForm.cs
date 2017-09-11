using LiteDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TodoDB
{
    public partial class MainForm : Form
    {
        // List<Todo> list = new List<Todo>();
        BindingList<Todo> list = new BindingList<Todo>();

        public MainForm()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTask();
        }

        private void AddTask()
        {
            var a = textBox1.Text.ToString();
            textBox1.Text = "";
            if (!string.IsNullOrEmpty(a))
            {
                using (var db = new LiteDatabase(@"MyData.db"))
                {
                    // Get customer collection
                    var col = db.GetCollection<Todo>("todos");

                    // Create your new customer instance
                    var todo = new Todo
                    {
                        Name = a,
                    };

                    // Create unique index in Name field
                    //col.EnsureIndex(x => x.Name, true);

                    // Insert new customer document (Id will be auto-incremented)
                    col.Insert(todo);
                    list.Add(todo);
                   

                    //listBox1.Items.Add(todo.Name);

                    // Update a document inside a collection
                    //todo.Name = "Lioness";

                    //col.Update(todo);

                    // Use LINQ to query documents (with no index)
                }
                //BindingManagerBase myBindingManager2 = BindingContext[list];
                //myBindingManager2.SuspendBinding();
                //myBindingManager2.ResumeBinding();
                listBox1.DataSource = list;
                listBox1.DisplayMember = "Name";
                
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                // Get customer collection
                var col = db.GetCollection<Todo>("todos");

                col.Delete(x => x.Name == "");

                foreach (var item in col.FindAll())
                {
                    list.Add(item);
                }


                //foreach (var item in col.FindAll())
                //{
                //    listBox1.Items.Add(item.Name);
                //}

                // Update a document inside a collection
                //todo.Name = "Lioness";

                //col.Update(todo);

                // Use LINQ to query documents (with no index)
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Name = "Name"; // name
            dataGridView1.Columns[0].HeaderText = "Name"; // header text
            dataGridView1.Columns[0].DataPropertyName = "Name"; // field 
            dataGridView1.Columns[0].Width = 169;

            DataGridViewCheckBoxColumn c = new DataGridViewCheckBoxColumn();
            c.Name = "Done";
            c.HeaderText = "Done";
            c.DataPropertyName = "Done";
            dataGridView1.Columns.Add(c);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.DataSource = list;

            listBox1.DataSource = list;
            listBox1.DisplayMember = "Name";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem;
          //  MessageBox.Show(selected);
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var col = db.GetCollection<Todo>("todos");
                col.Delete(x => x.Name == ((Todo)selected).Name);
            }
            list.Remove((Todo)selected);
           // listBox1.Items.Remove(selected);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                AddTask();
            }
        }

      

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selected = dataGridView1.CurrentRow;
            var b = selected.Cells["Done"].OwningColumn.DataPropertyName;
            var c = dataGridView1.CurrentCell.OwningColumn.DataPropertyName;
            if (c == "Done")
            {
                using (var db = new LiteDatabase(@"MyData.db"))
                {
                    var col = db.GetCollection<Todo>("todos");
                    Todo a = col.Find(x => x.Name == selected.Cells["Name"].Value.ToString()).FirstOrDefault();
                    a.Done = !a.Done;
                    col.Update(a);
                }
            }
           
          
        }
    }
}
