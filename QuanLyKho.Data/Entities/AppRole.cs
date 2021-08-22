using Microsoft.AspNetCore.Identity;
using System;

namespace QuanLyKho.Data.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public string MoTa { get; set; }
    }
}