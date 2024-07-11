using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psm32.Models;

namespace Psm32.Stores;

public class Session
{
    private User? _currentUser;


    public void StartSession(User user)
    {
        _currentUser = user;
    }

    public void EndSession()
    {
        _currentUser = null;
    }

    public bool LoggedIn()
    {
        return _currentUser != null;
    }
}

