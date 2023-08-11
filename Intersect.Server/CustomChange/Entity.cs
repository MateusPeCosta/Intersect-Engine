using System;

namespace Intersect.Server.Entities
{

    public abstract partial class Entity : IDisposable
    {
        public bool IsRunning = false;
    }
}
