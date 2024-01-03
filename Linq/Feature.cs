using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class Feature
    {
        public int Id { get; set; }

        public Feature(int id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Feature feature = (Feature)obj;
            return Id == feature.Id;
        }

        public override int GetHashCode() => HashCode.Combine(Id);
    }
}