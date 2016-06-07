using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace LeapRecorder
{
    class Program
    {
        readonly Leap.Controller _c;
        private FileStream _file;
        private int _countOfFrames;
        private ILeapSerializer _serializer;

        static void Main(string[] args)
        {
            Console.Write("Insert filename: ");
            var fileName = Console.ReadLine();
            var program = new Program(fileName);
            Console.WriteLine("Press any key to stop");
            Console.ReadKey();
            program.Stop();
            Console.WriteLine($"Frames captured: {program._countOfFrames}");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private Program(string fileName)
        {
            _c = new Controller();
            _serializer = new LeapFrameSerializer();
            _file = new FileStream(fileName, FileMode.OpenOrCreate);
            _c.FrameReady += WriteFrames;
        }

        private void WriteFrames(object sender, FrameEventArgs e)
        {
            var frame = _serializer.Serialize(e.frame);
            try
            {
                var length = frame.Length;
                _file.Write(BitConverter.GetBytes(length), 0, 4);
                _file.Write(frame, 0, length);
                _countOfFrames++;
            }
            catch (Exception)
            {
            }
        }

        private void Stop()
        {
            _file.Close();
            _c.FrameReady -= WriteFrames;
        }
        
    }
}
