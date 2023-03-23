using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual ICollection<Class> Classes { get; } = new List<Class>();
}
