#region usages

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trame;

#endregion

namespace GestureRecognition.Interface
{
    #region usages

    using GestureRecognition.Interface.Commands;

    #endregion

    /// <summary>
    /// Provides functionality for getting data about performed gestures from the user.
    /// </summary>
    public interface IGestureRecognition
    {
        
        #region Public Methods and Operators
        /// <summary>
        /// 
        /// </summary>
        void Stop();

        /// <summary>
        /// Subscribes the listener for the specified Command. The specified listener delegate will be called, if the user
        /// executes a gesture that is mapped to the specified <see cref="AUserCommand" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of Command (<see cref="AUserCommand" />) the <see cref="commandListener" /> Delegate is
        /// subscribed to.
        /// </typeparam>
        /// <param name="commandListener">The delegate that is subscribed for the specified Command.</param>
        void SubscribeToCommand<T>(Action<T> commandListener) where T : AUserCommand;

        /// <summary>
        /// Unsubscribes the specified listener Delegate from the specified type of Command. The specified listener delegate
        /// will no longer be called, if the user executes a gesture that is mapped to the specified <see cref="AUserCommand" />.
        /// </summary>
        /// <typeparam name="T"> The type of Command the listener will be unsubscribed from. </typeparam>
        /// <param name="commandListener"> The listener delegate that will be unsubscribed. </param>
        void UnsubscribeFromCommand<T>(Action<T> commandListener) where T : AUserCommand;

        /// <summary>
        /// Action for receiving new skeletons.
        /// 
        /// The recognition doesn't differentiate between users.
        /// </summary>
        /// <param name="skeleton">a new skeleton</param>
        void OnNewSkeleton(ISkeleton skeleton);

        #endregion
    }
}