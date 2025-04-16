using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gee.Core
{
    public abstract class BaseEntity<T1,T2,T3>
    {
        [Key]
        public T1 Id { get; set; }
        public DateTime CreatedDate { get;set;}
        public DateTime UpdatedDate { get;set;}
        public T2? CreatedBy { get; set; }
        public T2? UpdatedBy { get;set; }
    }
    public abstract class TenantBaseEntity<T1, T2, T3>: BaseEntity<T1, T2,T3>
    {
        public T3? TenantId { get; set; }

    }
    public abstract class BaseEntityModel<T1, T2, T3> 
    {
        [Key]
        public T1 Id { get; set; }
        public T3? TenantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public T2? CreatedBy { get; set; }
        public T2? UpdatedBy { get; set; }

    }
}
