using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace VimReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File (*.txt) | *.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, txtVimContent.Text);
            }
        }


        private void CommandBinding_Extract(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VIM Files (*.vim) | *.vim";
            if (openFileDialog.ShowDialog() == true)
            {
                var startTime = DateTime.Now;
                var fileName = openFileDialog.FileName;

                vib.Scene scene = new vib.Scene();
                vib.Deserializer.Load(fileName, out scene);


                StringBuilder sb = new StringBuilder(650000);
                sb.Append(System.IO.Path.GetFileName(fileName)).AppendLine();

                foreach (var cont in scene.Contents.Values)
                {
                    sb.Append($"contName: {cont.Name}");
                    sb.AppendLine($"  contKey: {cont.Key}");
                }
                sb.AppendLine();
                sb.AppendLine($"TEXTURES {scene.Textures.Count}");
                foreach (var tex in scene.Textures.Values)
                {
                    sb.Append($"texture key: {tex.Key}").AppendLine();
                    sb.Append($"texture filename: {tex.FileName}").AppendLine();
                }
                sb.AppendLine();

                sb.AppendLine("REGIONS");
                foreach (var region in scene.Regions.AllRegions())
                {
                    sb.AppendLine(region.ToString());
                    foreach(var co in region.Contents())
                    {
                        sb.AppendLine($"  region contKey: {co.Key}");
                        sb.Append($"   region contName: {co.Name}");

                    }
                }
                sb.AppendLine();

                foreach (var mesh in scene.Meshes.Values)
                {
                    sb.Append($"mesh key: {mesh.Key}").AppendLine();
                    foreach (var item in mesh.Lods[0].Surfaces)
                    {
                        sb.Append($"  surface Key: {mesh.UniqueKey}");
                        sb.Append($"  matKey: {item.MaterialKey}");
                        sb.AppendLine();
                    }
                }

                foreach (var mat in scene.Materials.Values)
                {
                    sb.Append($"matKey: {mat.Key}");
                    sb.Append($"  matName: {mat.Name}");
                    sb.Append($"  matClass: {mat.MaterialClass}");
                    sb.Append($"  matType: {mat.MaterialType}");
                    sb.AppendLine($"  base color: {mat.BaseColor}");
                }

                int i = 0;
                foreach (var data in scene.ObjectData.Values)
                {
                    string fam, el, id;
                    if (data.Descriptor.TryGetValue("Id", out id))
                    {
                        sb.Append("ID").Append(':').Append(' ').Append(id).AppendLine();
                    }
                    if (data.Descriptor.TryGetValue("Family name", out fam))
                    {
                        sb.Append("Family").Append(':').Append(' ').Append(fam).AppendLine();
                    }
                    if (data.Descriptor.TryGetValue("Name", out el))
                    {
                        sb.Append("Name").Append(':').Append(' ').Append(el).AppendLine();
                    }

                    foreach (KeyValuePair<string, string> kvp in data.Descriptor)
                    {
                        var keyTrim = kvp.Key.Trim();
                        var valueTrim = kvp.Value.Trim();
                        if (keyTrim != "Family name" && keyTrim != "Id" && keyTrim != "Name")
                        {
                            sb.Append(keyTrim).Append(':').Append(' ').Append(valueTrim).AppendLine();
                        }
                        i++;
                    }
                    sb.AppendLine();
                }

                var endTime = DateTime.Now;
                var totalTime = endTime - startTime;
                sb.Append($"Processing time: {totalTime.Minutes} minutes {totalTime.Seconds} seconds.");
                txtVimContent.Text = sb.ToString();
                txtCount.Text = i.ToString();
            }
        }

        private void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VIM Files (*.vim) | *.vim";
            if (openFileDialog.ShowDialog() == true)
            {
                var startTime = DateTime.Now;
                var fileName = openFileDialog.FileName;

                vib.Scene scene = new vib.Scene();
                vib.Deserializer.Load(fileName, out scene);


                StringBuilder sb = new StringBuilder(650000);
                sb.Append(System.IO.Path.GetFileName(fileName)).AppendLine();

                
                foreach (var cont in scene.Contents.Values)
                {
                    sb.Append($"contName: {cont.Name}");
                    sb.AppendLine($"  contKey: {cont.Key}");
                }


                foreach (var mesh in scene.Meshes.Values)
                {
                    sb.Append($"mesh key: {mesh.Key}").AppendLine();
                    foreach (var item in mesh.Lods[0].Surfaces)
                    {
                        sb.Append($"  surface Key: {mesh.UniqueKey}");
                        sb.Append($"  matKey: {item.MaterialKey}");
                        sb.AppendLine();
                    }
                }

                foreach (var mat in scene.Materials.Values)
                {
                    sb.Append($"matKey: {mat.Key}");
                    sb.Append($"  matName: {mat.Name}");
                    sb.Append($"  matClass: {mat.MaterialClass}");
                    sb.Append($"  matType: {mat.MaterialType}");
                    sb.AppendLine($"  base color: {mat.BaseColor}");
                }

                int i = 0;
                foreach (var data in scene.ObjectData.Values)
                {
                    string fam, el, id;
                    if (data.Descriptor.TryGetValue("Id", out id))
                    {
                        sb.Append("ID").Append(':').Append(' ').Append(id).AppendLine();
                    }
                    if (data.Descriptor.TryGetValue("Family name", out fam))
                    {
                        sb.Append("Family").Append(':').Append(' ').Append(fam).AppendLine();
                    }
                    if(data.Descriptor.TryGetValue("Name", out el))
                    {
                        sb.Append("Name").Append(':').Append(' ').Append(el).AppendLine();
                    }

                    foreach (KeyValuePair<string, string> kvp in data.Descriptor)
                    {
                        var keyTrim = kvp.Key.Trim();
                        var valueTrim = kvp.Value.Trim();
                        if (keyTrim != "Family name" && keyTrim != "Id" && keyTrim != "Name")
                        {
                            sb.Append(keyTrim).Append(':').Append(' ').Append(valueTrim).AppendLine();
                        }
                        i++;
                    }
                    sb.AppendLine();
                }
                
                var endTime = DateTime.Now;
                var totalTime = endTime - startTime;
                sb.Append($"Processing time: {totalTime.Minutes} minutes {totalTime.Seconds} seconds.");
                txtVimContent.Text = sb.ToString();
                txtCount.Text = i.ToString();
            };
        }
    }
}
