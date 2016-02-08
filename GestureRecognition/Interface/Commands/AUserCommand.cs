#region usages

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using Trame;

#endregion

namespace GestureRecognition.Interface.Commands
{
    /// <summary>
	/// Base class for all gesture commands from the user.
    /// </summary>
    public abstract class AUserCommand
    {
        protected abstract string InfoDump();

        public string Info => InfoDump();
        public IDictionary<JointType, InputVector> Input { get; set; }
    }
}