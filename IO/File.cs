using System.Diagnostics;
using static System.IO.File;

namespace IO
{
	static class File
	{
		public static char[,]? LoadMapCharData(string pathName)
		{
			FileStream fs;

            try
            {
                fs = Open(pathName, FileMode.Open);
            }
			catch (FileNotFoundException)
			{
				Debug.WriteLine($"File at{pathName} could not be found.");

				return null;
			}

            return ParseFileStream(fs);
        }

		private static char[,] ParseFileStream(FileStream fs)
        {
            (int dimJ, int dimI) = GetDimentions(fs);
            var charData = new char[dimJ, dimI];

            for (int j = 0; j < dimJ; j++)
            {
                for (int i = 0; i < dimI; i++)
                {
                    char c = (char)fs.ReadByte();

                    if (c == -1)
                    {
                        j = dimJ;
                        break;
                    }
                    else if (c == '\n')
                    {
                        break;
                    }
                    else
                    {
                        charData[j, i] = c;
                    }
                }
            }

            return charData;
        }

        private static (int, int) GetDimentions(FileStream fs)
        {
            throw new NotImplementedException();
        }
	}
}
