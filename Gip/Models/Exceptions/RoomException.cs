using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models.Exceptions
{
    public class RoomException : Exception
    {
        public RoomException()
        {
            
        }
        public RoomException(string message) : base(message)
        {
            
        }
    }
}
