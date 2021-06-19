using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace WpfApp11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Student> ListaStudentow { get; set; }
        public List<Ocena> ListaOcen { get; set; }

        public MainWindow()
        {

            ListaStudentow = new List<Student>() 
            {
                new Student(){imie="Jan",nazwisko="Kowalski",NrIndeksu=1234,Wydzial="KIS", },
                new Student(){imie="Anna",nazwisko="Nowak",NrIndeksu=4321,Wydzial="KIS"},
                 new Student(){imie="Michał",nazwisko="Jacek",NrIndeksu=34562,Wydzial="KIS"},
             
             



            };
          

            InitializeComponent();

            dgStudent.Columns.Add(new DataGridTextColumn() { Header = "Imie", Binding = new Binding("imie") });
            dgStudent.Columns.Add(new DataGridTextColumn() { Header = "Nazwisko", Binding = new Binding("nazwisko") });
            dgStudent.Columns.Add(new DataGridTextColumn() { Header = "NrIndeksu", Binding = new Binding("NrIndeksu") });
            dgStudent.Columns.Add(new DataGridTextColumn() { Header = "Wydzial", Binding = new Binding("Wydzial") });
          
            
            dgStudent.AutoGenerateColumns = false;
            dgStudent.ItemsSource = ListaStudentow;
        }

        private void bAddStudent_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new StudentWindow();
            if(dialog.ShowDialog()==true)
            {
                ListaStudentow.Add(dialog.student);
                dgStudent.Items.Refresh();
            }
        }

        private void bRemoveStudent1_Click(object sender, RoutedEventArgs e)
        {
            if(dgStudent.SelectedItem is Student)
            
                ListaStudentow.Remove((Student)dgStudent.SelectedItem);
                dgStudent.Items.Refresh();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream("data.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("[[Student]]");

            sw.Close();
        }

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {

            FileStream fs = new FileStream("data.txt", FileMode.Create);
            StreamReader sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                var ln = sr.ReadLine();
            }

            sr.Close();
        }
        void Save<T> (T ob, StreamWriter sw)
        {
            Type t = ob.GetType();
            sw.WriteLine($"[[t.FullName]]");
            foreach (var p in t.GetProperties())
            {
                sw.WriteLine($"[{p.Name}]");
                sw.WriteLine(p.GetValue(ob));
            }
            sw.WriteLine($"[[]]");
        }
        T Load <T> (StreamReader sr) where T: new()
        {
            T ob = default(T);
            Type tob = null;
            PropertyInfo property = null;
            while (!sr.EndOfStream)
            {

                var ln = sr.ReadLine();
                if (ln == "[[]]")
                    return ob;
                else if (ln.StartsWith("[["))
                {
                    tob = Type.GetType(ln.Trim('[', ']'));
                    if (typeof(T).IsAssignableFrom(tob))
                        ob = (T)Activator.CreateInstance(tob);
                }
                else if (ln.StartsWith("[") && ob != null)
                    property = tob.GetProperty(ln.Trim('[', ']'));
                else if (ob != null && property != null)
                    property.SetValue(ob, Convert.ChangeType(ln, property.PropertyType));

            }

            return default(T);
        }

        private void SaveToXml_Click(object sender, RoutedEventArgs e)
        {
            Samochod b = new Samochod();
            FileStream fs = new FileStream("./samochod.xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Samochod));
            serializer.Serialize(fs, b);
            fs.Close();

        }

        private void LoadFromXml_Click(object sender, RoutedEventArgs e)
        {
            if(File.Exists("./samochod.xml"))
            {

                Samochod b;
                FileStream fs = new FileStream("./samochod.xml", FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(Samochod));
                b = (Samochod)serializer.Deserialize(fs);
                fs.Close();
            }
        }
    }

}

