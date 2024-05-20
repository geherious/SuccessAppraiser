

namespace SuccessAppraiser.BLL.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string fieldName)
            : base($"There is no item with provided {fieldName}") 
        {
            
        }
    }
}
