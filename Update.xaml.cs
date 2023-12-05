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

namespace VideoTekka
{
    /// <summary>
    /// Logika interakcji dla klasy Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        private static string connection_string = "Data Source = MovieBase.db";
        private string _image_path;
        private byte[] _image;
        private Movie movie = new Movie();

                      
        public Update(string Title)
        {
            
            InitializeComponent();
            SqliteConnection connection = new SqliteConnection(connection_string);
            SqliteCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = $"SELECT * FROM Movies WHERE Title = '{Title}'";
            command.Connection = connection;
            var reader = command.ExecuteReader();
            reader.Read();           
            movie.Id = Convert.ToInt32(reader["Id"]);
            movie.Title = reader["Title"].ToString();
            movie.Director = reader["Director"].ToString();
            movie.Description = reader["Description"].ToString();
            movie.cover = (byte[])reader["Cover"];
            _image = (byte[])reader["Cover"];
            txtTitle.Text = movie.Title;
            txtOpis.Text = movie.Description;
            txtDirector.Text = movie.Director;

            MemoryStream memoryStream = new MemoryStream(movie.cover);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            Cover.Source = bitmapImage;
        }

    
        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            movie.Title = txtTitle.Text;
            movie.Description = txtOpis.Text;
            movie.Director = txtDirector.Text;

          
            SqliteConnection sqliteConnection = new SqliteConnection(connection_string);
            sqliteConnection.Open();
            SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
            sqliteCommand.CommandText = $"UPDATE Movies SET Title = '{movie.Title}',Director = '{movie.Director}',Description = '{movie.Description}',Cover = @Image WHERE Id = {movie.Id}";
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

    
        private void bLoad_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            _image_path = openFileDialog.FileName; 
            
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(_image_path);
            image.EndInit();
            Cover.Source = image;

            FileStream fileStream = File.Open(_image_path, FileMode.Open);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                //movie.cover = memoryStream.ToArray();
                _image= memoryStream.ToArray();
            }
        }
    }
}
