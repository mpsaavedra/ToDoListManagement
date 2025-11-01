using Bootler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Entities;

public class Role : Entity
{
    private string _name;
    private ICollection<User> _users = new List<User>();

    public Role()
    {
    }

    public Role(string name, ICollection<User>? users = null)
    {
        users ??= new List<User>();
        Name = name;
        Users = users;
    }
    public string Name 
    { 
        get => _name; 
        set => _name = value; 
    }
    public ICollection<User> Users 
    { 
        get => _users; 
        set => _users = value; 
    }
}
