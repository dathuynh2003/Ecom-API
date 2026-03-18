using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class SpecificationKey : BaseEntity
    {
        public int Id { get; private set; }
        public string? Name { get; private set; } = string.Empty;
        public string? Unit { get; private set; } = string.Empty;
        public ICollection<CategorySpecificationKey> CategorySpecificationKeys { get; private set; } = new List<CategorySpecificationKey>();
        public ICollection<ProductSpecificationKey> ProductSpecificationKeys { get; private set; } = new List<ProductSpecificationKey>();

        public static SpecificationKey Create(string name, string? unit)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(unit))
            {
                throw new ArgumentException("Invalid data");
            }
            return new SpecificationKey
            {
                Name = name,
                Unit = unit
            };
        }

        public void Update(string name, string? unit)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(unit))
            {
                throw new ArgumentException("Invalid data");
            }
            Name = name;
            Unit = unit;
        }
    }
}
