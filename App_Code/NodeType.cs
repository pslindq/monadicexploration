using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

[Serializable]
public partial class NodeType
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public NodeType()
    {
        Nodes = new HashSet<Node>(); 
    }

    public int NodeTypeID { get; set; }

    public int MonadID { get; set; }

    public int Sequence { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string PluralName { get; set; }

    [Required]
    [StringLength(50)]
    public string SlugName { get; set; }

    [Required]
    [StringLength(6)]
    public string Color { get; set; }

    public virtual Monad Monad { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Node> Nodes { get; set; }
}