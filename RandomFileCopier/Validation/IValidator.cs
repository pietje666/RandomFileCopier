using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//github showcase nico 22-08
namespace RandomFileCopier.Validation
{
    interface IValidator<T>
    {
        bool IsValid(T entity);

        string ErrorMessage { get; }
    }
}
