using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Inmersys.Domain.DB.Extensions
{
    public static class DBContext_Extensions_Methods
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> nav_paths)
            where T : class
        {
            return nav_paths.Aggregate(source, (query, path) => query.Include(path));
        }

        public static IEnumerable<string> GetThreePath(this DbContext context, Type clrEntityType)
        {
            IEntityType? entityType = context.Model.FindEntityType(clrEntityType);
            HashSet<INavigation> includedNavigations = new HashSet<INavigation>();
            Stack<IEnumerator<INavigation>> stack = new Stack<IEnumerator<INavigation>>();
            while (true)
            {
                List<INavigation> entityNavigations = new List<INavigation>();
                if (stack.Count <= int.MaxValue)
                {
                    foreach (INavigation navigation in entityType.GetNavigations())
                    {
                        if (includedNavigations.Add(navigation)) entityNavigations.Add(navigation);
                    }
                }
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0) yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                }
                else
                {
                    foreach (INavigation navigation in entityNavigations)
                    {
                        INavigation? inverseNavigation = navigation.Inverse;
                        if (inverseNavigation != null) includedNavigations.Add(inverseNavigation);
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }
                while (stack.Count > 0 && !stack.Peek().MoveNext()) stack.Pop();
                if (stack.Count == 0) break;
                entityType = stack.Peek().Current.TargetEntityType;
            }
        }
    }
}
