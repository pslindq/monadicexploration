using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

[Serializable]
public partial class Monad
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Monad()
    {
        NodeTypes = new HashSet<NodeType>();
    }

    public int MonadID { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; }

    [StringLength(250)]
    public string URL { get; set; }

    public bool ShowNodeTypes { get; set; }

    [Required]
    [StringLength(100)]
    public string URLSegment { get; set; }

    [Required]
    [StringLength(500)]
    public string AdminPWD { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<NodeType> NodeTypes { get; set; }
}