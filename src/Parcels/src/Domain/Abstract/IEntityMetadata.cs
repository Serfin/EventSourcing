using System;

namespace Parcels.Domain.Abstract
{
    public interface IEntityMetadata
    {
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
    }
}
