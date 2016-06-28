using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestureRecognition.Implementation;
using GestureRecognition.Interface;
using GestureRecognition.Interface.Commands;
using GestureRecognition = GestureRecognition.Implementation.GestureRecognition;

namespace TestApp
{
    class Program
    {
        private IGestureRecognition recognition;
        private int count;

        public Program()
        {
            recognition = GestureRecognitionFactory.Create(new LeapGestureController());
        }

        private void run()
        {
            recognition.SubscribeToCommand<ScaleAndRotate>((cmd) =>
            {
                count++;
                Console.WriteLine($"New command detected. Nr: {count}");
            });

            recognition.SubscribeToCommand<GrabCommand>((cmd) =>
            {
                count++;
                Console.WriteLine($"New grab command detected. Nr: {count}");
            });

            recognition.StartRecording();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            program.run();
        }
    }
}
