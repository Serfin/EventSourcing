using System;

namespace Parcels.Domain.Abstract
{
    public interface IEntity : IEntityMetadata
    {
        public Guid Id { get; set; }
    }
}
