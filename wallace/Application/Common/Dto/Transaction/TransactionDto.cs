using System;
using Wallace.Domain.Enums;

namespace Wallace.Application.Common.Dto
{
    public class TransactionDto : IDto
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public Repetition Repetition { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        
        public string PayeeName { get; set; }
        public Guid Payee { get; set; }
        
        public string CategoryName { get; set; }
        public string CategoryEmoji { get; set; }
        public Guid Category { get; set; }
        
        public Guid Owner { get; set; }
        public Guid Account { get; set; }
    }
}