using System;

namespace GestureRecognition.Implementation.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// 
        /// </summary>
        event Action<DataContainer> Ready;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        void OnNewData(DataContainer dc);
    }
}
