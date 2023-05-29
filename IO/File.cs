using System.Diagnostics;
using static System.IO.File;

namespace IO
{
	static class File
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
            (int dimJ, int dimI) = GetDimentions(sr);
            var charData = new char[dimJ, dimI];
            string? line;
            int j = 0, i = 0;

            while ((line = sr.ReadLine()) != null)
            {
                foreach (char c in line)
                {
                    charData[j, i] = c;
                    i++;
                }
                j++;
            }

            return charData;
        }

        private static (int, int) GetDimentions(StreamReader sr)
        {
            return GetDimentions(GetStringList(sr));
        }

        private static (int, int) GetDimentions(List<string> lines)
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
	}
}
