using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dualscape.Repository.Exceptions
{
    class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base("Invalid email")
        {

        }
        public InvalidEmailException(string message) : base(message)
        {

        }
        public InvalidEmailException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
