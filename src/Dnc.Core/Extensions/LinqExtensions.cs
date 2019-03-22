﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnc.Extensions
{
    /// <summary>
    /// Extension methods for linq.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Page the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">list</param>
        /// <param name="pageSize">count per page</param>
        /// <param name="action">what to do</param>
        public static void Page<T>(this IEnumerable<T> source,
            int pageSize,
            Action<IEnumerable<T>> action)
        {
            var total = source.Count();
            var pageCount = (total + pageSize - 1) / pageSize;

            for (int i = 1; i <= pageCount; i++)
            {
                var j = i;
                var selected = source.Skip((j - 1) * pageSize).Take(pageSize);//get items by pageNo.

                action?.Invoke(selected);
            }
        }



        /// <summary>
        /// Parallel the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">list</param>
        /// <param name="pageSize">count per page</param>
        /// <param name="action">what to do</param>
        public static string Parallel<T>(this IEnumerable<T> source,
            int pageSize,
            Action<IEnumerable<T>> action)
        {
            var sw = Stopwatch.StartNew();
            var actions = new List<Action>();

            sw.Start();
            source.Page(pageSize,
                selected =>
                {
                    actions.Add(() =>
                    {
                        action?.Invoke(selected);
                    });
                });

            System.Threading.Tasks.Parallel.Invoke(actions.ToArray());
            sw.Stop();

            var log =$"总计处理任务{source.Count()},耗时{sw.ElapsedMilliseconds}ms";
            return log;
        }
    }
}
