using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Class
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TeacherId { get; set; }

    public virtual ICollection<Resource> Resources { get; } = new List<Resource>();

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
