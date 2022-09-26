using Parcels.Domain;
using System;

namespace Parcels.DAL;

public class ParcelProjection
{
    public Guid Id { get; set; }
    public ParcelStatus ParcelStatus { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
}
