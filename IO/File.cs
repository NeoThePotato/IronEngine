using System.Diagnostics;
using System.IO;

namespace IO.File
{
	static class Map
	{
		public static char[,]? LoadMapCharData(string pathName)
		{
            StreamReader sr;

            try
            {
                sr = new StreamReader(pathName);
            }
			catch (FileNotFoundException)
			{
				Debug.WriteLine($"File at {pathName} could not be found.");

				return null;
			}

            var charData = ParseStreamReader(sr);
            sr.Close();

            return charData;
        }

		private static char[,] ParseStreamReader(StreamReader sr)
        {
            (int dimJ, int dimI) = GetDimensions(sr);
            ResetStreamReader(sr);
            var charData = new char[dimJ, dimI];
            string? line;
            
            for (int j = 0; (line = sr.ReadLine()) != null; j++)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    charData[j, i] = line[i];
                }
            }

            return charData;
        }

        private static (int, int) GetDimensions(StreamReader sr)
        {
            ResetStreamReader(sr);

            return GetDimensions(GetStringList(sr));
        }

        private static (int, int) GetDimensions(List<string> lines)
        {
            int sizeJ = lines.Count, sizeI = 0;

            foreach (string line in lines)
                sizeI = Math.Max(sizeI, line.Length);

            return (sizeJ, sizeI);
        }

        private static List<string> GetStringList(StreamReader sr)
        {
            List<string> lines = new List<string>(10);
            string? line;

            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines;
        }

        private static void ResetStreamReader(StreamReader sr)
        {
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        }
	}
}
