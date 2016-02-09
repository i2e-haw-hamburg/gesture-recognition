using System;

namespace GestureRecognition.Interface.Commands
{
    public abstract class AInterpretedCommand : AUserCommand
    {
        public string Id { get; set; }
        public string Name { get; set; }

        protected AInterpretedCommand(string id, string name)
        {
            Id = id;
            Name = name;
        }

        protected AInterpretedCommand()
        {
        }

        public double Probability { get; set; }

        protected override string InfoDump()
        {
            return $"{Name}({Id}) at {DateTime.Now}";
        }
    }
}