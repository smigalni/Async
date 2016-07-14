﻿using System;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    static class TaskExtensions
    {
        public static void Ignore(this Task task)
        {
        }

        public static async Task IgnoreCancellation(this Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}