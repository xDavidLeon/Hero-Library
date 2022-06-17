using System.Collections.Generic;
using System.IO;

namespace HeroLib
{
    public static class IOExt
    {
        public static byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
#if !UNITY_WEBPLAYER
            FileStream fs = new FileStream(fileName,
                FileMode.Open,
                FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
#endif
            return buff;
        }

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
#if !UNITY_WEBPLAYER
            foreach (string sp in searchPatterns)
                files.AddRange(System.IO.Directory.GetFiles(path, sp, searchOption));
            files.Sort();
#endif
            return files.ToArray();
        }

        public static string GetStringFromBytes(byte[] bytes)
        {
            string _string = System.Text.Encoding.UTF8.GetString(bytes);

            for (int i = 0; i < _string.Length; i++)
            {
                if (_string[i] == '\0')
                {
                    _string = _string.Remove(i);
                    break;
                }
            }

            return _string;
        }

        public static byte[] GetBytesFromString(string data)
        {
            return System.Text.Encoding.UTF8.GetBytes(data);
        }

        /// <summary>
        /// returns a new line string
        /// </summary>
        public static string IOEndOfLine
        {
            get
            {
                return "\n";
                //return System.Environtment.newLine;
            }
        }

        /// <summary>
        /// returns ": "
        /// </summary>
        public static string IOIntroStr
        {
            get { return ": "; }
        }
    }
}