using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using VideoTekka.model;
using static System.Net.Mime.MediaTypeNames;

namespace VideoTekka
{
    /// <summary>
    /// Logika interakcji dla klasy Create.xaml
    /// </summary>
    public partial class Create : Window
    {
        private static string connection_string = "Data Source = MovieBase.db";
        private string _image_path;
        private byte[] _image;       
        Movie movie = new Movie();
        public Create()
        {
            InitializeComponent();
        }

  

        private void bCreate_Click(object sender, RoutedEventArgs e)
        {
            movie.Title = txtTitle.Text;
            movie.Description = txtDescription.Text;
            movie.Director = txtDirector.Text;
            
           
            FileStream fileStream = File.Open(_image_path, FileMode.Open);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                _image = memoryStream.ToArray();

            }
            SqliteConnection sqliteConnection = new SqliteConnection(connection_string);
            sqliteConnection.Open();
            SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();           
            sqliteCommand.CommandText = $"INSERT INTO Movies (Title,Director,Description,Cover) VALUES('{movie.Title}','{movie.Director}','{movie.Description}',@Image)";
            sqliteCommand.Connection = sqliteConnection;
            sqliteCommand.Parameters.Add("@Image", SqliteType.Blob, _image.Length).Value = _image;
            sqliteCommand.ExecuteNonQuery();
            sqliteConnection.Close();

            this.Close();
        }


        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       

        private void bLoadCover_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            _image_path = openFileDialog.FileName;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(_image_path);
            image.EndInit();
            Cover.Source= image;
        }
    }
}
