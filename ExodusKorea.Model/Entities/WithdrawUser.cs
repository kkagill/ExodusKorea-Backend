using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class WithdrawUser
    {
        public long WithdrawUserId { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Reason { get; set; }
        public DateTime DateWithdrew { get; set; }
    }
}