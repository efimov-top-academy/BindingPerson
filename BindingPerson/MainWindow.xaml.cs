using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BindingPerson
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PersonModel model;
        public MainWindow()
        {
            InitializeComponent();

            List<Person> persons = new();

            string path = @"D:\RPO\table.xlsx";
            FileInfo fileInfo = new FileInfo(path);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

            int rows = worksheet.Dimension.Rows;
            int columns = worksheet.Dimension.Columns;

            for (int i = 2; i <= rows; i++)
            {
                Person person = new();
                for (int j = 1; j <= columns; j++)
                {

                    string content = "";

                    if (worksheet.Cells[i, j].Value != null)
                        content = worksheet.Cells[i, j].Value.ToString();

                    switch (j)
                    {
                        case 1: person.Time = content; break;
                        case 2: person.FirstName = content; break;
                        case 3: person.LastName = content; break;
                        case 4: person.Phone = content; break;
                        case 5: person.Email = content; break;
                    }

                }
                persons.Add(person);
            }

            model = new PersonModel(persons);
            DataContext = model;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Random random = new();
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            timer.Tick += timerTick;
            timer.Start();

            var timer10 = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer10.Tick += (sender, e) =>
            {
                timer.Stop();
                timer10.Stop();
                return;
            };
            timer10.Start();
            
            void timerTick(object sender, EventArgs e)
            {
                int index = random.Next(0, model.Persons.Count - 1);
                model.CurrentPerson = model.Persons[index];
                
            }
            
        }
    }
}
