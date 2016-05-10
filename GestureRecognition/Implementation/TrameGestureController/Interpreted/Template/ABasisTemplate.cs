using System;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Interface.Commands;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Template
{
    public abstract class ABasisTemplate : ITemplate
    {
        internal readonly IDictionary<JointType, Sequence<Vector3>> Sequences;

        protected ABasisTemplate(string id, string name)
        {
            Id = id;
            DisplayName = name;
            Sequences = new Dictionary<JointType, Sequence<Vector3>>();
        }

        /// <summary>
        /// The id of the gesture
        /// </summary>
        public string Id { get; private set; }

        public string DisplayName { get; private set; }

        public bool HasSequenceFor(JointType jointType)
        {
            return Sequences.ContainsKey(jointType);
        }

        public Sequence<Vector3> GetSequence(JointType jointType)
        {
            return Sequences[jointType];
        }

        public IList<Sequence<Vector3>> GetSequences()
        {
            return Sequences.Values.ToList();
        }

        public int CompareTo(ITemplate other)
        {
            return Id.CompareTo(other.Id);
        }

        public bool Equals(ITemplate other)
        {
            return Id.Equals(other.Id);
        }

        public AInterpretedCommand Command { get; set; }

        public abstract double EndCondition(IDictionary<JointType, InputVector> input);

        public abstract double StartCondition(IDictionary<JointType, InputVector> input);

        public abstract double MotionCharacteristic(IDictionary<JointType, InputVector> input);
    }
}
