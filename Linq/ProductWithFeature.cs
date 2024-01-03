using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class ProductWithFeature
    {
        public string Name { get; set; }

        public ICollection<Feature> Features { get; set; } = new List<Feature>();

        public ProductWithFeature(string name, params int[] featId)
        {
            Name = name;
            foreach (int id in featId)
            {
                Features.Add(new Feature(id));
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ProductWithFeature prod = (ProductWithFeature)obj;
            return Name == prod.Name && Features.SequenceEqual(prod.Features);
        }

        public override int GetHashCode() => HashCode.Combine(Name, Features);
    }
}
