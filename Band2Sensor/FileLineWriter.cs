using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Environment = System.Environment;

namespace Band2Sensor
{
    class FileLineWriter : ILineWriter
    {
        private string fileName;

        public FileLineWriter(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //documentsPath = Path.Combine(documentsPath, "band");
            this.fileName = Path.Combine(documentsPath, filename);
        }

        public Task WriteLineAsync(IDictionary<string, object> values)
        {
            string line = string.Join(";", values.Select(p => p.Value)) + Environment.NewLine;
            System.IO.File.AppendAllText(this.fileName, line);
            return Task.FromResult(true);
        }
    }
}