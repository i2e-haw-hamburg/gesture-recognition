﻿using System;
using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;
using Leap;

namespace GestureRecognition.Implementation
{
    internal interface IDecider
    {
        void Decide(IEnumerable<Result> motions);

        event Action<AInterpretedCommand> NewInterpretedCommand;
    }
}