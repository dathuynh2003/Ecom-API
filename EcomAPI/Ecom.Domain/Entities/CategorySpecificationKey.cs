using Ecom.Domain.Base;

namespace Ecom.Domain.Entities
{
    public class CategorySpecificationKey : BaseEntity
    {
        // composite key (CategoryId, SpecificationKeyId)
        public int CategoryId { get; private set; }
        public int SpecificationKeyId { get; private set; }
        public int DisplayOrder { get; private set; }
        public Category Category { get; private set; }
        public SpecificationKey SpecificationKey { get; private set; }

        public static CategorySpecificationKey Create(int categoryId, int specificationKeyId, int displayOrder)
        {
            return new CategorySpecificationKey
            {
                CategoryId = categoryId,
                SpecificationKeyId = specificationKeyId,
                DisplayOrder = displayOrder
            };
        }

        public void UpdateDisplayOrder(int newDisplayOrder)
        {
            DisplayOrder = newDisplayOrder;
        }
    }
}
