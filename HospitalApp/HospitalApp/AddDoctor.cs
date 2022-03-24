using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalApp.Models
{
    public partial class AddDoctor : Form
    {
        public AddDoctor()
        {
            InitializeComponent();

            doctorBindingSource.DataSource = new Doctor();
            IsClosed = false;
        }

        private string textDbLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\textDb.txt";
        public bool IsClosed { get; internal set; }

        //преписва въведените стойности в textDb файла
        private void SaveNewDoctor()
        {
            using (FileStream fileStream = new FileStream(textDbLocation, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(firstNameTextBox.Text);
                    streamWriter.WriteLine(middleNameTextBox.Text);
                    streamWriter.WriteLine(lastNameTextBox.Text);
                    streamWriter.WriteLine(typeTextBox.Text);
                    streamWriter.WriteLine(nINTextBox.Text);
                }
            }
        }
        private void CancelCreation(object sender, EventArgs e)
        {
            IsClosed = true;
            this.Close();
        }

        //проверява за резултатите от валидацията ако няма грешки запазва добавения доктор
        //връча първата грешка
        private void Add(object sender, EventArgs e)
        {
            doctorBindingSource.EndEdit();
            Doctor doctor = doctorBindingSource.Current as Doctor;
            if (doctor != null)
            {
                ValidationContext validationContext = new ValidationContext(doctor);
                List<ValidationResult> results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(doctor, validationContext, results, true))
                {
                    foreach (var validationResult in results)
                    {
                        MessageBox.Show(validationResult.ErrorMessage, "Messege", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            SaveNewDoctor();
            IsClosed = true;
            this.Close();
        }
    }
}
