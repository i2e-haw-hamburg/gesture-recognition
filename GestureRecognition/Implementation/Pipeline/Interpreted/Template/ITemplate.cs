using System;
using System.Collections;
using System.Collections.Generic;
using GestureRecognition.Interface.Commands;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Template
{
    public interface ITemplate : IComparable<ITemplate>, IEquatable<ITemplate>
    {
        string Id { get; }
        string DisplayName { get; }
        bool HasSequenceFor(JointType jointType);
        Sequence<Vector3> GetSequence(JointType jointType);
        IList<Sequence<Vector3>> GetSequences();
        AInterpretedCommand Command { get; set; }
        double EndCondition(IDictionary<JointType, InputVector> input);
        double StartCondition(IDictionary<JointType, InputVector> input);
        double MotionCharacteristic(IDictionary<JointType, InputVector> input);
    }
}
