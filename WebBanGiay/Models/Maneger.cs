//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBanGiay.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Maneger
    {
        public int IDMan { get; set; }
        public string TenQuanLy { get; set; }
        public int IDAccount { get; set; }
    
        public virtual Account Account { get; set; }
    }
}