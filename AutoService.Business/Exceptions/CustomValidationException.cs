using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Exceptions;

public class CustomValidationException : Exception
{
    public CustomValidationException(string message)
        :base(message)
    {

    }

}
