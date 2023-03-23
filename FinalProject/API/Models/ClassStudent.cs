using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ClassStudent
{
    public int ClassId { get; set; }

    public string StudentId { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
