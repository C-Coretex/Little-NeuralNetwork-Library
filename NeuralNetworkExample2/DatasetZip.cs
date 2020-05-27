using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;

namespace NeuralNetworkExample2
{
    public class DatasetZip
    {
        struct DataElement
        {
            public string[] pathes;
            public int num;

            public DataElement(string[] pathes, int num)
            {
                this.pathes = pathes ?? throw new ArgumentNullException(nameof(pathes));
                this.num = num;
            }
        }

        string pathToZip;
        string folderName;
        bool extracted = false;
        DataElement[] dataElements = new DataElement[10];


        public DatasetZip(string pathToZip, string folderName = "dataset")
        {
            this.pathToZip = pathToZip;
            this.folderName = folderName;

            if (Directory.Exists(folderName))
            {
                Directory.Delete(folderName, true);
            }
        }

        ~DatasetZip()
        {
            Directory.Delete(folderName, true);
        }
        public void Extract()
        {
            using (ZipArchive archive = ZipFile.OpenRead(pathToZip))
            {
                archive.ExtractToDirectory(folderName);
            }

            for (int i = 0; i <= 9; i++)
            {
                int num = i;
                string[] files = Directory.GetFiles($"dataset\\{num}");

                dataElements[i] = new DataElement(files, num);
            }

            extracted = true;
        }

        public Bitmap GetRandomImageByNum(int num)
        {
            if (!extracted)
            {
                throw new Exception("you must first extract .zip");
            }
            return (Bitmap)Bitmap.FromFile(RandomElement(dataElements[num].pathes));
        }

        Random rnd = new Random();
        string RandomElement(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException($"{array} is null or empty");
            }

            return array[rnd.Next(0, array.Length - 1)];
        }
    }
}
