using HospitalApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //ако няма файл textDb на предназначената локация той се създава
            //ако има файл се извличат данните от него
            if (File.Exists(textDbLocation) != true)
            {
                CreateTextDb();
            }
            else
            {
                GetDataFromText();
            }
            cancelSelection.Visible = false;
        }
        private string textDbLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\textDb.txt";
        private Doctor[] DoctorArr = new Doctor[1000];

        //за източник на данни на doctorBindingSource се задава DoctorArr
        //doctorBindingSource се използва за източник на doctorDataGridView
        private void BindData()
        {
            doctorBindingSource.DataSource = DoctorArr;
            doctorDataGridView.DataSource = doctorBindingSource;
        }
        public void CreateTextDb()
        {
            FileStream fs = File.Create(textDbLocation);
        }

        //извлича данните от textDb всеки ред съответства на параметър на доктор
        private void GetDataFromText()
        {
            int index = 0;
            using (StreamReader streamReader = new StreamReader(File.Open(textDbLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                while (streamReader.EndOfStream != true)
                {
                    DoctorArr[index] = new Doctor()
                    {
                        FirstName = streamReader.ReadLine(),
                        MiddleName = streamReader.ReadLine(),
                        LastName = streamReader.ReadLine(),
                        Type = streamReader.ReadLine(),
                        NIN = streamReader.ReadLine()
                    };
                    index++;
                }
            }
            BindData();
        }

        //скрива всички редове които нямат търсеното ЕГН
        //проверява дали ЕГН-то е правилно написано, е празно или не съответства
        //изписва се съобщение ако е срещната една от горе споменатите опции
        private void GetDoctor(object sender, EventArgs e)
        {
            int? indexOfDoctor = null;
            var NIN = searchBox.Text;
            for (int i = 0; i < DoctorArr.Length; i++)
            {
                if (Regex.IsMatch(NIN, "^[0-9]{10}"))
                {
                    try
                    {
                        if (DoctorArr[i].NIN == NIN)
                        {
                            indexOfDoctor = i;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        searchBox.Text = "";
                        NIN = "";
                        MessageBox.Show("Не съществува лекар с посоченото ЕГН", "Messege", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }
                else if (NIN == String.Empty)
                {
                    searchBox.Text = "";
                    NIN = "";
                    MessageBox.Show("ЕГН полето не може да е празно!", "Messege", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
                else
                {
                    searchBox.Text = "";
                    NIN = "";
                    MessageBox.Show("ЕГН полето не е попълнено правилно!", "Messege", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }
            if (indexOfDoctor != null)
            {
                doctorBindingSource.EndEdit();
                for (int i = 0; i < doctorDataGridView.Rows.Count; i++)
                {
                    if (i != indexOfDoctor)
                    {
                        doctorDataGridView.Rows[i].Visible = false;
                    }
                    else
                    {
                        doctorDataGridView.Rows[i].Visible = true;
                    }
                }
                searchBox.Text = "";
                cancelSelection.Visible = true;
            }
        }

        //отваря формата за добавяне на нов доктор
        private void AddDoctor_Click(object sender, EventArgs e)
        {

            AddDoctor addDoctorForm = new AddDoctor();
            addDoctorForm.ShowDialog();
            while (true)
            {
                if (addDoctorForm.IsClosed == true)
                {
                    RefreshGrid();
                    break;
                }
            }
        }
        public void RefreshGrid()
        {
            GetDataFromText();
            doctorDataGridView.Update();
            doctorDataGridView.Refresh();
        }

        //разкрива всички редове които са били скрити при търсенето на ЕГН
        private void cancelSelection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < doctorDataGridView.Rows.Count; i++)
            {
                doctorDataGridView.Rows[i].Visible = true;
            }
            cancelSelection.Visible = false;
        }
    }
}
