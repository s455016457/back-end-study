using System.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Example1.DDD.Entities
{
    [Table("Po")]
    public class Po
    {
        [Key]
        public string PoNum { get; private set; }

        public string VenId { get; private set; }
        
        public decimal TotalAmount { get; private set; }

        public byte[] RowVersion { get; private set; }
    
        public static Po CreateNew(string poNum,string venId,decimal totalAmount){
            return new Po{
                PoNum=poNum,
                VenId=venId,
                TotalAmount=totalAmount
            };
        }

        public void Update(String venId,decimal totalAmount){
            VenId=venId;
            TotalAmount=totalAmount;
        }
    }
}