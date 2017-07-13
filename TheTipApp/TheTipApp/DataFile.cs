using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace TheTipApp
{
    class DataFile<T>{
        private string FilePath { get; set; }

        private bool FileEmpty;

        public DataFile()
        {
            //Set the File path
            SetFilePath("dataFile.txt");

            //Check if File is empty
            FileEmpty = IsFileEmpty();
        }

        private void SetFilePath(string fileName)
        {
            var sysPath = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            FilePath = Path.Combine(sysPath.ToString(), fileName);
        }

        private bool IsFileEmpty()
        {
            try
            {
                if (new FileInfo(FilePath).Length == 0)
                    return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return false;
        }

        #region Writing to File

        public void Write(T inputData)
        {
            if (inputData is Tip)
            {
                WriteTip((Tip)(object)inputData);
            }
            else if (inputData is List<Tip>)
            {
                WriteTipList((List<Tip>)(object)inputData);
            }
        }

        private void WriteTip(Tip newTip)
        {
            List<Tip> tipList;

            //If the file is not empty, then get the current data
            if (!FileEmpty)
                tipList = ReadFileData();
            else
                tipList = new List<Tip>();

            tipList.Add(newTip);

            WriteTipList(tipList);
        }

        private void WriteTipList(List<Tip> tipList)
        {
            //Serialize InputData
            string objData = JsonConvert.SerializeObject(tipList, Formatting.Indented);

            //Writing to file using a StreamWriter
            using (StreamWriter streamWriter = new StreamWriter(new FileStream(FilePath, FileMode.Truncate, FileAccess.ReadWrite)))
            {
                streamWriter.Write(objData);
            }
        }

        #endregion Writing to File

        #region Reading From File

        public List<Tip> Read()
        {
            if (!FileEmpty)
                return ReadFileData();
            else
                return new List<Tip>();
        }
        
        private List<Tip> ReadFileData()
        {
            string fileData;
            List<Tip> tipList;

            //Getting the contents of the file
            using (var streamReader = new StreamReader(new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                fileData = streamReader.ReadToEnd();
            }

            try
            {
                //Deserializing the Data
                tipList = JsonConvert.DeserializeObject<List<Tip>>(fileData);
            }
            catch (System.Exception e)
            {
                tipList = new List<Tip>();
            }

            return tipList;
        }

        #endregion Reading From File
    }
}