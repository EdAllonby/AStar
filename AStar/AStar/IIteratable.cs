using System;

namespace AStar
{
    public interface IIteratable<T> where T : EventArgs
    {
        event EventHandler<T> IterationComplete;
    }
}