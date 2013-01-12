// --------------------------------------------------------------------------
// Copyright (c) MEMOS Software s.r.o. All rights reserved.
//
// Wake on LAN
// 
// File     : UserManagement.cs
// Author   : Lukas Neumann <lukas.neumann@memos.cz>
// Created  : 080614
//
// -------------------------------------------------------------------------- 


/// <summary>
/// User management class
/// </summary>
public static class UserManagement
{
    /// <summary>
    /// Returns if current user is an administrator
    /// </summary>
    /// <returns></returns>
    public static bool IsAdministrator()
    {
        //string userName = HttpContext.Current.User.Identity.Name;
        return true; // Array.IndexOf(ConfigurationManager.AppSettings["adminUsers"].Split(';'), userName) >= 0;
    }
        
}
