using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calendar.BLL; 

namespace Calendar.View
{
    public partial class Form2 : Form
    {
       public Form2()
        {
            InitializeComponent();
        }

        //Sign In
        private void button1_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Text == "" || richTextBox2.Text == "") {
                MessageBox.Show("Please fill the box");
            }
            else
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    foreach (User i in db.Users)
                    {
                        if(richTextBox1.Text == i.Name)
                        {
                            if(richTextBox2.Text == i.PassWord)
                            {
                                MessageBox.Show("Sign In Successfully");
                                Hide();
                                Form1 f = new Form1(richTextBox1.Text);
                                f.ShowDialog();
                                Close();
                                return;
                            }
                            


                        }
                    }

                    MessageBox.Show("Not Found User");
                }
            }
        }

        //Sign Up
        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "" || richTextBox2.Text == "")
            {
                MessageBox.Show("Please fill the box");
            }
            else
            {
                User u = new User
                {
                    Name= richTextBox1.Text,
                    PassWord = richTextBox2.Text,
                };
                Bll b = new Bll();
                if (b.AddUser(u))
                {
                    MessageBox.Show("Sign up successfully . Please click Sign In to start");
                }
                else
                {
                    MessageBox.Show("There are some error here");
                }
            }


        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            Bll b = new Bll();
            if (b.CheckUser(richTextBox1.Text, richTextBox2.Text))
            {
                MessageBox.Show("Please choose other name or pass because the data have already in database");
            }
        }
    }
}
