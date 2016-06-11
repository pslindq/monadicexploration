using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

[Serializable]
public partial class NodeLink 
{
    public int NodeLinkID { get; set; }

    public int NodeID1 { get; set; }

    public int NodeID2 { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [StringLength(21)]
    public string UniqueID { get; set; }

    public virtual Node Node { get; set; }

    public virtual Node Node1 { get; set; }
}