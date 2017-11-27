using System.Globalization;
using RandomFileCopier.Properties;

namespace RandomFileCopier.Validation
{
    class StringToDoubleValidator
        : IValidator<string>
    {
        public string ErrorMessage
        {
            get
            {
                return Resources.IncorrectDoubleFormat;
            }
        }

        public bool IsValid(string entity)
        {
            var result = false;
            double parseResult = -1;
            result = double.TryParse(entity, NumberStyles.Any, CultureInfo.CurrentUICulture , out parseResult);
            return result;
        }
    }
}
