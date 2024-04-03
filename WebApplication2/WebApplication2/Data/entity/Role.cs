using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EF;

public partial class Role
{
    public long RoleId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
