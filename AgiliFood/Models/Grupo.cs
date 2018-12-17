using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgiliFood.Models
{
    public class Grupo<K, T>
    {
        public K Key;
        public IEnumerable<T> Values;
    }
}