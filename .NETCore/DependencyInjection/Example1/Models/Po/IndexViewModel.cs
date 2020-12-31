using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Example1.DDD.Repostory;


namespace Example1.Models.Po
{
    public class IndexViewModel
    {
        public IList<PoModel> Pos { get; set; }
        
        private MyDbContext _context;
        public IndexViewModel Init(MyDbContext context){
            _context=context;
            return this;
        }

        public IndexViewModel Search(){
            Pos = _context.PoSet
                .Select(p=>new PoModel{PoNum=p.PoNum,VenId=p.VenId,TotalAmount=p.TotalAmount,RowVersion=BitConverter.ToString(p.RowVersion)})
                .ToList();
            return this;
        }
    }
}