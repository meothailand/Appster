using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwinkleStars.Infrastructure.Utils
{
    public class ModelObjectHelper
    {
        public static TDestination CopyObject<TSource, TDestination>(TSource source, TDestination destination) 
               where TSource : class
               where TDestination : class
        {
            var sourceProps = source.GetType().GetProperties().OrderBy(i => i.Name).ToList();
            var destinationProps = destination.GetType().GetProperties().OrderBy(i => i.Name).ToList();
            foreach (var prop in sourceProps)
            {
                var destProp = destinationProps.SingleOrDefault(i => i.Name == prop.Name && i.PropertyType == prop.PropertyType);
                if (destProp == null) continue;
                destProp.SetValue(destination, prop.GetValue(source));
            }
            return destination;
        }

        public static TDestination ObjectUpdateCopy<TSource, TDestination>(TSource source, TDestination destination, string[] passbyValues)
            where TSource: class
            where TDestination : class
        {
            var sourceProps = source.GetType().GetProperties().OrderBy(i => i.Name).ToList();
            var destinationProps = source.GetType().GetProperties().OrderBy(i => i.Name).ToList();
            if (passbyValues != null)
            {
                passbyValues.OrderBy(i => i);
                sourceProps = sourceProps.Where(i => Array.IndexOf(passbyValues,i.Name) < 0).ToList();
                destinationProps = destinationProps.Where(i => Array.IndexOf(passbyValues, i.Name) < 0).ToList();
            }

            foreach (var prop in sourceProps)
            {
                var desProp = destinationProps.SingleOrDefault(i => i.Name == prop.Name);
                if (desProp == null || desProp.GetValue(destination) == prop.GetValue(source)) continue;
                desProp.SetValue(destination, prop.GetValue(source));
            }
            return destination;
        }
    }
}