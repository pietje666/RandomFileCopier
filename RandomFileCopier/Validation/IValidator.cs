using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomFileCopier.Validation
{
    interface IValidator<T>
    {
        bool IsValid(T entity);

        string ErrorMessage { get; }
    }
}
