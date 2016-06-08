using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

[Serializable]
public partial class Node
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Node()
    {
        NodeLinks1 = new HashSet<NodeLink>();
        NodeLinks2 = new HashSet<NodeLink>();
    }

    public int NodeID { get; set; }

    public int NodeTypeID { get; set; }

    public int Sequence { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; }

    [Required]
    public string Text { get; set; }

    [StringLength(250)]
    public string URL { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<NodeLink> NodeLinks1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<NodeLink> NodeLinks2 { get; set; }

    public virtual NodeType NodeType { get; set; }
}