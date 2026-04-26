using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Common.Entites
{
    public class BaseEntity<Tkey> where Tkey : IEquatable<Tkey>
    {
        public Tkey Id { get; set; }
    }
}
