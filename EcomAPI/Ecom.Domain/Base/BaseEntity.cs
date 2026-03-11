namespace Ecom.Domain.Base
{
    public class BaseEntity : IBaseEntity
    {
        public bool IsDeleted { get; set; } = false;
    }
}
