// See https://aka.ms/new-console-template for more information
using ConsoleApp3;
using System.IO.MemoryMappedFiles;
using System.Text;

string filePath = "generated_text_file.txt";
int targetSizeMB = 150;
int chunkSize = 1024 * 1024; // 1 MB chunk size

long targetSizeBytes = targetSizeMB * 1024L * 1024L;

using (StreamWriter fileWriter = new StreamWriter(filePath))
{
    long currentSizeBytes = 0;

    while (currentSizeBytes < targetSizeBytes)
    {
        long remainingSizeBytes = targetSizeBytes - currentSizeBytes;
        int currentChunkSize = (int)Math.Min(chunkSize, remainingSizeBytes);

        string randomText = FileGenerator.GenerateRandomString(currentChunkSize);
        var append = randomText + "&";
        fileWriter.Write(append);

        currentSizeBytes += currentChunkSize;
    }
}

Console.WriteLine($"Text file generated: {filePath}");



//using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
//{
//    using (StreamReader reader = new StreamReader(fileStream))
//    {
//        StringBuilder lineBuilder = new StringBuilder();
//        char[] buffer = new char[4096]; // Set the buffer size as needed

//        while (!reader.EndOfStream)
//        {
//            int bytesRead = reader.Read(buffer, 0, buffer.Length);

//            for (int i = 0; i < bytesRead; i++)
//            {
//                char currentChar = buffer[i];

//                if (currentChar == '&')
//                {
//                    // Process the line here
//                    // Example: Console.WriteLine(lineBuilder.ToString());
//                    lineBuilder.Clear(); // Reset the StringBuilder for the next line
//                }
//                else
//                {
//                    lineBuilder.Append(currentChar);
//                }
//            }
//        }

//        // Process any remaining characters after the last newline (if needed)
//        if (lineBuilder.Length > 0)
//        {
//            // Process the last line (if any)
//            // Example: Console.WriteLine(lineBuilder.ToString());
//        }
//    }
//}

int fileSize = 1024; // Set the size of the memory-mapped file in bytes

// Create a memory-mapped file
using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Create, "MyMappedFile", fileSize))
{
    // Write data to the memory-mapped file
    using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
    {
        string dataToWrite = "Hello, this is some data to write to the memory-mapped file!";
        byte[] dataBytes = Encoding.UTF8.GetBytes(dataToWrite);

        accessor.WriteArray(0, dataBytes, 0, dataBytes.Length);
    }
}

// Read data from the memory-mapped file
using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("MyMappedFile"))
{
    using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
    {
        byte[] readData = new byte[fileSize];
        accessor.ReadArray(0, readData, 0, fileSize);

        string dataRead = Encoding.UTF8.GetString(readData).TrimEnd('\0');
        Console.WriteLine("Data read from memory-mapped file: " + dataRead);
    }
}


